using Detached.Mappers;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel;
using GraphInheritenceTests.DTOs;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug17;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug18;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance;
using static System.Net.Mime.MediaTypeNames;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec
{
    [TestFixture]
    public class ComplexDeepModelBugReproTests
    {
        private Customer _superCustomer;
        private Tag _tag2;

        [SetUp]
        public void BeforeEachTest()
        {
            using (var dbContext = new ComplexDbContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                foreach (CustomerKindId customerKindId in Enum.GetValues<CustomerKindId>())
                {
                    CustomerKind customerKind = new CustomerKind() { Id = customerKindId, Name = customerKindId.GetFriendlyName() };
                    dbContext.CustomerKinds.Add(customerKind);
                }

                Country countryDE = new Country()
                {
                    Name = "Germany",
                    IsoCode = "DE",
                };

                var addressIngolstadt = new Address()
                {
                    Street = "Hauptstrasse",
                    PostalCode = "85049",
                    City = "Ingolstadt",
                    Country = countryDE
                };

                var addressMunich = new Address()
                {
                    Street = "Terminalstrasse Mitte",
                    PostalCode = "85445",
                    City = "Oberding",
                    Country = countryDE
                };

                var tag1 = new Tag() { Name = "SuperPlus" };
                _tag2 = new Tag() { Name = "Marketing Campaign1" };

                _superCustomer = new Customer()
                {
                    CustomerKindId = CustomerKindId.Company,
                    CustomerName = "Super Customer",
                    PrimaryAddress = addressIngolstadt,
                    ShipmentAddress = addressMunich,
                };
                _superCustomer.Tags = new List<Tag>();
                _superCustomer.Tags.Add(tag1);
                _superCustomer.Tags.Add(_tag2);

                dbContext.Customers.Add(_superCustomer);

                dbContext.SaveChanges();
            }

            // Save again with plain EF without any changes
            using (var dbContext2 = new ComplexDbContext())
            {
                dbContext2.Update(_superCustomer);
                // No exception expected - OK
                dbContext2.SaveChanges();
            }
        }

        [Test]
        public void _01_AChangeOnCustomerKindId_ShouldSaveWithoutModifyCustomerKindEntity()
        {
            _superCustomer.CustomerName = "Super Customer - Changed to incomplete private";

            // INFO: the following in combination works, too :-)
            // superCustomer.CustomerKindId = CustomerKindId.Private;
            // superCustomer.CustomerKind = new CustomerKind() { Id = CustomerKindId.Private };
            // INFO: CustomerKind is has the [Aggregation] attribute
            _superCustomer.CustomerKind = new CustomerKind() { Id = CustomerKindId.Private };

            using (var dbContext = new ComplexDbContext())
            {
                var mapped = dbContext.Map<Customer>(_superCustomer);

                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                var loadedCustomer = dbContext.Customers
                    .Include(c => c.PrimaryAddress)
                    .Include(c => c.ShipmentAddress)
                    .Include(c => c.Tags)
                    .Include(c => c.CustomerKind)
                    .First();
                Assert.That(loadedCustomer.CustomerName, Is.EqualTo("Super Customer - Changed to incomplete private"), "No change would be saved. Maybe it's because it exists only in the concrete type.");
                Assert.That(loadedCustomer.CustomerKind.Id, Is.EqualTo(CustomerKindId.Private));

                // The name should stay at is was, although it was empty by the client
                Assert.That(loadedCustomer.CustomerKind.Name, Is.EqualTo("Private Customer"));

                Assert.That(loadedCustomer.PrimaryAddress.City, Is.EqualTo("Ingolstadt"));
                Assert.That(loadedCustomer.ShipmentAddress.City, Is.EqualTo("Oberding"));
                Assert.That(loadedCustomer.Tags, Has.Count.EqualTo(2));
                Assert.That(loadedCustomer.Tags.Select(t => t.Name), Contains.Item("SuperPlus"));
                Assert.That(loadedCustomer.Tags.Select(t => t.Name), Contains.Item("Marketing Campaign1"));
            }
        }

        [Test]
        public void _02_AChangeOnAggregationCountry_ShouldBeIgnored()
        {
            _superCustomer.PrimaryAddress = new Address() { Id = 1, Country = new Country() { Name = "changed" } };
            _superCustomer.NewAddress = new Address() { City = "NEW", Country = new Country() { Id = 1 } };

            using (var dbContext = new ComplexDbContext())
            {
                // preferred suggested way
                var mapped = dbContext.Map<OrganizationBase>(new
                {
                    _superCustomer.Id,
                    _superCustomer.OrganizationType,
                    _superCustomer.PrimaryAddress,
                    _superCustomer.NewAddress,
                    _superCustomer.ConcurrencyToken
                });

                // works, too
                // var mappedOrgaList = dbContext.Map<OrganizationBase>(superCustomer);

                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                Assert.That(dbContext.Countries.Count, Is.EqualTo(1));

                var germanyReLoaded = dbContext.Countries.First();
                // country as aggregate shouldn't be changed
                Assert.That(germanyReLoaded.Name, Is.Not.EqualTo("changed"));
                Assert.That(germanyReLoaded.Name, Is.EqualTo("Germany"));

                Customer loadedCustomer = dbContext.Customers
                    .Include(c => c.NewAddress)
                    .Single(c => c.Id == _superCustomer.Id);
                Assert.That(loadedCustomer.NewAddress.City, Is.EqualTo("NEW"));
                Assert.That(loadedCustomer.NewAddress.Country.Name, Is.EqualTo(germanyReLoaded.Name));
            }
        }

        [Test]
        public void _03_RemoveChangeAndAddEntriesInTagListComposition_ShouldBeSupportedInSave()
        {
            _superCustomer.Tags = new List<Tag>
            {
                // Tag1 removed - will not be sent back by client
                new Tag() { Id = _tag2.Id, Name = "Changed Marketing Campaign1", ConcurrencyToken = 2 },
                new Tag() { Id = 0, Name = "new Tag" },
                new Tag() { Id = 0, Name = "new Tag2" }
            };

            using (var dbContext = new ComplexDbContext())
            {
                // dbContext.Update(superCustomer); // EF doesn't support change tracking with removed items as we know

                // Leonardo Porro suggests to use an anonymous type
                var mapped = dbContext.Map<OrganizationBase>(new
                {
                    _superCustomer.Id,
                    _superCustomer.OrganizationType,
                    _superCustomer.Tags
                });

                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                var allOrganizations = dbContext.Organizations.Include(c => c.Tags).ToList();
                Assert.That(allOrganizations, Has.Count.EqualTo(1));

                Customer loadedSuperCustomer = allOrganizations.OfType<Customer>().Single(c => c.CustomerName.Contains("Super Customer"));
                Assert.That(loadedSuperCustomer.Tags, Has.Count.EqualTo(3));
                Assert.That(loadedSuperCustomer.Tags.Select(t => t.Name), Does.Not.Contains("Marketing Campaign1"));
                Assert.That(loadedSuperCustomer.Tags.Select(t => t.Name), Contains.Item("Changed Marketing Campaign1"));
                Assert.That(loadedSuperCustomer.Tags.Select(t => t.Name), Contains.Item("new Tag"));
            }
        }

        [Test]
        public void _04_DoChangeOnCompositionOrganizationNotesWithBackReferenceOrganizationId_ShouldNotBeSetInDTO()
        {
            // INFO: Back-references don't work with Map!
            _superCustomer.Notes.Add(new OrganizationNotes()
            {
                Date = DateTime.Today,
                Text = "Note...",
                //OrganizationId = superCustomer.Id // is allowed in entity, but mustn't be set from Frontend
            });

            // so you have to use a DTO or an anonymous type in the test
            int id;
            using (var dbContext = new ComplexDbContext())
            {
                var mapped2 = dbContext.Map<OrganizationBase>(new
                {
                    OrganizationType = nameof(Customer),
                    CustomerKindId = CustomerKindId.Company,
                    CustomerName = "New new Customer",
                    PrimaryAddressId = 1,
                    Notes = new[]
                    {
                        new
                        {
                            // no OrganizationId!
                            Date = DateTime.Today,
                            Text = "Note..."
                        }
                    }
                });
                dbContext.SaveChanges();
                id = mapped2.Id;
            }

            using (var dbContext = new ComplexDbContext())
            {
                var customerLoaded = dbContext.Customers
                    .Include(c => c.Notes)
                    .Single(c => c.Id == id);

                Assert.That(customerLoaded.Notes, Has.Count.EqualTo(1));
                Assert.That(customerLoaded.Notes[0].Date, Is.EqualTo(DateTime.Today));
                Assert.That(customerLoaded.Notes[0].Text, Is.EqualTo("Note..."));
                Assert.That(customerLoaded.Notes[0].Organization.Id, Is.EqualTo(id));
            }

            // ... if there is no other reference, then the back reference does work
            OrganizationNotes note1 = new OrganizationNotes() { OrganizationId = _superCustomer.Id, Date = new DateTime(2000, 05, 10), Text = "Note for super customer" };
            using (var dbContext = new ComplexDbContext())
            {
                note1 = dbContext.Map<OrganizationNotes>(note1);
                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                Customer customer1Loaded = dbContext.Customers.Include(o => o.Notes).SingleOrDefault(c => c.CustomerName == _superCustomer.CustomerName);

                Assert.That(customer1Loaded.Notes.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void _05_DoChangeOnParentChildrenTreeOrHierarchy_ShouldBePossibleWithDetachedMappersToo()
        {
            //doesn't work - parent gets lost (removed)
            // Seed first
            Customer parent = new Customer() { Name = "Parent", PrimaryAddressId = 1, CustomerKindId = CustomerKindId.Company };
            Customer child = new Customer() { Name = "Child", PrimaryAddressId = 1, CustomerKindId = CustomerKindId.Company };
            Customer childChild = new Customer() { Parent = child, Name = "ChildChild", PrimaryAddressId = 1, CustomerKindId = CustomerKindId.Company };
            child.Children.Add(childChild);
            using (var dbContext = new ComplexDbContext())
            {
                var mapped1 = dbContext.Add<Customer>(parent);
                var mapped2 = dbContext.Add<Customer>(child);
                var mapped3 = dbContext.Add<Customer>(childChild);

                dbContext.SaveChanges();
            }

            child.ParentId = parent.Id;
            // Link tree as aggregation
            //child.Parent = new Customer { Id = parent.Id };
            parent.Children.Add(child);

            using (var dbContext = new ComplexDbContext())
            {
                //dbContext.Update(parent); // works - expected
                //dbContext.Update(child); // works - expected

                // if the whole entities are used - parent gets lost (removed)
                // so the workaround with anonymous type is working
                var mapped1 = dbContext.Map<OrganizationBase>(new
                {
                    parent.Id,
                    parent.OrganizationType,
                    parent.ParentId,
                    parent.Parent,
                    parent.Children
                });
                dbContext.SaveChanges();

                var mapped2 = dbContext.Map<OrganizationBase>(new
                {
                    child.Id,
                    child.OrganizationType,
                    child.ParentId,
                    child.Parent,
                    child.Children
                });
                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                var allCustomers = dbContext.Customers;
                var loadedHierarchy = allCustomers
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .AsEnumerable();

                Assert.That(loadedHierarchy.Select(c => c.Name), Contains.Item("Parent"), "Parent gets lost (removed)?!");
                Customer parentLoaded = loadedHierarchy.Single(c => c.Name == "Parent");

                Assert.That(parentLoaded.Children, Has.Count.EqualTo(1));
                Assert.That(parentLoaded.Children[0].Name, Is.EqualTo("Child"));
                Assert.That(parentLoaded.Children[0].Children, Has.Count.EqualTo(1));
                Assert.That(parentLoaded.Children[0].Children[0].Name, Is.EqualTo("ChildChild"));
            }
        }

        [Test]
        public void _06_MapOnBaseType_ShouldSupportAllDiscriminatorValuesAndConcreteTypeProperties()
        {
            var customer1 = new
            {
                OrganizationType = nameof(Customer),
                Name = "Customer1",
                CustomerKindId = CustomerKindId.Company,
                CustomerName = "Customer1",
                PrimaryAddressId = _superCustomer.PrimaryAddressId
            };

            var government1 = new
            {
                OrganizationType = nameof(Government),
                Name = "Government1",
                GovernmentIdentifierCode = "ABC",
                PrimaryAddressId = _superCustomer.PrimaryAddressId
            };

            var subGovernment1 = new
            {
                OrganizationType = nameof(SubGovernment),
                Name = "SubGovernment1",
                GovernmentIdentifierCode = "DEF",
                SubName = "DEF-123",
                PrimaryAddressId = _superCustomer.PrimaryAddressId
            };

            using (var dbContext = new ComplexDbContext())
            {
                dbContext.Map<OrganizationBase>(customer1);
                dbContext.Map<OrganizationBase>(government1);
                dbContext.Map<OrganizationBase>(subGovernment1); // it seems to be, that a deeper hierarchy doesn't work now
                dbContext.SaveChanges();
            }

            int customer1Id, government1Id, subGovernment1LoadedId;
            using (var dbContext = new ComplexDbContext())
            {
                Customer customer1Loaded = dbContext.Customers.SingleOrDefault(c => c.Name == "Customer1");
                customer1Id = customer1Loaded.Id;
                Assert.That(customer1Loaded, Is.Not.Null);
                Assert.That(customer1Loaded.CustomerName, Is.EqualTo("Customer1"));

                Government government1Loaded = dbContext.Governments.SingleOrDefault(g => g.Name == "Government1");
                government1Id = government1Loaded.Id;
                Assert.That(government1Loaded, Is.Not.Null);
                Assert.That(government1Loaded.GovernmentIdentifierCode, Is.EqualTo("ABC"));

                SubGovernment subGovernment1Loaded = dbContext.SubGovernments.SingleOrDefault(g => g.Name == "SubGovernment1");
                subGovernment1LoadedId = subGovernment1Loaded.Id;
                Assert.That(subGovernment1Loaded, Is.Not.Null);
                Assert.That(subGovernment1Loaded.SubName, Is.EqualTo("DEF-123"));
            }
        }

        [Test]
        public void _07_AttachedExistingEntityInCompositionDoesInsertButShouldDoUpdate()
        {
            TodoItem todoItem1 = new TodoItem()
            {
                Title = "TodoItem1",
                ReusedLinkedItems = new List<ReusedLinkedItem>()
                {
                    new ReusedLinkedItem()
                    {
                        Title = "SubTodoItem1",
                        UploadedFiles = null
                    }
                }
            };

            // file will be uploaded seperately and will be inserted 
            var newFile1 = new UploadedFile() { FileTitle = "File1" };


            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var mapped1 = dbContext.Map<TodoItem>(todoItem1);
                var mapped2 = dbContext.Map<UploadedFile>(newFile1);
                dbContext.SaveChanges();
            }


            // --------------- edit -----------------------

            var updated = new
            {
                Id = 1,
                Title = "TodoItem1",
                ReusedLinkedItems = new[]
                {
                    new
                    {
                        Id = 1,
                        Title = "SubTodoItem1",
                        UploadedFiles = new[]
                        {
                            new
                            {
                                // existing file will be added (and modified with additional properties)
                                Id = 1,
                                FileTitle = "File1 changed",
                                IsShared = true
                            }
                        }
                    }
                },
            };
            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                // AssociateExistingCompositions = true - otherwise you get an UNIQUE constraint error
                var mapped = dbContext.Map<TodoItem>(updated, new MapParameters { AssociateExistingCompositions = true });
                dbContext.SaveChanges();
            }

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var todoItemLoaded = dbContext.TodoItems
                    .Include(t => t.ReusedLinkedItems)
                    .ThenInclude(s => s.UploadedFiles)
                    .First();

                Assert.That(todoItemLoaded.ReusedLinkedItems[0].UploadedFiles, Has.Count.EqualTo(1));
                Assert.That(todoItemLoaded.ReusedLinkedItems[0].UploadedFiles[0].FileTitle, Is.EqualTo("File1 changed"));
                Assert.That(todoItemLoaded.ReusedLinkedItems[0].UploadedFiles[0].IsShared, Is.True);
            }
        }

        [Test]
        public void _08_OptionalOneToOne_ShouldNotTryToLoadNullId()
        {
            var austria = new Country() { IsoCode = "AT", Name = "Austria", FlagPicture = new Picture() { FileName = "rotWeissRot.png" } };
            var argentina = new Country() { IsoCode = "AR", Name = "Argentina", FlagPicture = null };

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                dbContext.Countries.Add(austria);
                dbContext.Countries.Add(argentina);
                dbContext.SaveChanges();
            }

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                IQueryable<CountryDTO> projection = dbContext.Project<Country, CountryDTO>(dbContext.Countries);

                CountryDTO austriaLoaded = projection.Single(c => c.IsoCode == "AT");
                Assert.That(austriaLoaded.Name, Is.EqualTo("Austria"));
                Assert.That(austriaLoaded.FlagPictureId, Is.GreaterThan(0));
                Assert.That(austriaLoaded.FlagPicture, Is.Not.Null);
                Assert.That(austriaLoaded.FlagPicture.FileName, Is.EqualTo("rotWeissRot.png"));

                CountryDTO argentinaLoaded = projection.Single(c => c.IsoCode == "AR"); //System.InvalidOperationException : Nullable object must have a value.
                                                                                        // The origin is the empty FlagPicture - which is OK - but it tries to load a null FlagPictureId
                Assert.That(argentinaLoaded.Name, Is.EqualTo("Argentina"));
                Assert.That(argentinaLoaded.FlagPictureId, Is.Null);
                Assert.That(argentinaLoaded.FlagPicture, Is.Null);
            }
        }

        [Test]
        public void _09_MappedElements_ShouldNotBeDuplicated()
        {
            Customer customerOne = new Customer() { CustomerName = "1", PrimaryAddressId = 1 };
            Customer customerTwo = new Customer() { CustomerName = "2", PrimaryAddressId = 1 };

            int customerOneId, customerTwoId;
            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                dbContext.Customers.Add(customerOne);
                dbContext.Customers.Add(customerTwo);
                dbContext.SaveChanges();
                customerOneId = customerOne.Id;
                customerTwoId = customerTwo.Id;
            }

            var twoChangedCustomer = new
            {
                Id = customerTwoId,
                CustomerName = "2",
                //PrimaryAddressId = 1, // can be left out with DTO - will not be changed
                Recommendations = new[]
                {
                    new
                    {
                        RecommendedById = customerTwoId,
                        RecommendedToId = customerOneId,
                        RecommendationDate = new DateTime(2022, 05, 19)
                    }
                }
            };

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var mapped = dbContext.Map<Customer>(twoChangedCustomer);

                Assert.That(mapped.Recommendations, Has.Count.EqualTo(1), "EntityinheritanceBaseOneDTO reference has been duplicated, but shouldn't!");

                dbContext.SaveChanges();
            }
        }

        [Test]
        public void _10_TwoPropertiesToSameEntity_ShouldNotOverwriteSecondWithFirstValue()
        {
            Customer newCustomer = new Customer() { CustomerName = "no customer now", PrimaryAddressId = 1 };
            Customer fanCustomer = new Customer() { CustomerName = "fan", PrimaryAddressId = 1 };

            int newCustomerId, fanCustomerId;
            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                dbContext.Customers.Add(newCustomer);
                dbContext.Customers.Add(fanCustomer);
                dbContext.SaveChanges();
                newCustomerId = newCustomer.Id;
                fanCustomerId = fanCustomer.Id;
            }

            var fanChangedCustomer = new
            {
                Id = fanCustomerId,
                CustomerName = "fan",
                //PrimaryAddressId = 1, // can be left out with DTO - will not be changed
                Recommendations = new[]
                {
                    new
                    {
                        RecommendedById = fanCustomerId,
                        RecommendedToId = newCustomerId,
                        RecommendationDate = new DateTime(2022, 05, 19)
                    }
                }
            };

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var mapped = dbContext.Map<Customer>(fanChangedCustomer);
                dbContext.SaveChanges();
            }

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                Customer fanLoaded = dbContext.Customers
                    .Include(c => c.Recommendations)
                    .Single(c => c.CustomerName == "fan");

                Assert.That(fanLoaded.Recommendations[0].RecommendedById, Is.EqualTo(fanCustomerId));

                Assert.That(fanLoaded.Recommendations[0].RecommendedToId, Is.EqualTo(newCustomerId), "The Id is the same as itself or RecommendedById. The changed value got lost.");
                Assert.That(fanLoaded.Recommendations[0].RecommendedToId, Is.Not.EqualTo(fanLoaded.Recommendations[0].RecommendedById));
            }
        }

        [Test]
        public void _11_ProjectionToDTO_ShouldNotFailIfPropertyIsNotInDTO()
        {
            var germany = new Country() { IsoCode = "DEU", Name = "Germany", FlagPicture = new Picture() { FileName = "schwarzRotGold.png" } };

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                dbContext.Countries.Add(germany);
                dbContext.SaveChanges();
            }

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                // System Null Reference Exception: 
                // We Projecting a EntityinheritanceBaseOneDTO on a DTO which has not all Properties on it.
                // The type of the missing property doesnt matter (Tested with primitive, lists and complex types).
                IQueryable<CountryDTOWithoutPicture> dtosQuery = dbContext.Project<Country, CountryDTOWithoutPicture>(dbContext.Countries);
                var dto = dtosQuery.SingleOrDefault(item => item.IsoCode == "DEU");
                Assert.That(dto, Is.Not.Null);
            }
        }

        [Test]
        public void _12_MappingManyToManyRelation_ShouldNotFail()
        {
            var karen = new Student() // Karen is used later
            {
                Name = "Karen",
                Age = 22
            };

            var mike = new Student()
            {
                Name = "Mike",
                Age = 25
            };

            var math = new Course()
            {
                CourseName = "Maths",
                ClassRoomNumber = 314,
                Students = new()
            };

            math.Students.Add(mike);

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                dbContext.Courses.Add(math);
                dbContext.Students.Add(karen);
                dbContext.SaveChanges();
            }

            // Ok... Seed completed. Now a new DTO comes from the frontend (Completely unchanged).

            var mikeDTO = new StudentDTO()
            {
                Id = 1,
                Name = "Mike",
                Age = 25,
                ConcurrencyToken = 1
            };

            var mathDTO = new CourseDTO()
            {
                Id = 1,
                CourseName = "Math",
                ClassRoomNumber = 314,
                ConcurrencyToken = 1,
                Students = new()
            };

            mathDTO.Students.Add(mikeDTO);

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var mapped = dbContext.Map<Course>(mathDTO);
                // on a many to many relation Detached Mappers thinks, the student is added again even though thats not the case.
                Assert.DoesNotThrow(() => dbContext.SaveChanges());
            }

            // just for the sake of testing we try adding a new student

            var karenDTO = new StudentDTO()
            {
                Name = "Karen",
                Age = 22,
                Id = 2
            };

            mathDTO.Students.Add(karenDTO);

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var mapped = dbContext.Map<Course>(mathDTO);
                Assert.That(mapped.Students.Count(), Is.EqualTo(2));
                Assert.DoesNotThrow(() => dbContext.SaveChanges());
            }

            using (ComplexDbContext dbContext = new ComplexDbContext())
            {
                var courseFromDB = dbContext.Courses.Include(c => c.Students).SingleOrDefault(c => c.Id == 1);
                Assert.That(courseFromDB.Students.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public void _13_ProjectInheritanceWithinGraph_ShouldNotLoadOnlyBaseType()
        {
            // graph root
            var organizationList = new OrganizationListDTO()
            {
                ListName = "Europe",
            };

            OrganizationListDTO dto;
            using (var dbContext = new ComplexDbContext())
            {
                dbContext.Map<OrganizationList>(organizationList);
                dbContext.SaveChanges();

                var query = dbContext.OrganizationLists.Include(o => o.Organizations);
                var projected = dbContext.Project<OrganizationList, OrganizationListDTO>(query);
                dto = projected.First();
            }

            // Organizations is type of OrganisationBase
            // It inherits like this: OrganisationBase -> Government -> SubGovernment
            dto.Organizations.Add(new GovernmentDTO()
            {
                GovernmentIdentifierCode = "DE",
                OrganizationType = nameof(Government),
                PrimaryAddressId = 1,
                Name = "Germany",
            });

            dto.Organizations.Add(new SubGovernmentDTO()
            {
                GovernmentIdentifierCode = "AT",
                OrganizationType = nameof(SubGovernment),
                PrimaryAddressId = 1,
                Name = "Austria",
                SubName = "S�dtirol",
            });

            OrganizationListDTO dto2;
            using (var dbContext = new ComplexDbContext())
            {
                var mappedOrgaList = dbContext.Map<OrganizationList>(dto);
                Assert.That((mappedOrgaList.Organizations[0]), Is.TypeOf<Government>());
                Assert.That((mappedOrgaList.Organizations[1]), Is.TypeOf<SubGovernment>());

                // Detached.Mappers has no chance to know to which type to project to.
                // Idea: Introduce a new InheritanceDiscriminator attribute - here you can define the projection DTOs in the graph DTO
                Assert.That(((Government)mappedOrgaList.Organizations[0]).GovernmentIdentifierCode, Is.EqualTo("DE"), "Property of concrete type Government gets lost because of mapping only to base type");
                Assert.That(((Government)mappedOrgaList.Organizations[1]).GovernmentIdentifierCode, Is.EqualTo("AT"), "Property of concrete type Government gets lost because of mapping only to base type");

                dbContext.SaveChanges();

                var query = dbContext.OrganizationLists.Include(o => o.Organizations);
                var projected = dbContext.Project<OrganizationList, OrganizationListDTO>(query);
                dto2 = projected.First();
            }

            Assert.That(dto2.Organizations[1], Is.TypeOf<SubGovernmentDTO>());
            Assert.That(((SubGovernmentDTO)dto2.Organizations[1]).Name, Is.EqualTo("Austria"));
            Assert.That(((SubGovernmentDTO)dto2.Organizations[1]).SubName, Is.EqualTo("S�dtirol"));
        }

        [Test]
        public void _14_1_UpdateWithOldConcurrencyToken_WithPureEFCore_ShouldThrowDbUpdateConcurrencyException()
        {
            Student originalStudent;
            using (var dbContext = new ComplexDbContext())
            {
                originalStudent = new Student()
                {
                    Age = 12,
                    Name = "Chuck Norris"
                };

                dbContext.Add(originalStudent);
                dbContext.SaveChanges();
            }

            var client1Change = new Student()
            {
                Id = originalStudent.Id,
                Name = "Changed Name",
                ConcurrencyToken = originalStudent.ConcurrencyToken,
                Age = 15
            };

            var client2ChangeInSameTime = new Student()
            {
                Id = originalStudent.Id,
                Name = "other changed name",
                ConcurrencyToken = originalStudent.ConcurrencyToken,
                Age = 82
            };

            using (var dbContext2 = new ComplexDbContext())
            {
                dbContext2.Update(client1Change);
                dbContext2.SaveChanges();
            }

            using (var dbContext2 = new ComplexDbContext())
            {
                dbContext2.Update(client2ChangeInSameTime);
                Assert.Throws<DbUpdateConcurrencyException>(() => dbContext2.SaveChanges(),
                    "Should throw DbUpdateConcurrencyException because the concurrency token of the mappedStudent differs from the originalStudent.");
            }
        }

        [Test]
        public void _14_2_UpdateWithOldConcurrencyToken_WithMap_ShouldThrowDbUpdateConcurrencyException()
        {
            Student originalStudent;
            using (var dbContext = new ComplexDbContext())
            {
                originalStudent = new Student()
                {
                    Age = 12,
                    Name = "Chuck Norris"
                };

                dbContext.Add(originalStudent);
                dbContext.SaveChanges();
            }

            var client1Change = new StudentDTO()
            {
                Id = originalStudent.Id,
                Name = "Changed Name",
                ConcurrencyToken = originalStudent.ConcurrencyToken,
                Age = 15
            };

            var client2ChangeInSameTime = new StudentDTO()
            {
                Id = originalStudent.Id,
                Name = "other change",
                ConcurrencyToken = originalStudent.ConcurrencyToken,
                Age = 82
            };

            using (var dbContext = new ComplexDbContext())
            {
                Student mappedStudent = dbContext.Map<Student>(client1Change);

                // Assert That Concurrency Token should not be overwritten by map
                Assert.That(client1Change.ConcurrencyToken, Is.EqualTo(originalStudent.ConcurrencyToken));

                dbContext.SaveChanges();
            }
            using (var dbContext = new ComplexDbContext())
            {
                Student mappedStudent = dbContext.Map<Student>(client2ChangeInSameTime);

                // Assert That Concurrency Token should not be overwritten by map
                Assert.That(client2ChangeInSameTime.ConcurrencyToken, Is.EqualTo(originalStudent.ConcurrencyToken));

                Assert.Throws<DbUpdateConcurrencyException>(() => dbContext.SaveChanges(),
                    "Should throw DbUpdateConcurrencyException because the concurrency token of the mappedStudent differs from the originalStudent.");

                // Following command is executed when dbContext.SaveChanges() is called:

                //Executed DbCommand (0ms) [Parameters=[@p3='1', @p0='15', @p1='12', @p4='1', @p2='Changed Name' (Size = 12)], CommandType='Text', CommandTimeout='30']
                //UPDATE "Students" SET "Age" = @p0, "ConcurrencyToken" = @p1, "Name" = @p2
                //WHERE "Id" = @p3 AND "ConcurrencyToken" = @p4;
                //SELECT changes();

                // The ConcurrencyToken (@p4) in the WHERE clause still is 1, although it should be 11
                // Note: @p1 gets computed in the overwritten SaveChanges method, with postgresql xmin is used, which gets computed automatically on the db

                // The default EFCore behavior is to throw a DbUpdateConcurrencyException when the concurrency token is different
                // See Test _14_1
            }
        }
        
        [Test]
        public void _15_1_AddEntityWithOwnedTypes_ShouldAlsoStoreOwnedTypeValues()
        {
            var newStudentDto = new StudentDTO()
            {
                Age = 16,
                Name = "Chuck Norris",
                // This class is marked as owned with the [Owned] attribute
                // StudentGrades also is a property of the corresponding Student class
                Grades = new StudentGrades()
                {
                    English = "A+",
                    ComputerScience = "C++",
                    Math = "A+"
                }
            };

            Student newStudentEntity;
            
            using (var dbContext = new ComplexDbContext())
            {
                newStudentEntity = dbContext.Map<Student>(newStudentDto);
                
                // Assert that the owned type is mapped correctly
                Assert.That(newStudentEntity.Grades, Is.Not.Null);
                Assert.That(newStudentEntity.Grades.English, Is.EqualTo("A+"));
                Assert.That(newStudentEntity.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(newStudentEntity.Grades.Math, Is.EqualTo("A+"));
                
                dbContext.SaveChanges();
            }

            using (var dbContext = new ComplexDbContext())
            {
                var studentFromDb = dbContext.Students.Single(s => s.Id == newStudentEntity.Id);
                
                // Assert that the owned type is stored in the db
                Assert.That(studentFromDb.Grades, Is.Not.Null);
                Assert.That(studentFromDb.Grades.English, Is.EqualTo("A+"));
                Assert.That(studentFromDb.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(studentFromDb.Grades.Math, Is.EqualTo("A+"));
            }
            
            // Following Insert statement is executed:
            // Executed DbCommand (0ms) [Parameters=[@p0='16', @p1='0', @p2='Chuck Norris' (Size = 12)], CommandType='Text', CommandTimeout='30']
            // INSERT INTO "Students" ("Age", "ConcurrencyToken", "Name")
            // VALUES (@p0, @p1, @p2);
            // SELECT "Id"
            // FROM "Students"
            // WHERE changes() = 1 AND "rowid" = last_insert_rowid();
            
            // As you can see, none of the OwnedType columns are present in the statement
            
            // Test _15_2 shows the EF Core default behavior which stores the owned type values in the db
            // Test _15_3 shows that the storing of owned types already works with Detached.Mappers when updating an entity
        }

        [Test]
        public void _15_2_AddEntityWithOwnedTypes_WithPureEFCore_ShouldAlsoStoreOwnedTypeValues()
        {
            var newStudent = new Student()
            {
                Age = 16,
                Name = "Chuck Norris",
                Grades = new StudentGrades()
                {
                    English = "A+",
                    ComputerScience = "C++",
                    Math = "A+"
                }
            };

            using (var dbContext = new ComplexDbContext())
            {
                dbContext.Add(newStudent);
                dbContext.SaveChanges();
                
                Assert.That(newStudent.Grades, Is.Not.Null);
                Assert.That(newStudent.Grades.English, Is.EqualTo("A+"));
                Assert.That(newStudent.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(newStudent.Grades.Math, Is.EqualTo("A+"));
            }

            using (var dbContext = new ComplexDbContext())
            {
                var newStudentFromDb = dbContext.Students.Single(s => s.Id == newStudent.Id);
                Assert.That(newStudentFromDb.Grades, Is.Not.Null);
                Assert.That(newStudentFromDb.Grades.English, Is.EqualTo("A+"));
                Assert.That(newStudentFromDb.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(newStudentFromDb.Grades.Math, Is.EqualTo("A+"));
            }
            
            // Following Insert statement is executed:
            // Executed DbCommand (0ms) [Parameters=[@p0='16', @p1='0', @p2='Chuck Norris' (Size = 12), @p3='C++' (Size = 3), @p4='A+' (Size = 2), @p5='A+' (Size = 2)], CommandType='Text', CommandTimeout='30']
            // INSERT INTO "Students" ("Age", "ConcurrencyToken", "Name", "Grades_ComputerScience", "Grades_English", "Grades_Math")
            // VALUES (@p0, @p1, @p2, @p3, @p4, @p5);
            // SELECT "Id"
            // FROM "Students"
            // WHERE changes() = 1 AND "rowid" = last_insert_rowid();
            
        }

        [Test]
        public void _15_3_UpdateEntityWithOwnedTypes_AlsoUpdatesOwnedTypeValues()
        {
            var newStudentDto = new StudentDTO()
            {
                Age = 16,
                Name = "Chuck Norris",
                Grades = new StudentGrades()
                {
                    English = "A+",
                    ComputerScience = "C++",
                    Math = "A+"
                }
            };

            Student newStudentEntity;
            
            using (var dbContext = new ComplexDbContext())
            {
                // Store student first
                newStudentEntity = dbContext.Map<Student>(newStudentDto);
                dbContext.SaveChanges();
            }
            
            newStudentDto.Id = newStudentEntity.Id;
            newStudentDto.ConcurrencyToken = newStudentEntity.ConcurrencyToken;

            using (var dbContext = new ComplexDbContext())
            {
                // Now update the student
                var updatedStudentEntity = dbContext.Map<Student>(newStudentDto);
                dbContext.SaveChanges();
                
                Assert.That(updatedStudentEntity.Grades, Is.Not.Null);
                Assert.That(updatedStudentEntity.Grades.English, Is.EqualTo("A+"));
                Assert.That(updatedStudentEntity.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(updatedStudentEntity.Grades.Math, Is.EqualTo("A+"));
            }

            using (var dbContext = new ComplexDbContext())
            {
                var updatedStudentFromDb = dbContext.Students.Single(s => s.Id == newStudentEntity.Id);
                Assert.That(updatedStudentFromDb.Grades, Is.Not.Null);
                Assert.That(updatedStudentFromDb.Grades.English, Is.EqualTo("A+"));
                Assert.That(updatedStudentFromDb.Grades.ComputerScience, Is.EqualTo("C++"));
                Assert.That(updatedStudentFromDb.Grades.Math, Is.EqualTo("A+"));
            }
            
            // Following Update statement is executed:
            // Executed DbCommand (0ms) [Parameters=[@p3='1', @p4='1', @p0='C++' (Size = 3), @p1='A+' (Size = 2), @p2='A+' (Size = 2)], CommandType='Text', CommandTimeout='30']
            // UPDATE "Students" SET "Grades_ComputerScience" = @p0, "Grades_English" = @p1, "Grades_Math" = @p2
            // WHERE "Id" = @p3 AND "ConcurrencyToken" = @p4;
            // SELECT changes();
        }


        [Test]

        public void _16_TryBaseListToLinkWithEntity_WithMap_ShouldNotThrow()
        {
            var dto = new EntityOneDTO()
            {
                BaseHeads = new()
                {
                    new EntityTwoDTO()
                    {
                        Discriminator = nameof(EntityTwo)
                    },
                    new EntityFourDTO()
                    {
                        Discriminator = nameof(EntityFour),
                        BaseStationOneSeconds = new()
                        {
                            new EntityThreeDTO()
                            {
                                Discriminator = nameof(EntityThree)
                            }
                        },
                        EntityThrees = new()
                        {
                            new EntityFiveDTO()
                            {
                                Discriminator = nameof(EntityFive)
                            }
                        }
                    }
                }
            };
            
            using (var dbContext = new ComplexDbContext())
            {
                var mappedEntityOne = dbContext.Map<EntityOne>(dto);
                dbContext.SaveChanges();
            }
        }
        
        [Test]
        public void _17_TryItemsAreMovedFromOneListToAnother_WithMap_ShouldNotThrow()
        {
            var dto = new AngebotDTO()
            {
                Positionen = new()
                {
                    new ArtikelPositionDTO()
                    {
                        Positionsart = nameof(ArtikelPosition),
                    }
                }
            };
            Angebot dbUpdated;
            using (var dbContext = new ComplexDbContext())
            { 
                var mappedEntityOne = dbContext.Map<Angebot>(dto);
                dbContext.SaveChanges();
                dbUpdated = dbContext.Angebote.Include(a => a.Positionen).ThenInclude(p => ((UeberschriftPosition)p).Positionen).First();
            }
            var dtoAfterSave = new AngebotDTO()
            {
                ConcurrencyToken = dbUpdated.ConcurrencyToken,
                Id = dbUpdated.Id,
                Positionen = new()
                {
                    new UeberschriftPositionDTO()
                    {
                        Positionsart = nameof(UeberschriftPosition),
                        Positionen = new()
                        {
                            new ArtikelPositionDTO()
                            {
                                ConcurrencyToken = dbUpdated.Positionen.First().ConcurrencyToken,
                                Positionsart = nameof(ArtikelPosition),
                                Id = dbUpdated.Positionen.First().Id
                            }
                        }
                    }
                }
            };
            
            using (var dbContext = new ComplexDbContext())
            {
                var mappedEntityOne = dbContext.Map<Angebot>(dtoAfterSave);
                dbContext.SaveChanges();
            }
        }  
        [Test]
        public void _18_TryItemsAreMovedFromOneListToAnother_WithMap_ShouldNotThrow()
        {
            var dto = new ArtikelDTO()
            {
                OwnedOne = new OwnedOneDTO()
                {
                    DateTime = DateTime.Now,
                },
                OwnedTwo = new OwnedTwoDTO()
                {
                    Bool = true
                }
            };
            
            Artikel dbUpdated;
            using (var dbContext = new ComplexDbContext())
            { 
                var mappedEntityOne = dbContext.Map<Artikel>(dto);
                dbContext.SaveChanges();
                dbUpdated = dbContext.Artikel.First();
            }

            using (var dbContext = new ComplexDbContext())
            {
                dbUpdated = dbContext.Artikel.First();
            }
        }    
    }
}