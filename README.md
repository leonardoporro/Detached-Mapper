
# EntityFrameworkCore.Detached

Loads and saves entire detached entity graphs (the entity with their child entities and lists). 
Inspired by [GraphDiff](https://github.com/refactorthis/GraphDiff).

WARNING: This is a development project, not guaranteed to work for your particular purposes.
If you'd like to participate or need some support, please drop me an email: mail@leonardoporro.com.ar
or join https://github.com/leonardoporro/EntityFrameworkCore.Detached.
Thanks in advance for your help!

Features:
- 1.0.0-alpha3
 * [Owned] and [Associated] attributes to define scope when loading a graph.
 * Roots<TEntity>(): returns an IQueryable with the proper includes (joins).
 * LoadAsync<TEntity>(object[] key): loads a single detached root by its key.
 * SaveAsync: persists a single detached root to the database. 

Usage looks like this:
```csharp
// Create a detached wrapper for your context.
DetachedContext context = new DetachedContext(new YourDbContext());

// Get some detached entity root. e.g.: Company has an [Owned] list of Employees.
Company company = new Company {
	Employees = new[] {
    	new Employee { Id = 1, Name = "John Snow" },
        new Employee { Id = 0, Name = "Daenerys Targaryen" } //new!
    }
}

// Save the detached entity to the database.
await context.SaveAsync(company);              
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

# Build

In order to get alpha EF Core dependencies, you would need to add myget.org.
Go to menu Tools -> Options -> NuGet Package Manager -> Package Sources and add this source:
https://dotnet.myget.org/F/aspnetcore-dev/
Then you will be able to download EF Core 1.1.0-alpha...

# Nuget package:
https://www.nuget.org/packages/EntityFrameworkCore.Detached/