# Day 2 — Mocking Dependencies with Moq

## Overview

Practicing dependency mocking with Moq to isolate the service logic during unit testing.

## Steps Completed

* Reviewed why unit tests should isolate the service under test from real external dependencies.
* Used **Moq** to create a mock implementation of a repository interface.
* Configured mocked repository methods to return specific test data using `Setup()` and `ReturnsAsync()`.
* Injected the mocked repository into the service under test.
* Wrote a unit test to verify that the service processes mocked data correctly.
* Configured a mock dependency to throw an exception and tested the service's exception-handling behavior.
* Used `Verify()` to confirm that a repository method was called exactly once with the expected arguments.
* Focused on mocking external or costly dependencies rather than mocking every dependency unnecessarily.
* Ran the test suite and verified that the implemented tests pass.

## Tools

xUnit · Moq · .NET SDK · Visual Studio 




