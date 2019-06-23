Handle error responses without an exception.

By default override the `Microsoft.AspNetCore.Mvc.ApiBehaviorOptions.InvalidModelStateResponseFactory` and related settings
and use the configured ExceptionMappers. This can be disabled by setting `HttpExceptionsOptions.UseInvalidModelStateResponseFactory`.

As part of this release we had [1](https://github.com/ofpinewood/http-exceptions/milestone/2?closed=1) issues closed.

## Bugs
No bugs were fixed in this release.

## Improvements/Features
* [#7](https://github.com/ofpinewood/http-exceptions/issues/7) Handle error responses without an exception 
