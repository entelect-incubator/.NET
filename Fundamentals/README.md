# .NET Fundamentals

## What is .NET?

Watch intro video

 [![What is .NET?](https://i.ytimg.com/vi_webp/eIHKZfgddLM/hqdefault.webp)](https://youtu.be/eIHKZfgddLM?list=PLdo4fOcmZ0oWoazjhXQzBKMrFuArxpW80)

![.NET landscape](https://weblog.west-wind.com/images/2016/ASP.NET%20Core%20Overview/AspNetCoreToday.png)

## History

.NET is a general-purpose development platform. It can be used for any kind of app type or workload where general-purpose solutions are used. It has several key features that are attractive to many developers, as it combines high performance and richness with unique, differentiated developer productivity.

​.NET Framework is a very powerful and mature framework, with a huge class library that supports a wide variety of applications and solutions, only supported on Windows. The last version that will be shipped by Microsoft will be .NET 4.8.

.NET is a new version of .NET which is cross-platform, open-source and modular. It can be used for creating modern web apps, microservices, libraries and console applications that run everywhere (Windows, Linux and macOS). The last version of .NET that will be shipped will be .NET 3.1.

Unlike the traditional .NET Framework, which is a single package installation, system-wide, and Windows-only runtime environment, .NET is about decoupling .NET from Windows, allowing it to run in non-Windows environments without having to install a giant 400MB set of binaries (versus just the footprint of the components you need from .NET) plus the ability to deploy the applications coming with the framework itself supporting side-by-side execution of different versions of the framework.
With the Xamarin Platform, you can deliver native Android, iOS, macOS and Windows apps, using existing .NET skills, teams, and code.

The Xamarin Platform is based on Mono. Mono is the original open-source and cross-platform implementation of .NET, from the community Mono Project. It is now sponsored by Xamarin/Microsoft. Traditionally, Mono’s APIs have followed the progress of the .NET Framework, not .NET.

Open source is also an important part of the .NET ecosystem, with multiple .NET implementations and many libraries available under OSI-approved licenses.

## Future

There will be just one .NET going forward, and you will be able to use it to target Windows, Linux, macOS, iOS, Android, tvOS, watchOS and WebAssembly and more.

We will introduce new .NET APIs, runtime capabilities and language features as part of .NET.

![.NET a unified platform](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2022/11/dotnet-platform2.png)

![.NET schedule](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2022/11/NET-Release-Schedule.png)

## Runtime experiences

Mono is the original cross-platform implementation of .NET. It started as an open-source alternative to .NET Framework and transitioned to targeting mobile devices as iOS and Android devices became popular. Mono is the runtime used as part of Xamarin.

CoreCLR is the runtime used as part of .NET. It has been primarily targeted at supporting cloud applications, including the largest services at Microsoft, and now is also being used for Windows desktop, IoT and machine learning applications.

Taken together, the .NET and Mono runtimes have a lot of similarities (they are both .NET runtimes after all) but also valuable unique capabilities. It makes sense to make it possible to pick the runtime experience you want. We’re in the process of making CoreCLR and Mono drop-in replacements for one another. We will make it as simple as a build switch to choose between the different runtime options.

The following sections describe the primary pivots we are planning for .NET 5. They provide a clear view of how we plan to evolve the two runtimes individually, and also together.

- [ ] .NET Overview - [Read more...](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows)
- [ ] Coding Standards - [9 Coding Standards](https://blog.submain.com/coding-standards-c-developers-need/)
- [ ] Coding Standards - [Coding Guidelines And Best Practices](https://www.c-sharpcorner.com/blogs/c-sharp-coding-guidelines-and-best-practices-v10)

## Back to Intro

[Intro](https://github.com/entelect-incubator/.NET#intro)