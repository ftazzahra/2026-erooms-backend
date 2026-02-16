# E-Rooms Backend API

## Overview

E-Rooms is a RESTful backend service built with ASP.NET Core to handle room reservation and management processes.

This API provides secure authentication, user and role management, room administration, and booking operations. The system implements role-based access control (Admin and User) and is designed with scalability, maintainability, and security in mind.

---

## Key Features

- User account registration and authentication
- JWT-based authentication system
- Role-based access control (Admin & User)
- Secure password hashing with BCrypt
- Room management (Admin only)
- Booking management system
- User administration
- Interactive API documentation using Swagger
- SQL Server database integration with Entity Framework Core

---

## Technology Stack

- ASP.NET Core (.NET 10)
- Entity Framework Core (Code First)
- Microsoft SQL Server
- JSON Web Token (JWT)
- BCrypt.Net
- Swagger / OpenAPI

---

## Installation Guide

### 1. Clone the Repository

```bash
git clone https://github.com/ftazzahra/2026-erooms-backend.git
```

### 2. Navigate to the Project Directory

```bash
cd 2026-erooms-backend/erooms
```

### 3. Restore Project Dependencies

```bash
dotnet restore
```

### 4. Configure Database Connection

Ensure SQL Server is running on your machine.  
Then update the connection string inside:

```
appsettings.json
```

Example configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EroomsDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

After that, apply database migrations:

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

---

## How to Use

Once the application is running, open your browser and access:

```
https://localhost:5006/swagger
```

The Swagger UI allows you to test all available API endpoints, including:

- Account registration
- Login authentication
- Room creation and management
- Booking operations
- Administrative user management

---

## Environment Configuration

The following environment variables can be customized for production or deployment:

- `JWT__Key`
- `JWT__Issuer`
- `JWT__Audience`
- `ConnectionStrings__DefaultConnection`

JWT configuration example:

```json
"Jwt": {
  "Key": "your_secure_secret_key",
  "Issuer": "EroomsAPI",
  "Audience": "EroomsClient"
}
```

---

## Project Directory Structure

```
erooms/
│
├── Controllers/
├── Models/
├── Data/
├── DTOs/
├── Migrations/
├── Program.cs
├── appsettings.json
└── README.md
```

---

## License

This project is distributed under the MIT License.

---

## Author

Developed by  
**Zahra**

GitHub: https://github.com/ftazzahra
