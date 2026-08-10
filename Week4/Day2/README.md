# Day 2 — JWT Authentication & Token Issuance

Added JWT authentication to the Library Management System API to allow users to log in and access protected endpoints.

## Steps Completed

* Added ASP.NET Core Identity packages.
* Registered `AddIdentity<IdentityUser, IdentityRole>` in `Program.cs`.
* Configured `AddJwtBearer` authentication in `Program.cs`.
* Implemented the login flow using `AuthController` → `IAuthService` → `AuthService`.
* Used `SignInManager<IdentityUser>` to verify user credentials.
* Created a JWT containing the user's ID and email as claims.
* Configured the JWT issuer, audience, signing key, and token expiry.
* Tested the `/api/v1/auth/login` endpoint in Postman with valid and invalid credentials.
* Verified that invalid login attempts return `401 Unauthorized`.

## Tools

ASP.NET Core Identity • JWT Bearer Authentication • System.IdentityModel.Tokens.Jwt • Postman
