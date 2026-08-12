# Day 4 — Add Validators to Existing Endpoints

Adding request validation to Create/Update endpoints using FluentValidation.

## Steps Completed
- Installed FluentValidation and its ASP.NET Core integration package
- Wrote a validator for the Create request model, covering required fields, max length, and a business rule
- Wrote a validator for the Update request model
- Registered validators in `Program.cs`; invalid requests now return a structured 400 response automatically
- Tested each validation rule individually in Postman, confirming the returned error messages

## Tools
FluentValidation · Postman