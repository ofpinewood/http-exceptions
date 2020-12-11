# HttpExceptions <img src="http-exceptions-logo-256x256.gif" alt="PineBlog" height="44" align="left" />

## Return exceptions over HTTP
[![Build Status](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_apis/build/status/ofpinewood.http-exceptions?branchName=master)](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_build/latest?definitionId=6&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.HttpExceptions.AspNetCore.svg)](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/http-exceptions.svg)](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE)

Middleware and extensions for returning exceptions over HTTP, e.g. as ASP.NET Core Problem Details.
Problem Details are a machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
But you are not limited to returning exception results as Problem Details, but you can create your own mappers for your own custom formats.

### Where can I get it?
You can install [Opw.HttpExceptions.AspNetCore](https://www.nuget.org/packages/Opw.HttpExceptions.AspNetCore/) from the console.

``` cmd
> dotnet add package Opw.HttpExceptions.AspNetCore
```

### Getting started
Add the HttpExceptions services and the middleware in the `Startup.cs` of your application. First add HttpExceptions using the `IMvcBuilder` of `IMvcCoreBuilder`.

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc().AddHttpExceptions();
    // or services.AddMvcCore().AddHttpExceptions();
    ...
}
```

Then you can add the HttpExceptions middleware using the application builder.  `UseHttpExceptions` should be the first middleware
component added to the pipeline. That way the `UseHttpExceptions` Middleware catches any exceptions that occur in later calls. When
using HttpExceptions you don't need to use `UseExceptionHandler` or `UseDeveloperExceptionPage`.

``` csharp
public void Configure(IApplicationBuilder app)
{
    app.UseHttpExceptions(); // this is the first middleware component added to the pipeline
    ...
}
```

#### InvalidModelStateResponseFactory API behavior
HttpExceptions overrides the default `Microsoft.AspNetCore.Mvc.ApiBehaviorOptions.InvalidModelStateResponseFactory` and related settings and
will use the configured ExceptionMappers.

### Configuring options
You can extend or override the default behavior through the configuration options, `HttpExceptionsOptions`.

#### Include exception details
Whether or not to include the full exception details in the response. The default behavior is only to include exception details in a development environment.

``` csharp
mvcBuilder.AddHttpExceptions(options =>
{
    // This is the same as the default behavior; only include exception details in a development environment.
    options.IncludeExceptionDetails= context => context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
});
```

#### Is exception response
Is the response an exception and should it be handled by the HttpExceptions middleware.

``` csharp
mvcBuilder.AddHttpExceptions(options =>
{
    // This is a simplified version of the default behavior; only map exceptions for 4xx and 5xx responses.
    options.IsExceptionResponse = context => (context.Response.StatusCode < 400 && context.Response.StatusCode >= 600);
});
```

#### Should log exceptions (ILogger)
Should an exception be logged by the HttpExceptions middleware or not, default behavior is to log all exceptions (all status codes).

``` csharp
mvcBuilder.AddHttpExceptions(options =>
{
    // Only log the when it has a status code of 500 or higher, or when it not is a HttpException.
    options.ShouldLogException = exception => {
        if ((exception is HttpExceptionBase httpException && (int)httpException.StatusCode >= 500) || !(exception is HttpExceptionBase))
            return true;
        return false;
    };
});
```

####  ProblemDetails property mappings
You can inject your own mappings for the `ProblemDetails` properties using functions on the `HttpExceptionsOptions`, or by creating your own `IExceptionMapper` and/or `IHttpResponseMapper`. If you inject your own function that will be tried first, and if no result is returned the defaults will be used.

In the following example we will override the `ProblemDetails.Type` property. By default the `ProblemDetails.Type` property will be set by:

1. Either the `Exception.HelpLink` or the HTTP status code information link.
2. Or the `DefaultHelpLink` will be used.
3. Or an URI with the HTTP status name ("error:[status:slug]") will be used.

When the `ExceptionTypeMapping` or `HttpContextTypeMapping` are set the result of those functions will be tried first, if no result is returned the defaults will be used.

``` csharp
mvcBuilder.AddHttpExceptions(options =>
{
    ExceptionTypeMapping = exception => {
        // This is a example, you can implement your own logic here.
        return "My Exception Type Mapping";
    },
    HttpContextTypeMapping = context => {
        // This is a example, you can implement your own logic here.
        return "My  HttpContext Type Mapping";
    }
});
```

#### Custom ExceptionMappers
Set the ExceptionMapper collection that will be used during mapping. You can override and/or add ExceptionMappers for specific
exception types. The ExceptionMappers are called in order so make sure you add them in the right order.

By default there is one ExceptionMapper configured, that ExceptionMapper catches all exceptions. 

``` csharp
mvcBuilder.AddHttpExceptions(options =>
{
    // Override and or add ExceptionMapper for specific exception types, the default ExceptionMapper catches all exceptions.
    options.ExceptionMapper<BadRequestException, BadRequestExceptionMapper>();
    options.ExceptionMapper<ArgumentException, ExceptionMapper<ArgumentException>>();
    // The last ExceptionMapper should be a catch all, for type Exception.
    options.ExceptionMapper<Exception, MyCustomExceptionMapper>();
});
```

### Sample project using HttpExceptions middleware
See the `samples/Opw.HttpExceptions.AspNetCore.Sample` project for a sample implementation. This project contains examples on how to use the HttpExceptions middleware.

**Please see the code** :nerd_face

## HttpExceptions
[![Build Status](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_apis/build/status/ofpinewood.http-exceptions?branchName=master)](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_build/latest?definitionId=6&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.HttpExceptions.svg)](https://www.nuget.org/packages/Opw.HttpExceptions/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/http-exceptions.svg)](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE)

HTTP-specific exception classes that enable ASP.NET to generate exception information. These classes can be used by themselves or as base classes for your own HttpExceptions.

### Where can I get it?
You can install [Opw.HttpExceptions](https://www.nuget.org/packages/Opw.HttpExceptions/) from the console.

``` cmd
> dotnet add package Opw.HttpExceptions
```

### HttpExceptions

#### 4xx
- 400 BadRequestException
- 400 InvalidModelException
- 400 ValidationErrorException\<T\>
- 400 InvalidFileException
- 401 UnauthorizedException
- 403 ForbiddenException
- 404 NotFoundException
- 404 NotFoundException\<T\>
- 409 ConflictException
- 409 ProtectedException
- 415 UnsupportedMediaTypeException

#### 5xx
- 500 InternalServerErrorException
- 500 DbErrorException
- 500 SerializationErrorException
- 503 ServiceUnavailableException

## Contributing
We accept fixes and features! Here are some resources to help you get started on how to contribute code or new content.

* [Contributing](https://github.com/ofpinewood/http-exceptions/blob/master/CONTRIBUTING.md)
* [Code of conduct](https://github.com/ofpinewood/http-exceptions/blob/master/CODE_OF_CONDUCT.md)

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE).
