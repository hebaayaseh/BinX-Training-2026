# Day 4 — Custom Middleware 

## Overview
Identified centralized exception handling as a genuine cross-cutting 
concern missing from BookNest, implemented it as custom middleware, and 
opened a pull request summarizing all Sprint 2 work (Identity integration, 
RBAC, ownership checks, and this middleware).

## Steps Completed

-  Identified that BookNest had no centralized error handling — 
      unhandled exceptions were returning ASP.NET Core's default developer 
      exception page, including full stack traces, to any client.
-  Implemented `ExceptionMiddleware`, registered as the first middleware 
      in the pipeline, converting all exceptions into RFC 7807 
      `ProblemDetails` responses.
-  Built a custom exception hierarchy (`AppException` base class with 
      `NotFoundException`, `BadRequestException`, `ConflictException`), 
      each carrying its own HTTP status code.
-  Refactored controller logic to throw exceptions instead of manually 
      returning error responses, removing the need for per-endpoint 
      try/catch blocks.
-  Tested across multiple endpoints (`reserve`, and a temporary 
      diagnostic endpoint) confirming the middleware catches exceptions 
      consistently without any endpoint-specific handling code.
-  Pushed to `feature/sprint2-middleware` and opened a pull request 
      summarizing the authentication, RBAC, and middleware work from 
      this sprint.

## Why This Is a Genuine Cross-Cutting Concern
Error handling isn't specific to any one endpoint — it applies uniformly 
across the entire API surface. Without centralizing it, every controller 
would need repeated try/catch logic, risking inconsistent responses and 
accidental leakage of internal details in endpoints someone forgot to wrap.

## Tools Used
- ASP.NET Core custom middleware
- `Microsoft.AspNetCore.Mvc.ProblemDetails`
- `ILogger<T>` for server-side error logging
