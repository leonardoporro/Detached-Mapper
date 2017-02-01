
# Detached.EntityFramework

Loads and saves entire detached entity graphs (the entity with their child entities and lists). 
Inspired by [GraphDiff](https://github.com/refactorthis/GraphDiff).
Also provides some plugins to simplificate repetitive tasks, like auditing and pagination.

# Features
* Loading entity graphs.
* Updating entity graphs.
* Plugins:
	- Audit: Sets CreatedBy, CreatedDate, ModifiedBy, ModifiedDate.
	- ManyToMany Patch: Copies an intermediate table back and forth to a given collection
	simulating a skip-level navigation. Will be removed when EF Core supports many to many relations.
	- PartialKey: Allows adding [PKey(n)] attribute to define composite keys.
	- Pagination: Loads pages of data in an homogeneus way.
    - [Here your own plugin!].
	 
# Basic Usage
```csharp
// Create a detached wrapper for your context.
IDetachedContext<YourDbContext> context = new DetachedContext<YourDbContext>(new YourDbContext());

// Get some detached entity root. e.g.: Company has an [Owned] list of Employees.
Company company = new Company {
	Employees = new[] {
    	new Employee { Id = 1, Name = "John Snow" },
        new Employee { Id = 0, Name = "Daenerys Targaryen" } //new!
    }
}

// Save the detached entity to the database.
await context.UpdateAsync(company);
await context.SaveChangesAsync();  
```
You can also get detached root entities:

```csharp
DetachedContext context = new DetachedContext(new YourDbContext());

// This loads the company with Id: 1 along with the employees (and any other relation).
Company company = await context.LoadAsync<Company>(1); 

```

# Configuration:
To be able to process the [Owned] and [Associated] attributes, a custom convention builder is needed.
You can replace the ICoreConventionSetBuilder service by overriding OnConfiguring in your DbContext:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseDetached().UseSqlServer(...);
}
```
Or if you are using your own service collection: 

```csharp

            var serviceProvider = new ServiceCollection()
                 .AddEntityFrameworkInMemoryDatabase()
                 .AddEntityFrameworkDetached()
                 .BuildServiceProvider();
```

# Nuget package:
https://www.nuget.org/packages/EntityFrameworkCore.Detached/