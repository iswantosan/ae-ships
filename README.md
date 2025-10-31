# AE Ships - Backend Code Challenge

Ship Management System backend solution built with C# and SQL Server.

## Project Structure

```
ae-ships/
├── src/
│   ├── AE.Ships.Api/              - Web API layer
│   ├── AE.Ships.Application/      - Application/business logic layer
│   ├── AE.Ships.Domain/           - Domain models
│   └── AE.Ships.Infrastructure/   - Data access & external services
├── tests/
│   ├── AE.Ships.Api.Tests/        - API integration tests
│   └── AE.Ships.Application.Tests/ - Unit tests
└── database/                       - SQL Server database scripts
    ├── schema/                     - DDL scripts
    ├── data/                       - Sample data
    ├── stored-procedures/          - Stored procedures
    └── docs/                       - Database documentation
```

## Tech Stack

-   **Backend**: C# / .NET 9.0
-   **Database**: SQL Server (Express, Standard, or Azure SQL Database)
-   **Testing**: xUnit
-   **Architecture**: Clean Architecture / Onion Architecture
-   **Database Access**: Stored Procedures only

## Getting Started

### 1. Database Setup

See full instructions in [database/README.md](database/README.md)

Quick setup:

```bash
cd database
sqlcmd -S localhost -d master -i setup-master.sql
```

Or via SSMS:

1. Open SQL Server Management Studio
2. File → Open → File → select `database/setup-master.sql`
3. Execute (F5)

### 2. API Setup

#### Prerequisites
- .NET 9.0 SDK installed
- SQL Server running with ShipManagement database created
- Connection string configured in `appsettings.json`

#### Configuration

Update connection string in `src/AE.Ships.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ShipManagement;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

#### Run Locally

```bash
# Navigate to API project
cd src/AE.Ships.Api

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

API will be available at:
- HTTP: `http://localhost:5110`
- HTTPS: `https://localhost:7195`
- Swagger UI: `https://localhost:7195/swagger`

#### Using Docker

```bash
# Build and run using docker-compose
docker-compose up -d

# API will be available at http://localhost:8080
# Swagger UI at http://localhost:8080/swagger
```

### 3. Authentication

#### JWT Authentication

The API uses JWT Bearer authentication. To access protected endpoints:

1. **Login** to get a token:
   ```bash
   POST /api/auth/login
   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Use the token** in subsequent requests:
   ```
   Authorization: Bearer <your-token>
   ```

3. **Validate token**:
   ```bash
   POST /api/auth/validate
   {
     "token": "<your-token>"
   }
   ```

**Available test users:**
- Username: `admin`, Password: `admin123` (Role: Admin)
- Username: `manager`, Password: `manager123` (Role: Manager)
- Username: `user`, Password: `user123` (Role: User)

#### Swagger UI

Swagger UI includes JWT authentication support:
1. Click **Authorize** button at the top right
2. Enter: `Bearer <your-token>` (or just `<your-token>`)
3. Click **Authorize** to use the token in all requests

### 4. API Endpoints

#### Ships
- `GET /api/ships` - Get all ships
- `GET /api/ships/{code}` - Get ship by code
- `GET /api/ships/status/{status}` - Get ships by status
- `GET /api/ships/user/{userId}` - Get ships assigned to user
- `POST /api/ships` - Create new ship
- `PUT /api/ships/{code}` - Update ship
- `DELETE /api/ships/{code}` - Delete ship

#### Crew
- `GET /api/crew/{shipCode}` - Get crew list for ship (with pagination, sorting, search)
  - Query parameters: `pageNumber`, `pageSize`, `sortColumn`, `sortDirection`, `searchTerm`

#### Financial Reports
- `GET /api/financial-reports/{shipCode}` - Get financial report for ship
  - Query parameters: `accountPeriod` (e.g., "2025-07")

#### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

#### User-Ship Assignments
- `GET /api/user-ship-assignments/{userId}` - Get ship assignments for user
- `POST /api/user-ship-assignments/assign` - Assign ship to user
- `POST /api/user-ship-assignments/unassign` - Unassign ship from user

## Features

### Database Challenge ✅

-   [x] Database schema design with ERD
-   [x] DDL scripts for all entities
-   [x] Sample data with realistic scenarios
    -   [x] 6 ships (active/inactive, various fiscal years)
    -   [x] 125+ crew members
    -   [x] 18 rank types
    -   [x] 5 parent accounts, 35+ child accounts
    -   [x] Budget & transaction data for 2024-2025
-   [x] Stored procedures
    -   [x] Crew list with pagination, sorting, search
    -   [x] Financial reports with YTD calculations
    -   [x] Ship management
    -   [x] User assignments

### API Challenge ✅

-   [x] REST API endpoints
-   [x] Ship management
-   [x] Crew management
-   [x] Financial reporting
-   [x] User & ship assignments
-   [x] JWT Authentication
-   [x] Swagger UI with JWT support
-   [x] Unit tests (xUnit)
-   [x] Integration tests
-   [x] Error handling and validation
-   [x] Logging (action filters)

## Database Documentation

Complete database documentation:

-   [database/docs/ERD](database/docs/ERD) - Entity Relationship Diagram
-   [database/docs/DATABASE_DESIGN](database/docs/DATABASE_DESIGN) - Design documentation
-   [database/README.md](database/README.md) - Setup guide & stored procedures

## Key Business Rules

### Ship Management

-   Ships have fiscal year codes (e.g., "0112" for Jan-Dec, "0403" for Apr-Mar)
-   Status: Active or Inactive
-   Only active ships in operational queries

### Crew Management

-   Crew status calculated from dates:
    -   **Onboard**: Currently serving
    -   **Planned**: Future assignment
    -   **Relief Due**: Contract overdue > 30 days
    -   **Signed Off**: Contract completed
-   One rank per crew member per contract
-   Multiple contracts supported

### Financial Reporting

-   Fiscal year-aware YTD calculations
-   Hierarchical Chart of Accounts
-   Budgets & actuals at child account level
-   Parent accounts aggregate children
-   Zero (0) ≠ NULL in financial data

## Development

### Prerequisites

-   .NET 9.0 SDK
-   SQL Server (Express or higher)
-   Visual Studio 2022 / VS Code / Rider

### Build

```bash
dotnet build
```

### Test

```bash
dotnet test
```

### Run

```bash
cd src/AE.Ships.Api
dotnet run
```

### Docker

Build and run with Docker:

```bash
# Build image
docker build -t ae-ships-api .

# Run container
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="<your-connection-string>" ae-ships-api
```

Or use docker-compose (includes SQL Server):

```bash
docker-compose up -d
```

### Testing

Run all tests:

```bash
dotnet test
```

Run specific test project:

```bash
# Unit tests
dotnet test tests/AE.Ships.Application.Tests

# Integration tests
dotnet test tests/AE.Ships.Api.Tests
```

### CI/CD

GitHub Actions workflow is configured (`.github/workflows/ci.yml`) to:
- Build the solution on push/PR
- Run all tests
- Publish test results

Workflow triggers on:
- Push to `main`, `master`, or `develop` branches
- Pull requests to these branches

## Architecture & Design Decisions

### Clean Architecture

The solution follows Clean Architecture principles:
- **Domain**: Core business entities and interfaces (no dependencies)
- **Application**: Business logic and services
- **Infrastructure**: Data access (repositories using stored procedures only)
- **API**: Controllers, authentication, middleware

### Database Access

- **Stored Procedures Only**: All database access is via stored procedures to ensure:
  - SQL injection protection
  - Centralized business logic
  - Performance optimization
  - Security compliance

### Authentication

- **JWT Bearer**: Token-based authentication
- **Development Mode**: Auto-authentication for easier testing
- **Production Ready**: Proper token validation and expiration

### Error Handling

- Consistent error responses
- Validation using FluentValidation (where applicable)
- Exception handling with appropriate HTTP status codes
- Meaningful error messages

### Testing Strategy

- **Unit Tests**: Service layer with mocked repositories (xUnit + Moq)
- **Integration Tests**: API controllers with in-memory/test database (xUnit + WebApplicationFactory)
- **Coverage**: Core business logic and API endpoints

## Additional Features

### Logging

Action filter logging for:
- Request/response tracking
- Error logging
- Performance monitoring

### Swagger Documentation

- Full API documentation
- JWT authentication support
- Request/response examples
- Interactive API testing

### Docker Support

- Multi-stage Dockerfile for optimized image size
- Docker Compose for local development with SQL Server
- Environment variable configuration
