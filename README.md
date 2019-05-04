# BLiP.API.Template [![Build Status](https://travis-ci.org/lfmundim/Blip.Api.Template.svg?branch=master)](https://travis-ci.org/lfmundim/Blip.Api.Template) [![Nuget](https://buildstats.info/nuget/Blip.Api.Template)](https://www.nuget.org/packages/blip.api.template) [![GitHub stars](https://img.shields.io/github/stars/lfmundim/Blip.Api.Template.svg)](https://github.com/lfmundim/Blip.Api.Template/stargazers)


## This template aims to kickstart the development of an API to be used alongside BLiP's Builder feature

## Installation
If you already have `dotnet` installed, you can install this template with the command
```sh
dotnet new -i Blip.Api.Template
```

If you don't have `dotnet` installed, follow [these](https://www.microsoft.com/net/learn/get-started-with-dotnet-tutorial) instructions from _Microsoft_ to install it on GNU/Linux, Mac or Windows, and then use the command above.

## Usage
To create a new project using this template, after installing, type in the following command
```sh
dotnet new blip-api
```
Your new project should be created in the open folder.

### Renaming the `Services` project
By default, when you create a new solution using the template, all `.csproj` files will use the name of the folder you're in (along with any mentions of `Blip.Api.Template`. You can give the `Services` project a different name should you like it using the CLI param `--Services`. 

For example, if you run the command

```bash
dotnet new blip-api --Services DifferentName.Services
```

inside a folder named `Blip.Bot.Project`, it will create a `.sln` with two projects: `Blip.Bot.Project.csproj` and `DifferentName.Services.csproj`, instead of a `Blip.Bot.Project.Services.csproj`.

## File Structure
```cs
 📁Blip.Api.Template // assuming your folder is called Blip.Api.Template. Whatever name you choose will replace all occurrences of that string in the sln
 |__📁src
    |__📁Blip.Api.Template // The Web API project Folder
    |   |__📁Controllers
    |   |   |__📃{YourControllers.cs}
    |   |   |__📃{...}
    |   |__📁Models
    |   |   |__📃{YourModelClasses.cs}
    |   |   |__📃{...}
    |   |__📁Services
    |   |   |__📃{YourServices.cs}
    |   |__📁Interfaces
    |   |   |__📃{YourInterfaces.cs}
    |   |__📁Middleware
    |   |   |__📃{YourMiddlewareFiles.cs}
    |   |__📃MySettings.cs
    |   |__📃Startup.cs
    |   |__📃appsettings.json
    |__📁Blip.Api.Template.Services // (If needed) The project to use for 3rd party APIs to be consumed
    |   |__{Recommended to follow similar structure from the above project}
    |__📁Blip.Api.Template.Tests // It is strongly suggested that you try to cover most parts of your code
    |   |__{YourTestsFiles.cs}
    |__⚙️.editorconfig
```

## Uninstallation
To uninstall the template from your local machine, use the command
```sh
dotnet new -u Blip.Api.Template
```
Your current projects that already use the template **will not** be affected.

## Recommended Packages
This template comes bundled with a few highly recommended packages, such as [Blip.HttpClient](https://github.com/lfmundim/Blip.HttpClient), [RestEase](https://github.com/canton7/RestEase), [Serilog](https://github.com/serilog/serilog) and [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore). Those packages' repositories do a great job at explaining what they do, and it is strongly recommended that you familiarize yourself with them.

Furthermore, I will list some other interesting packages:
* For testing:
    * [Shouldly](https://github.com/shouldly/shouldly): *Assert* library that makes it easy to read and understand tests assertions. For example:
    ```cs
    bex.Reason.Description.ShouldBe("The requested resource was not found");
    getResource.ShouldNotBeNull();
    exceptionThrown.ShouldBeTrue();
    ```
    * [XUnit](https://github.com/xunit/xunit): Widely used Testing Framework for .NET
    * [NSubstitute](https://github.com/nsubstitute/NSubstitute): Library that allows the mocking of 3rd party (or 1st party) services that act like *black boxes*, meaning that you can't access their source code and therefore it is not up to you (or not interesting for you) to test it, but you still need to verify if the required methods are *at least* being called. Note that, as it is a substitute, the actual method *will not* be called, meaning that it *will not* access any external sources. For example:
    ```cs
    Serilog.ILogger _logger = Substitute.For<ILogger>();
    // <...>
    // Substitute call that will not, in fact, log anything anywhere
    _logger.Received(1).Information(Arg.Any<string>(), Arg.Any<string>()); 
    // Checks if the Serilog.ILogger.Information method was called EXACTLY one time with two params, where both of them are strings
    ```
* For logging:
    * [RestingLogger](https://github.com/lfmundim/RestingLogger): easy-to-use RestEase-powered library that logs every request sent and response received from external APIs. Optimized and tested using Serilog while logging on SEQ. **Note: might not work on other architectures.**

## Feature Roadmap
* Basic controller for external link tracking
