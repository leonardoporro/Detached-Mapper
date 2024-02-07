using Detached.Mappers.EntityFramework;
using Detached.Mappers.Tests.Contrib.ScottSoftware.Model;

namespace Detached.Mappers.Tests.Contrib.ScottSoftware;

public class TestUpdateScenario
{
    [Fact]
    public void map_update_composition()
    {
        var correctedTitle = @"The Dragon's Path";
        var correctedName = @"Daniel Abraham";
        // create the sqlite db.
        new TellDbContext().Database.EnsureCreated();

        using (var context = new TellDbContext())
        {
            context.Map<Creator>(new Creator
            {
                FullName = "Daniel Abraham.",
                PrimaryLanguage = "EN",
                Works = new() {
                    new Work {
                        Title = @"The Dragons Path",
                        Language = "EN"
                    }
                }
            });
            context.SaveChanges();
        }

        using (var context = new TellDbContext())
        {
            Assert.Equal(2, context.Creators.Count());
            Assert.Equal(2, context.Works.Count());
            Assert.Equal(2, context.Creators.OrderBy(c => c.Id).Last().Id);
        }

        using (var context = new TellDbContext())
        {
            var updatedCreator = context.Map<Creator>(new Creator
            {
                Id = 2,
                FullName = "Daniel Abraham",
                Works = new()
                {
                    new Work {
                        Id = 2,
                        Title = correctedTitle,
                        Language = "EN"
                    },
                    new Work {
                        Title = @"The King's Blood",
                        Language = "EN"
                    }
                }
            });

            context.SaveChanges();
        }

        using (var context = new TellDbContext())
        {
            Assert.Equal(2, context.Creators.Count());
            Assert.Equal(correctedName, context.Creators.OrderBy(c => c.Id).Last().FullName);
            Assert.Equal(3, context.Works.Count()); // 1 from seed, + 1 from creation, + 1 from updating
            Assert.Equal(correctedTitle, context.Works.Where(w => w.Id == 2).Single().Title);
        }
    }
}