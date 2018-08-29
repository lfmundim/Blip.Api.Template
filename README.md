# BLiP.API.Template ![release_badge](https://img.shields.io/badge/release-v1.0.5-brightgreen.svg)

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

## File Structure
```cs
    📁src
    |__📁Blip.Api.Template // The Web API project Folder
    |   |__📁Controllers
    |   |   |__📃{YourControllers.cs}
    |   |   |__📃{...}
    |   |__📁Models
    |   |   |__📃{YourModelClasses.cs}
    |   |   |__📃{...}
    |   |__📁Services
    |   |   |__📃{YourServicesAndInterfaces.cs}
    |   |__📃MySettings.cs
    |   |__📃Startup.cs
    |   |__📃appsettings.json
    |__📁Blip.Api.Template.Services // (If needed) The project to use for 3rd party APIs to be consumed
        |__{Recommended to follow similar structure from the above project}
```

## Uninstallation
To uninstall the template from your local machine, use the command
```sh
dotnet new -u Blip.Api.Template
```
Your current projects that already use the template **will not** be affected.

## Feature Roadmap
* Change `CustomSender` methods to `HTTP`
* Basic controller for external link tracking