# Day 3 — Authorization & Role-Based Access Control

Added role-based authorization to the Library Management System API to restrict specific endpoints (like category deletion) to Admin users only.

## Steps Completed
- Added UserRole enum (Admin, Customer) under LibraryManagment.Enum.
- Configured AddAuthorization in Program.cs with an Admin policy using RequireRole.
- Applied [Authorize] at the controller level and [Authorize(Roles = nameof(UserRole.Admin))] on the DeleteCategory action to restrict it to Admins.
- Embedded the user's roles as ClaimTypes.Role claims inside the JWT during token generation (GenerateJwtToken).
- Fixed a bug where the JWT token wasn't being awaited (GenerateJwtToken), which caused an invalid token to be issued and returned.
- Implemented DbSeeder to seed default Admin and Customer roles and test users (userAdmin@gmail.com, userCustomer@gmail.com) on application startup.
- Added JWT Bearer security definition to Swagger (AddSecurityDefinition + AddSecurityRequirement) to enable the Authorize button and allow sending tokens directly from Swagger UI.
- Tested protected endpoints (/api-category/delete-category/{id}) in Swagger:
- Verified requests without a token return 401 Unauthorized.
- Verified requests with a valid Customer token (non-Admin) return 403 Forbidden.
- Verified requests with a valid Admin token succeed.
## Tools

ASP.NET Core Identity • JWT Bearer Authentication • Role-Based Authorization • Swagger/Swashbuckle • Postman