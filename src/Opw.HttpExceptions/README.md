# Http Exceptions
HTTP-specific exception classes that enable ASP.NET to generate exception information. These classes can be used by themself or as base classes for your own HttpExceptions.

## 3xx
- ...

## 4xx
- 400 BadRequestException
- 400 InvalidModelException
- 400 InvalidFileException
- 401 UnauthorizedException
- 403 ForbiddenException
- 404 NotFoundException
- 404 NotFoundException<T>
- 409 ConflictException
- 409 ProtectedException
- 415 UnsupportedMediaTypeException

## 5xx
- 500 InternalServerErrorException
- 500 DbErrorException
- 500 SerializationErrorException
- 503 ServiceUnavailableException

# ASP.NET Core Problem Details for Http Exceptions
ASP.NET Core Problem Details extensions for using Http Exceptions).