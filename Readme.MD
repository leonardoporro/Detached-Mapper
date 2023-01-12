![Detached Banner](banner.png?raw=true)

Map your DTOs directly to the DbContext

##### Play with Mapper in .Net Fiddle!
- Attach graph of anonymous objects: [link](https://dotnetfiddle.net/l7q6wA) 
- Import json graph: [link](https://dotnetfiddle.net/Mn8mYU)

##### Check related tools
- [Runtime Types](https://github.com/leonardoporro/Detached-RuntimeTypes)
- [Patch Types](https://github.com/leonardoporro/Detached-PatchTypes)

##### Let me know if it works for you (or doesn't)!
- [LinkedIn](https://www.linkedin.com/in/leonardoporro/)
- [E-mail](mailto:mail@leonardoporro.com.ar)
(Or hire me! ;) )

# Mappers
#### What is it

Hi!, I'm trying to build a fast, object-oriented mapper to simplify the task of saving disconected entities in web services.
It was heavily inspired by [GraphDiff](https://github.com/zzzprojects/GraphDiff), [AutoMapper](https://github.com/AutoMapper/AutoMapper) and other awesome libraries.
Any fix or feedback is very welcome.

#### What does it solve
If you worked with .net apis long enought you have seen one or all of these situations:

###### Problem 1: Disconected entities state

When working with EntityFramework, NHibernate, or practically any other ORM that tracks entity states, 
there is a problem when persinsing non-tracked entities that comes usually from a deserialization (request, response, a file, another db, etc.)
EF has the TrackGraph method, but the state of each entity needs to be specified manually.
NHibernate has a Merge feature, but it merges everything, and sometimes there are associated entities that should not be modified.

###### Problem 2: Mapping Data transfer objects (DTOs) to Entities
To reduce data traffic and, improve the security and the readability of swagger models, many times DTOs are used, and often they are very similar
to the original entity. 

A manual mapping or a tool like AutoMapper is needed to convert the
DTOs to entities, before attach them to the ORM context/session, but this library aims to do it automatically and save steps.

###### Problem 3: Patches (partial updates)
C# does not support an "Undefined" value, like JavaScript, that's why for partial 
updates, Null may be used for optional values. But in some cases, null needs to be persisted, to remove 
a previous field value or disconnect an existing relationship.

### How it works
The main method of Detached is MapAsync. It receives an object or dictionary as a parameter,
then loads the corresponding entity graph from the database (like GraphDiff does),
and then copies the values from the given DTO and set the correct states.

###### Solution 1: Mapping DTOs to Entities
Detached can handle DTO-Entity, DTO-DTO and Entity-Entity mapping.
There are important tasks when mapping entities, like checking back references
and merging collections. So that, classes representing entities should be marked
with the [Entity] attribute or configured fluently within the `DbContext` registration method
by calling: 
```csharp
detachedMapperOptions.Type<Entity>().IsEntity();
```
for each entity you would like
to configure mapping for.

For EntityFramework, entities are marked automatically when the correspondng DbSet exists.
More info on configuration, later in this doc.

###### Solution 2: Setting states - Compositions and Aggregations
Aggregations are the weak relations (B and A are independent), also known as "Has a".
Compositions are the strong (B cannot exists without A), also known as "Owns a".

This can be configured using the [Aggregation] and [Composition] attributes on the corresponding properties
or fluently using 

```csharp
 services.AddDbContext<MyDbContext>(
                    options =>
                    {
                        options.UseDetached(detachedMapperOptions => {                      
                             
                             // For the 'Entity' type (in the example):
                            detachedMapperOptions.Type<Entity>().Member(i => i.Member).Composition();
                            
                            // Example: for the 'Author' type:
                             detachedMapperOptions.Type<Author>().Member(i => i.AuthorTitle).Composition();
                             
                             // Example: for the 'Book' type:
                              detachedMapperOptions.Type<Book>().Member(i => i.Barcode).Composition();
                            
                              // etc...
                            
                        });
                        
                        // Then continue your typical DbContext configuration  below...
```


More info on configuration, later in this doc.

When mapping, detached **will only modify** the root entity (the one passed as a parameter) and
all the related entities marked as compositions. Associations are just attached an marked as Unmodified. 
Assuming that associations exist, also helps reducing the ammount of data loaded for the comparison.

**[Aggregation]** -> Link to an existing entity, is marked as Unmodified.
**[Composition]** -> Create or update the entity, is marked as Created/Updated/Deleted.

**(Note: there is an internal parameter: mapParams.AggregationAction, that allows creating associated entities when they don't exist,
this is used to import complex graphs from json without having to order by dependency)**.

###### Solution 3: DTOs and IPatch
Detached copies only the properties that match (or are configured) from the DTO to the Entity,
other properties are not overwritten.
For partial updates, a DTO with the needed properties may be created, or a Dictionary&lt;string, object&gt;
can be passed as a parameter. It also maps anonymous objects.

Partial updates without manually creating new types are supported using the IPatch interface,
the PatchProxyFactory and the PatchJsonConverter.
IPatchs indicates when properties are dirty, and PatchJsonConverter can generate proxies of the 
classes that auto-implement IPatch and deserialize json using that proxy.
More info on configuration, later in this doc.

#### Getting Started

###### Install Nu-Get package
```powershell
> Install-Package Detached.Mappers.EntityFramework -Version 6.0.2
```
###### Create your regular DbContext
```cs
public class TestDbContext : DbContext
{
    ...
}
```
###### Register dependencies
Detached needs a QueryProvider instance, that helps loading the current status of the entities to save
and a Mapper instance that copies the given DTO/Entity state over the current entities.
These services are added by calling UseDetached on DbContextBuilderOptions.

If working on ASP.NET, this is usually configured in the DI container at the beggining (Startup.cs).

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<MainDbContext>(cfg =>
    {
        ...
        cfg.UseDetached();
    });
} 
```

###### Perform a Map
The MapAsync extension method can be called to load current state and copy DTO values.
MapAsync returns the persisted entity (that comes from the DB) with the updated values. No updates are persisted
until SaveChanges is called().

```cs
User attachedUser = await dbContext.MapAsync<User>(new UserDTO { Id = 1, Name = "NewName" });
await dbContext.SaveChangesAsync();
```

###### Configuration
Associations must be configured as aggregations or compositions, so Detached can choose how to load and map them.
In order to do this, use [Aggregation] or [Composition] attributes:

```cs
class Invoice
{
    [Composition]
    public List<Row> Rows { get; set; }
}
```

Or use MappingOptions when calling UseDetached to configure it:

```csharp
services.AddDbContext<MainDbContext>(cfg =>
    {
        ...
        cfg.UseDetached(options => {
            options.Type<Invoice>().Member(i => i.Rows).Composition();
        });
    });

```
###### Solution 3: Different approaches for Partial Updates
Approaches to perform partial updates:

1 - Use a DTO or a Dictionary or an Anonymous class that contains only the properties to update
Mapper will copy only matching properties, as the other ones do not exist.

2 - Programatically implement IPatch interface, that allows to mark properties as dirty.
Mapper will check the value of IPatch.IsSet before copying the property value.

3 - The Patch Type Proxy factory (Patch.Create) can be used to generate a new runtime type that inherits your class and implements IPatch automatically.
This is done by overriding all property setters and adding an internal HashSet<string> to keep the values, so that, 
**All properties must be marked as Virtual; otherwise, the proxy factory wont't be able to override them**.
 
When sending a JSON with undefined properties, serializer leave them with their default values and the "undefined" state is lost during deserialization.
That's why PatchJsonConverter allows to automatically deserialize some classes into Patch Proxy types directly, tracking the "undefined" information in the 
form of IsSet() true/false calls.

See this library for source code and more info and samples: [Patch Types](https://github.com/leonardoporro/Detached-PatchTypes)
