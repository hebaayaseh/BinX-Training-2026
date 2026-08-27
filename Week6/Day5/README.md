## Day 5 — CardioTrack Lab Management, Doctor Scheduling, Emergency Contacts & Audit Logging
## Overview

On this day, I extended the CardioTrack application with several new backend features covering lab test workflows, doctor scheduling, patient emergency contacts, and centralized audit logging. The goal was to apply core backend engineering concepts such as role-based validation, business rule enforcement, database transactions, and safe entity serialization when integrating cross-cutting concerns like audit logging.

## Steps Completed

- Lab Request Management

Implemented an endpoint allowing doctors to create one or more lab test requests for a patient in a single call.
Validated that the requesting doctor exists, is active, and holds the correct role before allowing request creation.
Validated that the target patient exists, and that an optional linked appointment belongs to that patient.
Implemented role-scoped viewing of lab requests: patients see only their own requests, doctors see only requests they created, and technicians/admins can view all requests.
Implemented manual status transitions (e.g., Seen, Collected, Cancelled) for lab requests, restricted to technicians and admins.
Prevented manually setting a request's status to "Completed," since that transition is only allowed to happen automatically when a result is uploaded.

- Lab Result Management

Implemented an endpoint allowing technicians to upload a lab result file for a patient, optionally linked to an existing lab request.
Added file upload handling that stores the result file on disk and saves its relative URL on the result record.
Prevented uploading more than one result against the same lab request.
Implemented an automatic status update: when a result is successfully created and linked to a lab request, the related lab request's status is automatically set to "Completed" within the same operation.
Implemented role-scoped viewing of lab results, following the same access rules as lab requests.
Added a manual status update endpoint for lab results (e.g., Checked, Delivered), restricted to technicians and admins.

- Doctor Schedule Management

Implemented an endpoint for doctors to add a weekly schedule slot, with conflict validation to prevent overlapping time ranges for the same doctor.
Implemented schedule updates that deactivate the old slot and create a new one, while automatically flagging any already-scheduled appointments that fall on the old slot as postponed.

- Emergency Contact Management

Implemented full CRUD operations (add, update, delete, view) for a patient's emergency contacts.

- Audit Log Integration

Integrated the existing IAuditLog service into every new feature above (lab requests, lab results, doctor schedules, emergency contacts), so that every create/update/status-change operation is recorded with the acting user, action, entity type, and before/after values.
Key Concepts Applied

- Transactional Consistency
Creating a lab result and auto-completing its related lab request are treated as a single unit of work, ensuring the database never ends up with a result that exists without its request being marked complete, or vice versa.

- Role-Based Authorization
Each endpoint enforces access using policy-based authorization (Doctor-only, Technician/Admin-only, or role-scoped viewers), rather than relying on ad-hoc checks scattered in controllers.

Safe Audit Logging & Avoiding Serialization Cycles
While wiring up IAuditLog across the new services, I ran into a System.Text.Json circular reference exception. Because Entity Framework Core automatically fixes up navigation properties between tracked entities (e.g., a Patient already tracked in the same DbContext gets linked back-and-forth with its LabRequests, or a Doctor with the Patients assigned to them), passing raw entity objects directly into the audit log caused infinite reference loops during JSON serialization. The fix was to always log lightweight anonymous snapshots containing only the relevant primitive fields, instead of the tracked entity itself — this is now a standing rule I follow for every log.LogAsync(...) call in the project.

- Data Scoping by Role
Query results for lab requests and lab results are filtered based on the caller's role and their relationship to the data (own patient record, own requested patients, or full visibility for lab/admin staff), rather than returning all records and filtering client-side.

## Tools

Entity Framework Core · ASP.NET Core Web API · MySQL (Pomelo) · LINQ · FluentValidation · Database Transactions · JWT Authentication · Visual Studio · .NET SDK