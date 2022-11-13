# .NET Fundamentals

.NET is a free, cross-platform, open source developer platform for building many different types of applications.

With .NET, you can use multiple languages, editors, and libraries to build for web, mobile, desktop, games, and IoT.

## What is .NET?

Watch intro video

| Entelect | Public |
| --- | --- |
| [![.NET Forum FUndamentals?](https://i.ibb.co/JsCTrYN/net-forum-fundamentals.jpg)](https://web.microsoftstream.com/video/0a53829b-6cfc-48a6-909c-226255f1344b) | [![What is .NET?](https://i.ytimg.com/vi_webp/eIHKZfgddLM/hqdefault.webp)](https://youtu.be/eIHKZfgddLM?list=PLdo4fOcmZ0oWoazjhXQzBKMrFuArxpW80) |

### Languages

You can write .NET apps in C#, F#, or Visual Basic.

C# is a simple, modern, object-oriented, and type-safe programming language.
F# is a cross-platform, open-source, functional programming language for .NET. It also includes object-oriented and imperative programming.
Visual Basic is an approachable language with a simple syntax for building type-safe, object-oriented apps.

### Cross Platform

Whether you're working in C#, F#, or Visual Basic, your code will run natively on any compatible OS. Different .NET implementations handle the heavy lifting for you:

.NET 7 is a cross-platform .NET implementation for websites, servers, and console apps on Windows, Linux, and macOS.
.NET Framework supports websites, services, desktop apps, and more on Windows.
Xamarin/Mono is a .NET implementation for running apps on all the major mobile operating systems.

### One consistent API

.NET Standard is a base set of APIs that are common to all .NET implementations.

Each implementation can also expose additional APIs that are specific to the operating systems it runs on. For example, .NET Framework is a Windows-only .NET implementation that includes APIs for accessing the Windows Registry.

![.NET landscape](https://weblog.west-wind.com/images/2016/ASP.NET%20Core%20Overview/AspNetCoreToday.png)

# History

.NET is a general-purpose development platform. It can be used for any kind of app type or workload where general purpose solutions are used. It has several key features that are attractive to many developers, as it combines high performance and richness with unique, differentiated developer productivity.

​.NET Framework is a very powerful and mature framework, with a huge class library that supports a wide variety of applications and solutions, only supported on Window. The last version that will be shipped by Microsoft will be .NET 4.8.

.NET 7 is a new version of .NET which is a cross-platform, open-source and modular. It can be used for creating modern web apps, microservices, libraries and console applications that run everywhere (Windows, Linux and macOS). The last version of .NET 7 that will be shipped will be .NET 7 3.1.

Unlike the traditional .NET Framework, which is a single package installation, system-wide, and Windows-only runtime environment, .NET 7 is about decoupling .NET from Windows, allowing it to run in non-Windows environments without having to install a giant 400mb set of binaries (versus just the footprint of the components you need from .NET 7) plus the ability to deploy the applications coming with the framework itself supporting side-by-side execution of different versions of the framework.
With the Xamarin Platform, you can deliver native Android, iOS, macOS and Windows apps, using existing .NET skills, teams, and code.

The Xamarin Platform is based on Mono. Mono is the original open-source and cross-platform implementation of .NET, from the community Mono Project. It is now sponsored by Xamarin/Microsoft. Traditionally, Mono’s APIs have followed the progress of the .NET Framework, not .NET 7.

Open source is also an important part of the .NET ecosystem, with multiple .NET implementations and many libraries available under OSI-approved licenses.

## Future

There will be just one .NET going forward, and you will be able to use it to target Windows, Linux, macOS, iOS, Android, tvOS, watchOS and WebAssembly and more.

We will introduce new .NET APIs, runtime capabilities and language features as part of .NET 7.

![.NET a unified platform](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2022/11/dotnet-platform2.png)

![.NET schedule](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2022/11/NET-Release-Schedule.png)

# Runtime experiences

Mono is the original cross-platform implementation of .NET. It started out as an open-source alternative to .NET Framework and transitioned to targeting mobile devices as iOS and Android devices became popular. Mono is the runtime used as part of Xamarin.

CoreCLR is the runtime used as part of .NET 7. It has been primarily targeted at supporting cloud applications, including the largest services at Microsoft, and now is also being used for Windows desktop, IoT and machine learning applications.

Taken together, the .NET 7 and Mono runtimes have a lot of similarities (they are both .NET runtimes after all) but also valuable unique capabilities. It makes sense to make it possible to pick the runtime experience you want. We’re in the process of making CoreCLR and Mono drop-in replacements for one another. We will make it as simple as a build switch to choose between the different runtime options.

The following sections describe the primary pivots we are planning for .NET 5. They provide a clear view on how we plan to evolve the two runtimes individually, and also together.
