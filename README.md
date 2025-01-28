# RESTful API with ASP.NET 8, JWT Authentication, and .NET Identity

This project is a complete and professional RESTful API built using ASP.NET 8 (.NET Core). It incorporates **JWT Authentication** and **.NET Identity** for robust security and user management.

## Features

- **ASP.NET 8 Core**: Leverage the latest features and performance improvements.
- **JWT Authentication**: Secure endpoints using JSON Web Tokens.
- **.NET Identity**: Built-in user management system for authentication and authorization.
- **Entity Framework Core**: Database integration with seamless migrations.
- **Dependency Injection**: Modular and testable architecture.
- **CRUD Operations**: Implement Create, Read, Update, Delete for your data models.
- **Professional Practices**: Adheres to clean code and industry standards.

## Prerequisites

Make sure you have the following installed:

- [Visual Studio](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/)
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Postman](https://www.postman.com/) or any API testing tool (optional)
- Docker (if running SQL Server in a container)

Here’s the Configure the Database section as a single block of code in markdown:

## Configure the Database

1. Update the `appsettings.json` file with your database connection string:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=ApiMovies;User Id=sa;Password=YourPassword;Encrypt=False;TrustServerCertificate=True;"
   }

2. Apply migrations to set up the database:
    ```json
    dotnet ef migrations add <MigrationName>
    dotnet ef database update

## Endpoints

### Authentication
- `POST /api/auth/login` - Authenticate a user and return a JWT token.
- `POST /api/auth/register` - Register a new user.

### CRUD Operations
- Example for a resource like `Categories`:
   - `GET /api/categories` - Retrieve all categories.
   - `GET /api/categories/{id}` - Retrieve a specific category.
   - `POST /api/categories` - Create a new category.
   - `PUT /api/categories/{id}` - Update an existing category.
   - `DELETE /api/categories/{id}` - Delete a category.

## Tools & Libraries Used

- **ASP.NET Core 8**: Backend framework.
- **Entity Framework Core**: ORM for database operations.
- **JWT Authentication**: Secure token-based authentication.
- **.NET Identity**: User authentication and role management.
- **Swagger**: API documentation and testing.

## License

This project is licensed under no License.


