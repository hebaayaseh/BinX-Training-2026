# Day 5 — Securing the API: Rate Limiting, CORS & Security Headers

Adding request validation to Create/Update endpoints using FluentValidation.

## Steps Completed
- Configured rate limiting to protect against brute-force and DoS patterns
- Configured CORS to restrict API access to a known frontend origin only
- Enabled HTTPS redirection and HSTS
- Reviewed codebase for raw SQL / SQL injection risks

### 1. Rate Limiting
- Used built-in .NET rate limiting (`AddRateLimiter`), no external package.
- Two fixed-window policies:
  - `General`: 100 requests / minute — applied broadly to controllers.
  - `Login`: 5 requests / minute — applied specifically to `POST /api-auth/login` to slow brute-force attempts.
- `UseRateLimiter()` added to the middleware pipeline after `UseAuthorization()`.

### 2. CORS
- Added a named policy `AllowFrontend` restricted to a specific origin (no wildcard `AllowAnyOrigin`).
- `UseCors("AllowFrontend")` placed before `UseAuthentication()` / `UseAuthorization()` in the pipeline — required ordering.
- **TODO:** replace placeholder origin `https://localhost:3000` with the real production frontend URL once known.

### 3. Security Headers
- `UseHttpsRedirection()` was already present.
- Added `UseHsts()`, applied only outside `Development` (HSTS forces HTTPS long-term on the browser side, unwanted during local dev).
- Content-Security-Policy header: not yet added — flagged as a follow-up item.

### 4. SQL Injection Review
- Reviewed `AuthService`, category services — all queries go through EF Core LINQ / Identity's `UserManager`, no raw SQL used.
- No instances of `FromSqlRaw` with string interpolation found.
- Action item going forward: any future raw SQL must use `FromSqlInterpolated` or explicit parameters, never string interpolation into `FromSqlRaw`.

## Tools
FluentValidation · Postman