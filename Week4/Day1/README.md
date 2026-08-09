# Day 1 — Set Up Identity & Implement Registration

Adding ASP.NET Core Identity to the Library Management System API for user registration.

## Steps Completed
- Added Identity NuGet packages, updated `LibraryDbContext` to inherit from `IdentityDbContext`
- Ran migration to add Identity schema, applied to the database
- Registered `AddIdentity<IdentityUser, IdentityRole>` in `Program.cs`
- Built registration flow: `AuthController` → `IAuthService` → `AuthService` (uses `UserManager`)
- Tested `/api/v1/auth/register` in Postman with valid and weak-password requests

## Tools
ASP.NET Core Identity · Entity Framework Core · Postman