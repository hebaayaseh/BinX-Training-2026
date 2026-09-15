# CardioTrack

Backend REST API for a Cardiac Patient Monitoring Center, built with 
ASP.NET Core. Supports four roles (Admin, Doctor, Nurse, Patient) with 
role-based authorization, automatic vital sign alert generation, and a 
complete medical workflow — patient management, medications, appointments, 
and medical history.

## Tech Stack

- **Backend:** C# / .NET 8, ASP.NET Core Web API
- **Database:** MySQL, Entity Framework Core (Pomelo provider)
- **Auth:** JWT Bearer Authentication, Refresh Token Rotation, Role-based 
  Authorization Policies
- **Caching:** Redis (cache-aside pattern)
- **Validation:** FluentValidation
- **Testing:** xUnit, Moq, EF Core InMemory
- **Documentation:** Swagger / OpenAPI, Postman

## Prerequisites

- .NET 8 SDK
- MySQL Server (8.0+)
- Docker (for Redis)
- Visual Studio 2022 or VS Code

## Environment Variables / Configuration

Copy `appsettings.json` to `appsettings.Development.json` and fill in the 
following (this file is gitignored and never committed):

```json
{
  "ConnectionStrings": {
    "Connection": "server=localhost;database=CardioTrack;user=root;password=YOUR_PASSWORD;",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "Issuer": "CardioTrack",
    "Audience": "CardioTrackUsers"
  },
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "Username": "your-email@gmail.com",
    "Password": "YOUR_APP_PASSWORD",
    "From": "your-email@gmail.com",
    "DisplayName": "CardioTrack"
  }
}
```

## Setup Instructions

1. **Clone the repository**
```bash
   git clone https://github.com/hebaayaseh/BinX-Training-2026.git
   cd Project/CardioTrack/CardioTrack
```

2. **Start Redis via Docker**
```bash
   docker run --name cardiotrack-redis -p 6379:6379 -d redis
```
   (For subsequent runs after restarting your machine: `docker start cardiotrack-redis`)

3. **Configure your database connection**
   Set up `appsettings.Development.json` as shown above.

4. **Apply migrations**
```bash
   dotnet ef database update
```
   Migrations run automatically on application startup as well.

5. **Run the application**
```bash
   dotnet run
```
   Swagger UI available at `https://localhost:[port]/swagger`.

6. **Seed data**
   Runs automatically on first startup — creates an Admin account, sample 
   doctors, nurses, patients, and 50+ records across core tables for 
   realistic testing.

## Default Test Accounts

| Role | Email | Password |
|---|---|---|
| Admin | heba.ayaseh04@gmail.com | Heba1234@ |
| Doctor | ahmadayaseh@gmail.com | Ahmad1234@ |
| Nurse | sameerayaseh@gmail.com | Samerr1234@ |
| Patient | hebaayaseh17@gmail.com | ibrahem1234@ |

## Running Tests

```bash
dotnet test
```

## API Documentation

- **Swagger UI:** available at `/swagger` when running locally
- **Postman Collection:** [CardioTrack_postman_collection.json](./CardioTrack_postman_collection.json) 
  — includes all endpoints organized by role, with test scripts

## Project Structure

├── Controllers/ API endpoints and routing
├── Services/ Business logic
├── Interfaces/ Service contracts (for DI and testing)
├── Models/ EF Core entities
├── DTOs/ Request/response models
├── Validators/ FluentValidation rules per DTO
├── Middleware/ Centralized exception handling
├── Data/ DbContext and seed data
└── CardioTrack.Tests/ Unit and integration tests


## Migration History

Run `dotnet ef migrations list` to see the full history, or check the 
`Migrations/` folder directly.

## Known Limitations

- Caching strategy caches full unfiltered patient lists per doctor; may 
  need revisiting at very large scale (hundreds of patients per doctor)
- Email sending is synchronous (no background queue) — acceptable at 
  current scale, documented as a future improvement

## License

Individual training project — BinX Tech ASP.NET Backend Track.