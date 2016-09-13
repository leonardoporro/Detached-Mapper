
# EntityFrameworkCore.Detached

Loads and saves entire detached entity graphs (the entity with their child entities and lists). Inspired by  [GraphDiff](https://github.com/refactorthis/GraphDiff).


Features:

 * [Owned] and [Associated] attributes to define scope when loading a graph.
 * Load, Save and Delete basic methods for working with detached entities.


Usage looks like this:
```csharp
DetachedContext context = new DetachedContext(new YourDbContext());

Company company = new Company {
	Employees = new[] {
    	new Employee { Id = 1, Name = "John Snow" },
        new Employee { Id = 0, Name = "Daenerys Targaryen" } //new!
    }
}
// (this is a detached company)
await context.SaveAsync(company);              
```

# Configuration:
To be able to process the [Owned] and [Associated] attributes, a convention builder is needed:


```csharp
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<ICoreConventionSetBuilder, 	DetachedCoreConventionSetBuilder>();
        }
```

WARNING: THIS PROJECT MAKES USE OF SOME INTERNAL ENTITY FRAMEWORK FEATURES AND IT CAN BE BROKEN BY FUTURE CHANGES.
