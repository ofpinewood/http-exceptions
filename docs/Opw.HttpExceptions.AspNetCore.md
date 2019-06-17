# Return exceptions as ASP.NET Core Problem Details
[![Build Status](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_apis/build/status/ofpinewood.http-exceptions?branchName=master)](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_build/latest?definitionId=6&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.HttpExceptions.AspNetCore.svg)](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/http-exceptions.svg)](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE)

Extensions for returning exceptions as ASP.NET Core Problem Details. Problem Details are a machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.

## Where can I get it?
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Opw.HttpExceptions.AspNetCore](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/) from the package manager console:

``` cmd
PM> Install-Package Opw.HttpExceptions.AspNetCore
```

## Getting started
Add HttpExceptions services to the services collection.

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHttpExceptions();
    ...
}
```

`UseHttpExceptions` should be the first middleware component added to the pipeline. That way the `UseHttpExceptions` Middleware catches any exceptions that occur in later calls.
When using HttpExceptions you don't need to use `UseExceptionHandler` or `UseDeveloperExceptionPage`.

``` csharp
public void Configure(IApplicationBuilder app)
{
    app.UseHttpExceptions(); // this is the first middleware component added to the pipeline
    ...
}
```

## Configuring Options
You can extend or override the default behaviour through the configuration options, `HttpExceptionsOptions`.

### Include exception details
Include exception details, default behavior is only to include exception details in a development environment.

``` csharp
services.AddHttpExceptions(options =>
{
    // This is the same as the default behavior; only include exception details in a development environment.
    options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
});
```

### Is exception response
Is the response an exception and should it be handled by the HttpExceptions middleware.

``` csharp
services.AddHttpExceptions(options =>
{
    // This is a simplified version of the default behavior; only include exception details for 4xx and 5xx responses.
    options.IsExceptionResponse = context => (context.Response.StatusCode < 400 && context.Response.StatusCode >= 600);
});
```

### Custom ExceptionMappers
Gets or sets the ExceptionMapper collection that will be used during mapping. You can verride and/or add ExceptionMappers for specific exception types. The ExceptionMappers are called in order so make sure you add them in the right order.

By default there is one ExceptionMapper configured, that ExceptionMapper catches all exceptions. 

``` csharp
services.AddHttpExceptions(options =>
{
    // Override and or add ExceptionMapper for specific exception types, the default ExceptionMapper catches all exceptions.
    options.ExceptionMapper<BadRequestException, BadRequestExceptionMapper>();
    options.ExceptionMapper<ArgumentException, ExceptionMapper<ArgumentException>>();
    // The last ExceptionMapper should be a catch all, for type Exception.
    options.ExceptionMapper<Exception, MyCustomExceptionMapper>();
});
```

## Samples
See [Opw.HttpExceptions.AspNetCore.Sample](/docs/Opw.HttpExceptions.AspNetCore.Sample.md).

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE).