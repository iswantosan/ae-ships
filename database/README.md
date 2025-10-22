# Ship Management System - Database

Database schema, sample data, and stored procedures for Ship Management System.

## Folder Structure

```
database/
├── setup-master.sql           - Master setup script (run this for complete setup)
├── schema/                    - DDL scripts (CREATE TABLE, indexes, constraints)
│   ├── 01_create_tables.sql
│   └── 02_create_indexes.sql
├── data/                      - Sample data INSERT scripts
│   ├── 01_insert_ships_and_ranks.sql
│   ├── 02_insert_crew_members.sql
│   ├── 03_insert_crew_service_history.sql
│   ├── 04_insert_chart_of_accounts.sql
│   ├── 05_insert_budget_data.sql
│   ├── 06_insert_account_transactions.sql
│   └── 07_insert_users_and_assignments.sql
├── stored-procedures/         - Stored procedures for API
│   ├── sp_GetCrewList.sql
│   ├── sp_GetFinancialReport.sql
│   ├── sp_GetShips.sql
│   ├── sp_GetShipsByUser.sql
│   └── sp_GetCrewMemberHistory.sql
└── docs/                      - ERD and database documentation
    ├── ERD
    └── DATABASE_DESIGN
```

## Quick Start

Run a single script for complete setup:

```bash
sqlcmd -S localhost -d master -i setup-master.sql
```
