# FitTrack

`FitTrack` is a role-based fitness tracking web app built with ASP.NET Core Razor Pages (.NET 10).

It supports `User`, `Trainer`, and `Admin` workflows with session-based auth, OTP verification by email, and fitness/nutrition tracking features.

## Features

### Authentication
- Register and login with password hashing (`BCrypt`)
- OTP-based verification (`/Auth/TwoFactor`) for registration and login
- SMTP email delivery for OTP
- Session-based access control and protected routes

### User features
- Body weight logging and trend chart data
- Goal tracking with target timeline (weeks to deadline)
- Workout logging with duration and calories burned
- Nutrition logging with macro calculations (protein, carbs, fat)
- Mood and energy tracking with recommendations
- Personalized dashboard insights (streaks, calories, AI-style guidance)

### Trainer features
- View assigned clients
- Track client latest weight and workout activity
- Manage workout plans

### Admin features
- Manage users, foods, and workout plans
- View system-level dashboard stats
- Enforced naming rule for roles:
  - Admin names must end with `Admin`
  - Trainer names must include `Trainer`

## Tech stack

- ASP.NET Core Razor Pages (`net10.0`)
- Entity Framework Core (`SqlServer` provider)
- SQL Server (via `DefaultConnection`)
- `BCrypt.Net-Next` for password hashing
- Session + custom `BasePageModel` role checks
- Vanilla CSS/JS in `wwwroot/css/fittrack.css` and `wwwroot/js/fittrack.js`

## Project structure

- `Pages/Auth` - login, register, OTP, logout
- `Pages/User` - body weight, goals, workouts, nutrition, mood
- `Pages/Trainer` - trainer client views
- `Pages/Admin` - admin management pages
- `Pages/Dashboard` - role-aware dashboard
- `Data/AppDbContext.cs` - EF Core context + seed data
- `Models/Models.cs` - domain models
- `Services/SmtpEmailSender.cs` - email service
- `SQL_QUERIES.sql` - SQL schema + seed script for manual setup

## Getting started

### 1) Prerequisites
- .NET 10 SDK
- SQL Server / SQL Express
- SMTP account for OTP email

### 2) Configure settings
Update `appsettings.json`:
- `ConnectionStrings:DefaultConnection`
- `Smtp:*` values (`Host`, `Port`, `EnableSsl`, `Username`, `Password`, `FromEmail`, `FromName`)

> Security note: do not commit real SMTP credentials. Use user secrets or environment variables.

### 3) Create database
This project includes `SQL_QUERIES.sql` with full table creation and seed data.

Run the script in SSMS (or equivalent) against your target SQL Server database.

### 4) Run the app
```powershell
dotnet restore
dotnet run
```

Then open the app URL shown in terminal (typically `https://localhost:<port>`).

## Default seeded accounts

If you run the provided SQL seed/app seed data, typical test users include:
- Admin: `admin@fittrack.rw`
- Trainer: `trainer@fittrack.rw`
- Users: `alice@fittrack.rw`, `bob@fittrack.rw`

Use your own safe credentials in non-local environments.

## Notes

- Access to pages is enforced by session checks in `Program.cs` and role helpers in `BasePageModel`.
- Dashboard behavior changes by role (`Admin`, `Trainer`, `User`).
- Authentication pages are designed to use illustration/photo backgrounds per project UI preference.
