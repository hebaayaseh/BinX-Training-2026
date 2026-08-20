# Day 5 — Global Exception Handling

## Overview
Implemented centralized exception-handling middleware for the CardioTrack API. 
All unhandled exceptions are now caught in a single place, logged with request 
context, and returned to the client as standardized `ProblemDetails` responses 
without exposing internal exception details or stack traces.

## Steps Completed
- Implemented global exception-handling middleware returning `ProblemDetails` for any unhandled exception.
- Confirmed the response never leaks the actual exception message or stack  trace to the client.
- Added structured logging (`ILogger`) for caught exceptions, including request path and method.
- Added a temporary test endpoint to deliberately trigger an unhandled exception and confirmed the middleware catches it correctly.
- Confirmed no redundant try/catch blocks exist in individual endpoints — the global handler covers all of them.

## Tools
- ASP.NET Core Middleware
- `Microsoft.AspNetCore.Mvc.ProblemDetails`
- `ILogger<T>`
- xUnit + `WebApplicationFactory` (integration test for the middleware)
 
