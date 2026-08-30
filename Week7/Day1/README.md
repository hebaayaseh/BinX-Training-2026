# Day 1 — Sprint 2 Planning & Wiring Identity into the Capstone 

## Sprint 2 — Identity Integration (BookNest)

## Sprint Goal
Integrate ASP.NET Core Identity into a small library reservation system 
(BookNest) to practice a clean Identity setup from scratch — replacing 
manual user/role management with Identity's built-in infrastructure — 
before considering a similar migration on the CardioTrack capstone project.

### Project Overview
BookNest is a minimal library reservation API. Members can browse available 
books and reserve a copy; Admins can add new books to the catalog.

### Identity Integration
- `ApplicationUser : IdentityUser<int>` — extends Identity's base user with 
  a custom `FullName` property.
- `BookNestDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>` 
  — replaces a manually-defined `User` table with Identity's full schema 
  (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, etc.).
- Roles seeded on startup: `Admin` and `Member`.
- Registration always assigns the `Member` role by default; an Admin account 
  is seeded separately.

### Authentication Flow
- `POST /api/auth/register` — creates a new `Member` account via 
  `UserManager.CreateAsync()`.
- `POST /api/auth/login` — validates credentials via 
  `UserManager.CheckPasswordAsync()`, retrieves the user's roles via 
  `UserManager.GetRolesAsync()`, and issues a JWT containing those roles as 
  claims.
- Protected endpoints use `[Authorize(Policy = "...")]` backed by 
  role-based authorization policies (`AdminOnly`, `MemberOnly`).

### Core Business Logic
`POST /api/books/{id}/reserve` — checks `AvailableCopies` before allowing a 
reservation; rejects with `409 Conflict` if no copies remain, and decrements 
the count on success.

## Role Structure

| Role | Equivalent to | Capabilities |
|---|---|---|
| Admin | admin-equivalent | Add new books to the catalog |
| Member | customer-equivalent | Browse books, reserve available copies |

## Endpoint → Role Requirements

| Endpoint | Method | Required Role |
|---|---|---|
| /api/auth/register | POST | Anonymous |
| /api/auth/login | POST | Anonymous |
| /api/books | GET | Anonymous |
| /api/books | POST | Admin |
| /api/books/{id}/reserve | POST | Member |

## Tech Stack
- ASP.NET Core Web API (.NET 8)
- ASP.NET Core Identity (`IdentityDbContext<ApplicationUser, IdentityRole<int>, int>`)
- Entity Framework Core + SQLite
- JWT Bearer Authentication


Seeded on startup:
- Roles: `Admin`, `Member`
- Admin account: `admin@booknest.com` / `Admin123!`
- Sample book: "Clean Code" by Robert C. Martin (3 copies)






