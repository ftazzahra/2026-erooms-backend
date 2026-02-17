# Changelog

All notable changes to this project are documented in this file.

The format follows Keep a Changelog principles and Semantic Versioning.

---
## [1.0.0] - 2026-02-16



### Added
- **Backend setup with ASP.NET Core (.NET 10)** – Base structure for the backend.  
- **Entity Framework Core with SQL Server** – Database connection and ORM for data operations.  
- **JWT-based authentication** – Secure login and token generation.  
- **Role-based authorization (Admin & User)** – Access control for endpoints based on roles.  
- **Password hashing with BCrypt** – Secure storage of user passwords.  
- **Room management endpoints (CRUD for Admin)** – Endpoints to create, read, update, and delete rooms.  
- **Booking management endpoints (CRUD)** – Endpoints for creating and managing bookings.  
- **User management endpoints** – Endpoints to manage user accounts and data.  
- **Swagger API documentation** – Interactive documentation for testing APIs.  
- **DTOs for request and response** – Structured data transfer objects for requests and responses.  
- **Profile management endpoints (CRUD)** – Endpoints to view and update user profiles.  

### Improvements and Corrections
- **JWT Claims** – Include role, userId, and email in tokens to support role-based access and user identification.  
- **DTO Mapping** – Room and Booking endpoints return all required fields through DTOs for reliable API responses.  
- **Profile Password Handling** – Hash passwords automatically when updated through the profile endpoint to maintain security.  
- **Database Seeding & Roles** – Create initial admin, roles, and rooms during setup; assign default role to new users.  
- **Git Ignore Configuration** – `.gitignore` updated to exclude unnecessary files and folders for a cleaner repository.  

