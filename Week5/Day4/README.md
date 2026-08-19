# Day 4 — Global Exception Handling Lab

## Overview

Implementing global exception handling in ASP.NET Core to provide consistent error responses, prevent sensitive exception details from being exposed, and log unexpected errors for troubleshooting.

## Steps Completed

* Implemented global exception-handling middleware to catch unhandled exceptions across the API.
* Configured the middleware to return a standardized `ProblemDetails` response to the client.
* Ensured that actual exception messages and stack traces are not exposed in API responses.
* Added structured logging using `ILogger` to record caught exceptions and relevant request context.
* Created a test endpoint that deliberately triggers an unhandled exception.
* Verified that the global middleware catches the exception and returns the expected error response.
* Removed redundant `try/catch` blocks from individual endpoints where exception handling is now covered by the global middleware.
* Tested the API to confirm consistent exception handling and logging behavior.

## Tools

ASP.NET Core · ILogger · Visual Studio · .NET SDK · Notion
 
