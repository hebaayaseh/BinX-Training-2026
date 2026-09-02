# Day 3 — Apply RBAC and Ownership Checks 

## Overview
Audited every endpoint in BookNest against its correct access requirement 
(public, any authenticated user, or Admin-only), and added explicit 
ownership checks so a Member can only access their own reservation data — 
not another Member's.

## Steps Completed

-  Confirmed the `Member` role is assigned by default at registration, 
      and an `Admin` account is seeded on application startup.
-  Reviewed every endpoint in the API and applied the correct role 
      requirement, documented below.
-  Added `GET /api/books/my-reservations` and 
      `GET /api/books/my-reservations/{id}` — both scoped to the 
      authenticated member via the `memberProfileId` JWT claim, with the 
      single-reservation endpoint performing an explicit ownership check 
      (`WHERE Id == id AND MemberId == memberProfileId`) before returning data.
-  Added `GET /api/books/all-reservations` (Admin-only) to have a second 
      Admin-restricted endpoint for testing role rejection.
-  Tested in Postman that a Member token is rejected with `403 Forbidden` 
      from both Admin-only endpoints (`POST /api/books` and 
      `GET /api/books/all-reservations`).
-  Tested in Postman that one Member's token cannot retrieve another 
      Member's specific reservation — the endpoint returns `404 Not Found` 
      rather than leaking a `403`, so the response doesn't confirm whether 
      the record exists at all.
-  Committed the implementation to `feature/rbac-ownership-checks`.

## Endpoint Access Matrix

| Endpoint | Method | Access Level |
|---|---|---|
| `/api/auth/register` | POST | Public |
| `/api/auth/login` | POST | Public |
| `/api/books` | GET | Public |
| `/api/books` | POST | Admin only |
| `/api/books/{id}/reserve` | POST | Member only |
| `/api/books/my-reservations` | GET | Member only (own data) |
| `/api/books/my-reservations/{id}` | GET | Member only (ownership-checked) |
| `/api/books/all-reservations` | GET | Admin only |

## Why the Ownership Check Returns 404, Not 403
Returning `403 Forbidden` when a reservation belongs to someone else would 
confirm to the caller that a record with that ID exists — an information 
leak. Returning `404 Not Found` in both cases (record doesn't exist, or 
belongs to another member) reveals nothing about other users' data.

## Why Ownership Is Checked at the Query Level
The check isn't "is this user a Member?" — it's "does this specific 
reservation belong to this specific member?". This is enforced directly in 
the database query (`WHERE MemberId == memberProfileId`) rather than 
fetched first and checked in code afterward, so unauthorized data is never 
even loaded into memory.

## Tools Used
- ASP.NET Core Authorization Policies (`AdminOnly`, `MemberOnly`)
- JWT custom claim (`memberProfileId`) for ownership scoping
- Postman (multi-account testing: two Member tokens + one Admin token)






