# HttpExceptions
[![Build Status](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_apis/build/status/ofpinewood.http-exceptions?branchName=master)](https://ofpinewood.visualstudio.com/Of%20Pine%20Wood/_build/latest?definitionId=6&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.HttpExceptions.svg)](https://www.nuget.org/packages/Opw.HttpExceptions/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/http-exceptions.svg)](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE)

HTTP-specific exception classes that enable ASP.NET to generate exception information. These classes can be used by themselves or as base classes for your own HttpExceptions.

## Where can I get it?
You can install [Opw.HttpExceptions](https://www.nuget.org/packages/Opw.HttpExceptions/) from the NuGet package manager console:

```
PM> Install-Package Opw.HttpExceptions
```

## HttpExceptions

### 3xx
- ...

### 4xx
- 400 BadRequestException
- 400 InvalidModelException
- 400 InvalidFileException
- 401 UnauthorizedException
- 403 ForbiddenException
- 404 NotFoundException
- 404 NotFoundException\<T\>
- 409 ConflictException
- 409 ProtectedException
- 415 UnsupportedMediaTypeException

### 5xx
- 500 InternalServerErrorException
- 500 DbErrorException
- 500 SerializationErrorException
- 503 ServiceUnavailableException

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/http-exceptions/blob/master/LICENSE).