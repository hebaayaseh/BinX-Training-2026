# CardioTrack 

CardioTrack is a backend REST API for a cardiac patient monitoring system.

The project was developed as an individual ASP.NET Core backend training project.
It demonstrates C# fundamentals, object-oriented programming, REST API development,
Entity Framework Core, database relationships, authentication and authorization,
input validation, error handling, automated testing, Swagger/OpenAPI, and Postman-based
API verification.

---

##  Project Overview

CardioTrack provides a backend system for managing patients and their cardiac
health-related information.

The system supports different user roles with role-based access:

- Admin
- Doctor
- Nurse
- Patient

Medical staff can manage patient information, record vital signs, manage medications,
create and manage medical histories, and manage appointments.

The system also automatically evaluates recorded vital signs and creates alerts when
measurements exceed the configured thresholds.

Patients have restricted access to their own information.

---

##  Project Objectives

The main objectives of this project are to demonstrate:

- C# and Object-Oriented Programming
- ASP.NET Core Web API
- RESTful API design
- Controllers and routing
- Dependency Injection
- Entity Framework Core
- LINQ
- Async/Await
- DTO-based request and response handling
- Database relationships
- JWT authentication
- Role-based authorization
- Input validation
- Centralized exception handling
- Automatic vital-sign alert generation
- Unit testing using xUnit
- Mocking using Moq
- API testing using Swagger and Postman

---

#  Technologies

## Backend

- C#
- ASP.NET Core Web API
- .NET 8

## Database

- MySQL
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- EF Core Migrations

## Authentication & Security

- JWT Bearer Authentication
- Role-based Authorization
- Password hashing
- Refresh Tokens
- Email verification

## API Documentation

- Swagger / OpenAPI
- Postman

## Testing

- xUnit
- Moq
- Entity Framework Core InMemory provider

---

#  User Roles

## Admin

The Admin can:

- Login
- Logout
- Create Doctor accounts
- Create Nurse accounts
- Manage staff accounts
- View staff information
- View patients
- Activate accounts
- Deactivate accounts
- Manage profile information

---

## Doctor

The Doctor can:

- Login
- Logout
- View and edit profile
- View assigned patients
- Manage their assigned patients
- Add medical history
- Manage medications
- Create appointments
- Update appointment status
- View appointments by status
- Record vital signs
- View vital signs
- View vital-sign alerts
- Resolve vital-sign alerts
- Activate/provision patient accounts

Doctors are restricted to their assigned patients where applicable.

---

## Nurse

The Nurse can:

- Login
- Logout
- View and edit profile
- Record vital signs
- View appointments for a specific doctor
- Filter appointments by status
- Update appointment status
- View vital-sign alerts
- Resolve vital-sign alerts
- Access basic patient information

---

## Patient

The Patient has restricted access to their own records.

Patients can:

- Login
- Logout
- View profile
- Edit profile/password
- View vital-sign history
- View active medications
- View appointments
- View medical history

The system also supports temporary-password and email verification flows.

---

#  Authentication & Authorization

CardioTrack uses JWT Bearer Authentication.

After successful login, the authenticated user receives a JWT token.

The token contains the information required by the application for authorization,
including:

- User ID
- User Role

Protected endpoints use authorization policies to restrict access according to the
user's role.

### Authorization Policies

The project defines role-based policies such as:

- `AdminOnly`
- `DoctorOnly`
- `NurseOnly`
- `DoctorOrNurse`
- `MedicalStaff`
- `PatientOnly`
- `AllActors`

### Authentication Flow

```text
Login
  │
  ▼
Validate Credentials
  │
  ▼
Generate JWT
  │
  ▼
Client sends Bearer Token
  │
  ▼
Authentication Middleware
  │
  ▼
Authorization Policy
  │
  ▼
Protected Endpoint
```
# Test Accounts Information
## Admin 
Email : heba.ayaseh04@hmail.com
Password : Heba1234@

## Doctor 
Email : ahmadayaseh@gmail.com
Password : Ahmad1234@

## Nurse 
Email : sameerayaseh@gmail.com
Password : Samerr1234@

## Patient 
Email : hebaayaseh17@gmail.com
Password : ibrahem1234@
