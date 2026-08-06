# Day 4 - Implement Full CRUD

## Objective
Implement a complete CRUD API for the primary resource.

## Implemented Endpoints

- **POST** `/api/Category`
  - Creates a new category.
  - Returns **201 Created** with a `Location` header.

- **GET** `/api/Category`
  - Returns all categories.
- **GET** `/api/Category/{id}`
  - Returns a category by ID.
  - Returns **404 Not Found** if the category does not exist.

- **PUT** `/api/Category/{id}`
  - Updates an existing category.
  - Returns **400 Bad Request** for invalid input.
  - Returns **404 Not Found** if the category does not exist.

- **DELETE** `/api/Category/{id}`
  - Deletes a category.
  - Returns **204 No Content** on success.
  - Returns **404 Not Found** if the category does not exist.

## Testing

All endpoints were tested using **Postman**, including:

- Successful request for each endpoint.
- One error case for each endpoint (invalid input or non-existing resource).

## Technologies

- ASP.NET Core Web API
- C#
- Postman 
