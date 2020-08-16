# Detached 5
#### What is it
Hi!, I'm trying to build a fast, object-oriented mapper to simplify the task of saving disconected entities in web services.
It was heavily inspired by [GraphDiff](https://github.com/zzzprojects/GraphDiff), [AutoMapper](https://github.com/AutoMapper/AutoMapper) and other awesome libraries.
Any fix or feedback is very welcome.
#### What does it solve
###### Disconected entities
When working with EntityFramework, NHibernate, or practically any other ORM that tracks entity states, there is a problem when persinsing non-tracked entities that comes usually from a deserialization (request, response, a file, another db, etc.)
EF has the TrackGraph method, but the state of each entity needs to be specified manually.
NHibernate has a Merge feature, but it merges everything, and sometimes there are associated entities that should not be modified.
###### Data transfer objects (DTOs)
To reduce data traffic and, improve the security and the readability of swagger models, many times DTOs are used, and often they are very similar
to the original entity. A manual mapping or a tool like AutoMapper is needed to convert the
DTOs to an entity, before attach them to the ORM context/session.
###### Partial updates
C# does not support an "Undefined" value, like JavaScript, that's why for partial 
updates, Null may be used for optional values. But in some cases, null needs to be persisted, to remove 
a previous field value or disconnect an existing relationship.
#### How it works
The main method of Detached is MapAsync. It receives an object or dictionary as a parameter,
then loads the corresponding entity from the database (like GraphDiff and Merge),
copies the values from the given DTO and set the correct states.
###### Entities and DTOs
Detached can handle DTO-Entity, DTO-DTO and Entity-Entity mapping.
There are important tasks when mapping entities, like checking back references
and merging collections. So that, classes representing entities should be marked
with the [Entity] attribute or configured with modelOptions.Configure&lt;Entity&gt;().IsEntity().
More info on configuration, later in this doc.
###### Compositions and Aggregations
Aggregations are the weak relations (B and A are independent), also known as "Has a".
Compositions are the strong (B cannot exists without A), also known as "Owns a".
This can be configured using the [Aggregation] and [Composition] attributes on the corresponding properties
or fluently using modelOptions.Configure&lt;Entity&gt;().Member(e => e.Member).Composition().
More info on configuration, later in this doc.
When mapping, detached **will only modify** the root entity (the one passed as a parameter) and
all the related entities marked as compositions. Associations are just attached an marked as Unmodified.
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

TO-DO!

###### Install Nu-Get package

###### Inherit from DetachedDbContext

###### Register dependencies

###### Add the DbContext as a regular one

###### Define model and DTO

###### Perform a Map
