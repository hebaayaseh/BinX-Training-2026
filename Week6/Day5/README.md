# Day 4 — CardioTrack Appointment Booking & Transactions

## Overview

On Day 4, I enhanced the Appointment functionality in the CardioTrack application by implementing appointment availability checks, dynamic appointment fee calculation, and database transactions.

The goal was to apply important backend concepts such as validation before processing data, calculating values based on business rules, and ensuring data consistency using transactions.

## Steps Completed

Enhanced the existing Appointment functionality without creating any additional tables.

Added validation to ensure that the authenticated user exists, is active, and has the appropriate role to create appointments.

Validated that the selected patient exists and is assigned to the requested doctor.

Implemented an availability check to prevent scheduling multiple appointments for the same doctor at the same time.

Added conflict validation to ensure that an already scheduled time slot cannot be booked again.

Implemented appointment fee calculation based on the appointment reason.

Added a `Fee` property to the Appointment entity to store the calculated appointment cost.

Added support for an optional `RelatedAlertId` when an appointment is created as a response to a Vital Sign Alert.

Implemented a database transaction to ensure that multiple related operations are completed successfully as a single unit.

The transaction handles:

* Creating the new appointment.
* Resolving the related Vital Sign Alert when `RelatedAlertId` is provided.
* Saving both changes together.

Implemented transaction rollback handling to ensure that if any operation fails, all changes are reverted and the database remains consistent.

Implemented transaction commit only after all operations are successfully completed.

## Key Concepts Applied

### Availability Check

Before creating an appointment, the system checks whether the selected doctor already has a scheduled appointment at the requested date and time.

This prevents double booking and ensures proper appointment scheduling.

### Appointment Fee Calculation

The appointment fee is calculated dynamically based on the provided appointment reason.

A dedicated method was created to handle the business logic for calculating appointment costs.

### Database Transactions

A transaction is used to guarantee data consistency when creating an appointment and resolving a related alert.

Both operations must succeed together. If any error occurs during the process, the transaction is rolled back and no partial changes are saved.

## Tools

Entity Framework Core · ASP.NET Core Web API · SQL Server · LINQ · Database Transactions · JWT Authentication · Visual Studio · .NET SDK
