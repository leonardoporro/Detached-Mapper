# Detached
Detached is a set of tools to make the process of building services or REST APIs faster.
It started with EntityFramework and was inspired by [GraphDiff](https://github.com/refactorthis/GraphDiff).
Each tool has its own nuget and instructions, so please check individual read me as needed.

If you'd like to participate or need some support, please drop me an email: mail@leonardoporro.com.ar
or [fork me on github](https://github.com/leonardoporro/Detached/fork).
Thanks in advance for your help!

WARNING: This project is not guaranteed to work for your particular needs. 

# Detached.EntityFramework
Allows loading and saving entire entity graphs (the entity with its children/relations) at once and without extra code.

[Read me](./README-ENTITYFRAMEWORK.md)

# Detached.Services
Provides generic repositories based on Detached.EntityFramework.

[Read me](./README-SERVICES.md)

# Detached.Mvc
Provides generic controllers and validations based on Detached.Services. 

[Read me](./README-MVC.md)

# Detached.Mvc.Localization
Provides automatic localization by mapping full names and namespaces of Clr Types to specified keys and resource files.
It also features a JsonStringLocalizer.
This project does not have dependencies on Detached.EntityFramework or Detached.Services so it can be used separately.

[Read me](./README-MVC-LOCALIZATION.md)

# Demos
Currently building Angular2 and MVC demos.

# Build
To build the project, you need:
 - Microsoft Visual Studio 2015 Update 3
 - .NET Core 1.0.1 Tools Preview 2
https://www.microsoft.com/net/core#windowsvs2015

Unit tests depend on Moq beta. Please, in VS2015, go to Tools->Options, look for Nuget Package manager 
and add this source:
https://www.myget.org/F/aspnet-contrib/api/v3/index.json
