
# Detached.EntityFramework

Loads and saves entire detached entity graphs (the root entity object with its children). 
Inspired by [GraphDiff](https://github.com/refactorthis/GraphDiff).
It also provides some plugins to simplificate repetitive tasks, like auditing and pagination.

## Features
* Loading entity graphs.
* Updating entity graphs.
* Plugins:
	- Audit: Sets CreatedBy, CreatedDate, ModifiedBy, ModifiedDate.
	- ManyToMany Patch: Copies an intermediate table back and forth to a given collection
	simulating a skip-level navigation. Will be removed when EF Core supports many to many relations.
	- PartialKey: Allows adding [PKey(n)] attribute to define composite keys.
	- Pagination: Loads pages of data in an homogeneus way.
    - [Here your own plugin!].

## Usage
You need to configure the DbContext and DetachedContext, add [Owned] and [Associated] attributes to your model and persist using the 
the DetachedContext methods.

#### Configuring the DbContext
When configuring your DdContext, specify UseDetached(). Optionally, add plugins and/or change configuration using the
lambda argument e.g.: .UseDetached(conf => conf.UseAudit()).

```csharp
    services.AddDbContext<YourDbContext>(conf =>
        conf.UseSqlServer("YourConnectionString")
            .UseDetached(dconf => dconf.UseManyToManyHelper()));
```

An in-memory instance is probably better for unit testing:
```csharp
    var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .AddEntityFrameworkDetached()
            .BuildServiceProvider();
```

Optionally, you can register a generic detached context to inject it later into the services.

```csharp
services.AddTransient(typeof(IDetachedContext<>), typeof(DetachedContext<>));
```

#### Configuring the Model
Add [Owned] attributes to the navigation properties where the related objects will be modified along with the root entity (composition).
Add [Associated] attributes to the navigation properties where the related objects are self-contained independent entities (association).

```csharp
public class Invoice {
	[Associated]
	public InvoiceType Type { get; set; }

	[Associated]
	public Customer Customer { get; set; }

	[Owned]
	public IList<InvoiceRow> Rows { get; set; }

	public decimal Total { get; set; }
}

public class InvoiceRow {
	public int Id { get; set; }
	public int Count { get; set; }
	public string Description { get; set; }
	public decimal UnitPrice { get; set; }
}

public class InvoiceType {
	public int Id { get; set; }
	public int Description { get; set; }
}

public class Customer { ... }
```

##### Persisting using DetachedContext
Once configured, inject the DetachedContext into your service and use it to load and save entity graphs.

```csharp
public class InvoiceService {
	IDetachedContext<YourDbContext> _detachedContext;
	// inject the detached context. 
	public YourService(IDetachedContext<YourDbContext> detachedContext)
	{
		_detachedContext = detachedContext; 
	}

	public virtual async Task<Invoice> Save(Invoice entity)
    {
		// update the entity graph: it will add/remove children and modify fields as needed.
        entity = await _detachedContext.Set<Invoice>().UpdateAsync(T);

		// once updated, we need to save the changes.
        await _detachedContext.SaveChangesAsync();

		// return the saved entity (it will contain, generated fields and ids).
        return entity;
    }
}
```

## Plugins

In progress ... 


# Nuget package:

Detached.EntityFramework
https://www.nuget.org/packages/Detached.EntityFramework

(Deprecated) https://www.nuget.org/packages/EntityFrameworkCore.Detached/
Please move to Detached.EntityFramework and sorry for the inconveniences.