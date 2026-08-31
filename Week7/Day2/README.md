# Day 2 — Registration & Login with Linked Domain Entity 

## Overview
Extended BookNest's Identity setup by introducing a separate domain entity 
(`MemberProfile`) linked to `ApplicationUser` via a foreign key, and 
implemented a full registration/login flow that creates both records 
transactionally and issues a JWT carrying the domain entity's ID as a claim.

## Steps Completed

- Added `MemberProfile` entity with a foreign key (`ApplicationUserId`) 
      linking it to `ApplicationUser`, configured as a 1:1 relationship via 
      a unique index and `HasOne().WithOne()`.
- Implemented `POST /api/auth/register` — creates both the 
      `ApplicationUser` (via `UserManager.CreateAsync`) and the linked 
      `MemberProfile` inside a single database transaction, rolling back 
      both if either step fails.
- Implemented `POST /api/auth/login` — validates credentials, retrieves 
      the linked `MemberProfile`, and issues a JWT containing a custom 
      `memberProfileId` claim alongside the standard identity claims 
      (`NameIdentifier`, `Email`, roles).
- Tested the full flow in Postman: registered a new member, confirmed 
      both `AspNetUsers` and `MemberProfiles` records exist with matching 
      IDs, logged in, and decoded the returned JWT on jwt.io to verify the 
      `memberProfileId` claim matches the profile created at registration.
- Committed the implementation to `feature/registration-login-transaction`.

## Why a Separate Domain Entity
`ApplicationUser` represents identity (login credentials, security stamps, 
lockout state). `MemberProfile` represents domain-specific data (`FullName`, 
`JoinedAt`, `MaxBooksAllowed`) that belongs to the business domain, not to 
authentication. Keeping them separate avoids polluting the Identity table 
with application-specific fields and mirrors how a larger system (e.g. a 
`Patient` or `Customer` table) would relate to its Identity user.

## Why a Transaction
Without wrapping both inserts in a transaction, a failure after the 
`ApplicationUser` is created (but before `MemberProfile` is saved) would 
leave an orphaned login-capable account with no domain profile. The 
transaction guarantees both records are created together or not at all.

## Why a Custom JWT Claim
Domain operations (e.g. reserving a book) need the `MemberProfile` ID 
directly. Without the claim, every request would require an extra database 
lookup to resolve `ApplicationUser.Id → MemberProfile.Id`. Embedding it in 
the token avoids that round trip.

## Tools Used
- ASP.NET Core Identity (`UserManager<ApplicationUser>`)
- Entity Framework Core (`BeginTransactionAsync` / `CommitAsync` / `RollbackAsync`)
- JWT Bearer Authentication
- Postman + jwt.io (manual token verification)






