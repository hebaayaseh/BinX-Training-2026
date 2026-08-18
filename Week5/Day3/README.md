# Day 3 — Integration Testing with WebApplicationFactory

## Overview

Implementing integration tests with `WebApplicationFactory` to test the API through real HTTP requests and verify the behavior of the application as a whole.

## Steps Completed

* Configured `WebApplicationFactory` to host the API in-memory for integration testing.
* Created an `HttpClient` to send requests to the API without using a real network connection.
* Added integration tests for real API endpoints and verified their HTTP responses.
* Tested the **Get-by-id** endpoint for both successful and not-found scenarios.
* Configured a separate test database or in-memory database to keep integration tests isolated from the development database.
* Tested an authenticated endpoint using a valid test JWT.
* Verified HTTP status codes and response data returned by the API.
* Ran the integration test suite and verified the implemented tests pass.

## Tools

Microsoft.AspNetCore.Mvc.Testing · .NET SDK · Visual Studio 
