# REST Resource Map Lab

## Objective
The goal of this lab was to design a simple RESTful API for a Library Management System.

## Domain
Library Management System

## Core Resources
- Books
- Authors
- Members
- Loans

## Books Endpoints
- GET /api/v1/books
- GET /api/v1/books/{id}
- POST /api/v1/books
- PUT /api/v1/books/{id}
- DELETE /api/v1/books/{id}

## Nested Resource
- GET /api/v1/members/{memberId}/loans

## HTTP Status Codes
- 200 OK
- 201 Created
- 204 No Content
- 400 Bad Request
- 404 Not Found

## API Versioning
This API uses URI versioning:

`/api/v1/`

## What I Learned
- How to identify REST resources.
- How to design CRUD endpoints.
- How to use nested resources.
- How to choose appropriate HTTP status codes.
- How to apply API versioning.
