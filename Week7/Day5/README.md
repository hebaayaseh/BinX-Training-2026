# Day 5 - Summary — BookNest

## Sprint Goal
Integrate ASP.NET Core Identity, apply role-based access control across 
every endpoint, add ownership checks for member-specific data, and 
centralize exception handling as reusable middleware.

## Registration & Login Flow
1. POST /api/auth/register → creates ApplicationUser (Identity) + 
   MemberProfile (domain entity) in a single transaction; role defaults 
   to "Member"
2. POST /api/auth/login → validates credentials, retrieves roles via 
   UserManager, conditionally attaches memberProfileId claim (Members only — 
   Admins do not have a MemberProfile)
3. JWT claims: NameIdentifier, Email, Name, Role(s), and memberProfileId 
   (when applicable)

## RBAC Matrix

| Endpoint | Method | Access Level |
|---|---|---|
| /api/auth/register | POST | Public |
| /api/auth/login | POST | Public |
| /api/books | GET | Public |
| /api/books | POST | Admin only |
| /api/books/{id}/reserve | POST | Member only |
| /api/books/my-reservations | GET | Member only (own data) |
| /api/books/my-reservations/{id} | GET | Member only (ownership-checked) |
| /api/books/all-reservations | GET | Admin only |


## Key Bug Found & Fixed This Sprint
Admin login initially failed because the login logic unconditionally 
required a MemberProfile. Fixed by branching the profile lookup based on 
the user's role.



