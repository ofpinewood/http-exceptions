# ASP.NET Core Problem Details for HttpExceptions
[![Build Status](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_apis/build/status/ofpinewood.http-exceptions?branchName=master)](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_build/latest?definitionId=6&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.HttpExceptions.AspNetCore.svg)](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/http-exceptions.svg)](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE)

ASP.NET Core Problem Details extensions for using HttpExceptions.

## Where can I get it?
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Opw.HttpExceptions.AspNetCore](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/) from the package manager console:

```
PM> Install-Package Opw.HttpExceptions.AspNetCore
```

## Getting started
Add HttpExceptions services to the services collection.
``` csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpExceptions();
}
```

`UseHttpExceptions` is the first middleware component added to the pipeline. Therefore, the `UseHttpExceptions` Middleware catches any exceptions that occur in later calls.
When using HttpExceptions you don't need to use `UseExceptionHandler` or `UseDeveloperExceptionPage`.
``` csharp
public void Configure(IApplicationBuilder app)
{
    app.UseHttpExceptions(); // this is the first middleware component added to the pipeline

    ...
}

```

## Samples
See [Opw.HttpExceptions.AspNetCore.Sample](/docs/Opw.HttpExceptions.AspNetCore.Sample.md).

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE).