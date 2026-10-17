# Seema&apos;s Cloud Talk – Backend

Seema&apos;s Cloud Talk is a CRM and Project Management System backend built with **ASP.NET Core (.NET 10)** and **PostgreSQL**. It provides secure APIs for managing users, teams, clients, projects, roles, and permissions.

## Tech Stack

- ASP.NET Core (.NET 10)
- Entity Framework Core
- PostgreSQL (Neon Database)
- JWT Authentication & Authorization
- SignalR for real-time user presence
- Scalar / OpenAPI for API documentation

## Key Features

- User authentication and role-based access control
- User, team, and client management
- Project management and assignment
- Role and permission management
- Real-time online user tracking
- Dashboard reports and analytics
- RESTful API architecture

## AI/ML Integration

- AI/ML-assisted reporting and analytics to support report generation and data-driven insights.
- Helps analyze project and performance data for reporting purposes.

## Getting Started

1. Clone the repository.
2. Configure the database connection and JWT settings in `appsettings.json` or User Secrets.
3. Apply Entity Framework Core migrations.
4. Run the application.

```bash
dotnet run
```

## API Documentation

API documentation is available through Scalar/OpenAPI when the application is running.

**Frontend Repository:** [GitHub Repo](https://github.com/Rahi-007/CRM-Frontend)

**Database:** PostgreSQL hosted on Neon.

**Note:** Keep database credentials and JWT secrets private. Do not commit sensitive configuration to GitHub.