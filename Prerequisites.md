```markdown
<img align="left" width="116" height="116" src="./Assets/net-logo.svg" />

# &nbsp;**.NET Incubator Prerequisites**

This document lists knowledge and environment prerequisites for the .NET Incubator.

## Recommended reading & concepts

- .NET Fundamentals - [Read more...](https://github.com/entelect-incubator/.NET/tree/master/Fundamentals)
- .NET overview and fundamentals - [Read more...](https://learn.microsoft.com/dotnet/core/introduction)
- Coding standards and best practices - [9 Coding Standards](https://blog.submain.com/coding-standards-c-developers-need/)

### Key platform concepts to be familiar with

1. The Startup/Host configuration pipeline
2. Dependency injection (services)
3. Middleware
4. Servers (Kestrel)
5. Configuration
6. Options pattern
7. Environments (Development, Staging, Production)
8. Logging
9. Routing
10. Error handling
11. Making HTTP requests
12. Static files

## Architectural patterns

The incubator uses small, focused solutions that follow the Single Responsibility Principle and a Clean Architecture (layered) approach. We use CQRS in later phases; you don't need to master it before starting, but familiarity helps.

## Implicit usings

The projects in this incubator use implicit usings where appropriate. See: https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-10#global-using-directives

## Entity Framework Core

- What it is: https://learn.microsoft.com/ef/core/
- What's new in recent EF Core versions: https://learn.microsoft.com/ef/core/what-is-new/

## Setup

- Install .NET SDK 10 (or later) — ensure `dotnet --version` reports a 10.x SDK.
- Install Visual Studio 2022/2023 (or VS Code) with .NET workloads if you prefer.

## Back to Intro

[Intro](https://github.com/entelect-incubator/.NET#intro)

```
