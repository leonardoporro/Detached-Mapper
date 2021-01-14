![Detached Banner](banner.png?raw=true)
##### Play with Detached in .Net Fiddle!
- Attach graph of anonymous objects: [link](https://dotnetfiddle.net/l7q6wA) 
- Import json graph: [link](https://dotnetfiddle.net/Mn8mYU)

# Detached Mapper
#### What is it

Hi!, I'm trying to build a fast, object-oriented mapper to simplify the task of saving disconected entities in web services.
It was heavily inspired by [GraphDiff](https://github.com/zzzprojects/GraphDiff), [AutoMapper](https://github.com/AutoMapper/AutoMapper) and other awesome libraries.
Any fix or feedback is very welcome.

## What does it solve
###### Disconected entities

When working with EntityFramework, NHibernate, or practically any other ORM that tracks entity states, 
there is a problem when persinsing non-tracked entities that comes usually from a deserialization (request, response, a file, another db, etc.)
EF has the TrackGraph method, but the state of each entity needs to be specified manually.
NHibernate has a Merge feature, but it merges everything, and sometimes there are associated entities that should not be modified.

###### Data transfer objects (DTOs)
To reduce data traffic and, improve the security and the readability of swagger models, many times DTOs are used, and often they are very similar
to the original entity. 

A manual mapping or a tool like AutoMapper is needed to convert the
DTOs to entities, before attach them to the ORM context/session.

###### Partial updates
C# does not support an "Undefined" value, like JavaScript, that's why for partial 
updates, Null may be used for optional values. But in some cases, null needs to be persisted, to remove 
a previous field value or disconnect an existing relationship.

### How it works
The main method of Detached is MapAsync. It receives an object or dictionary as a parameter,
then loads the corresponding entity graph from the database (like GraphDiff does),
and then copies the values from the given DTO and set the correct states.

###### Entities and DTOs
Detached can handle DTO-Entity, DTO-DTO and Entity-Entity mapping.
There are important tasks when mapping entities, like checking back references
and merging collections. So that, classes representing entities should be marked
with the [Entity] attribute or configured with ```modelOptions.Configure<Entity>().IsEntity()```.
For EntityFramework, entities are marked automatically when the correspondng DbSet exists.
More info on configuration, later in this doc.

###### Compositions and Aggregations
Aggregations are the weak relations (B and A are independent), also known as "Has a".
Compositions are the strong (B cannot exists without A), also known as "Owns a".

This can be configured using the [Aggregation] and [Composition] attributes on the corresponding properties
or fluently using 
```cs modelOptions.Configure<Entity>().Member(e => e.Member).Composition()```.
More info on configuration, later in this doc.

When mapping, detached **will only modify** the root entity (the one passed as a parameter) and
all the related entities marked as compositions. Associations are just attached an marked as Unmodified.
(Note: there is an internal parameter: MapperParameters.AggregationAction, that allows creating associated entities when they don't exist,
this is used to import complex graphs from json without having to order by dependency).

###### Partial updates
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
```cs
Install-Package Detached.Mappers.EntityFramework -Version 5.0.12
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

```cs
services.AddDbContext<MainDbContext>(cfg =>
    {
        ...
        cfg.UseDetached(options => {
            options.Configure<Invoice>().Member(i => i.Rows).Composition();
        });
    });

```
###### Partial Updates

- Use a DTO or a Dictionary or an Anonymous class that contains only the field to update
Mapper will copy only matching properties.

- Implement IPatch interface, that allows to mark properties as dirty.
Mapper will check IPatch.IsSet before copying the property value.
The Patch Type Proxy generator (Patch.Create) can be used to generate a type inheriting from the given type that automatically implements IPatch (NOTE: properties must be marked as virtual).
PatchJsonConverter deserializes json into a graph of patch proxies. This is used by Import feature to write only the fields that exist in the json data.










