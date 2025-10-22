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

Coming soon...

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

### API Challenge

-   [ ] REST API endpoints
-   [ ] Ship management
-   [ ] Crew management
-   [ ] Financial reporting
-   [ ] User & ship assignments
-   [ ] Unit tests (xUnit)
-   [ ] Integration tests

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
