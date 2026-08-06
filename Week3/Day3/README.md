## Day 3 — EF Core Setup & Code-First Migrations
## Overview
Setting up Entity Framework Core with MySQL (XAMPP) and running the first migration for the Library Management System database.

## Steps Completed
Added EF Core + Pomelo MySQL NuGet packages
Created entity classes for all tables from the Day 2 ERD (Book, Author, BookAuthor, Category, Member, Borrowing)
Created LibraryDbContext with a DbSet for each entity, registered in Program.cs
Configured the connection string in appsettings.Development.json (gitignored)
Next Steps
Run Add-Migration InitialCreate
Run Update-Database
Verify tables in phpMyAdmin

## Tools
MySQL (XAMPP) · Entity Framework Core 
