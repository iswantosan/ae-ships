@echo off
REM =============================================
REM Ship Management System - Database Setup (using osql)
REM =============================================

setlocal enabledelayedexpansion

set SERVER=localhost\MSSQLSERVER01
set DATABASE=ShipManagement
set USERNAME=sa
set PASSWORD=

REM Check if password is provided
if "%1"=="" (
    echo Usage: setup-database-osql.bat ^<password^>
    echo   OR: setup-database-osql.bat integrated
    echo.
    echo Examples:
    echo   setup-database-osql.bat YourPassword
    echo   setup-database-osql.bat integrated
    pause
    exit /b 1
)

if /i "%1"=="integrated" (
    set AUTH=-E
    set USERNAME=
    set PASSWORD=
    echo Using Integrated Security (Windows Authentication)
) else (
    set AUTH=-U %USERNAME% -P %1
    set PASSWORD=%1
    echo Using SQL Authentication with user: %USERNAME%
)

echo.
echo =========================================
echo Ship Management System - Setup (osql)
echo =========================================
echo Server: %SERVER%
echo Database: %DATABASE%
echo.

REM Change to script directory
cd /d "%~dp0"

echo Step 1: Creating database (if not exists)...
if /i "%1"=="integrated" (
    osql -S %SERVER% -E -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '%DATABASE%') CREATE DATABASE [%DATABASE%]"
) else (
    osql -S %SERVER% -U %USERNAME% -P %PASSWORD% -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '%DATABASE%') CREATE DATABASE [%DATABASE%]"
)

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to create/verify database
    pause
    exit /b 1
)

echo.
echo Step 2: Creating schema (tables)...
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -i schema\01_create_tables.sql
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i schema\01_create_tables.sql
)
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to create tables
    pause
    exit /b 1
)

echo Step 3: Creating indexes...
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -i schema\02_create_indexes.sql
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i schema\02_create_indexes.sql
)
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to create indexes
    pause
    exit /b 1
)

echo Step 4: Creating functions...
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -i functions\fn_GetFiscalYearStartDate.sql
    osql -S %SERVER% -d %DATABASE% -E -i functions\fn_CalculateCrewStatus.sql
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i functions\fn_GetFiscalYearStartDate.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i functions\fn_CalculateCrewStatus.sql
)
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to create functions
    pause
    exit /b 1
)

echo Step 5: Inserting sample data...
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -i data\01_insert_ships_and_ranks.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\02_insert_crew_members.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\03_insert_crew_service_history.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\04_insert_chart_of_accounts.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\05_insert_budget_data.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\06_insert_account_transactions.sql
    osql -S %SERVER% -d %DATABASE% -E -i data\07_insert_users_and_assignments.sql
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\01_insert_ships_and_ranks.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\02_insert_crew_members.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\03_insert_crew_service_history.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\04_insert_chart_of_accounts.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\05_insert_budget_data.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\06_insert_account_transactions.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i data\07_insert_users_and_assignments.sql
)

echo Step 6: Creating stored procedures...
REM User Management SPs
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_CreateUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetAllUsers.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetUserById.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_UpdateUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_DeleteUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_UserExists.sql
    REM Ship Management SPs
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_CreateShip.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetAllShips.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetShips.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetShipByCode.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetShipsByStatus.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_UpdateShip.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_DeleteShip.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_ShipExists.sql
    REM Crew Management SPs
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetCrewList.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetCrewMemberHistory.sql
    REM Financial Report SPs
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetFinancialReport.sql
    REM User-Ship Assignment SPs
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetShipsByUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_AssignShipToUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_UnassignShipFromUser.sql
    osql -S %SERVER% -d %DATABASE% -E -i stored-procedures\sp_GetUserShipAssignments.sql
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_CreateUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetAllUsers.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetUserById.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_UpdateUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_DeleteUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_UserExists.sql
    REM Ship Management SPs
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_CreateShip.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetAllShips.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetShips.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetShipByCode.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetShipsByStatus.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_UpdateShip.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_DeleteShip.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_ShipExists.sql
    REM Crew Management SPs
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetCrewList.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetCrewMemberHistory.sql
    REM Financial Report SPs
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetFinancialReport.sql
    REM User-Ship Assignment SPs
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetShipsByUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_AssignShipToUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_UnassignShipFromUser.sql
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -i stored-procedures\sp_GetUserShipAssignments.sql
)

echo.
echo =========================================
echo Setup Complete!
echo =========================================
echo.
echo Running verification...
if /i "%1"=="integrated" (
    osql -S %SERVER% -d %DATABASE% -E -Q "SELECT 'Ships' AS TableName, COUNT(*) AS RecordCount FROM Ships UNION ALL SELECT 'Ranks', COUNT(*) FROM Ranks UNION ALL SELECT 'CrewMembers', COUNT(*) FROM CrewMembers UNION ALL SELECT 'CrewServiceHistory', COUNT(*) FROM CrewServiceHistory UNION ALL SELECT 'Users', COUNT(*) FROM Users UNION ALL SELECT 'UserShipAssignments', COUNT(*) FROM UserShipAssignments UNION ALL SELECT 'ChartOfAccounts', COUNT(*) FROM ChartOfAccounts UNION ALL SELECT 'BudgetData', COUNT(*) FROM BudgetData UNION ALL SELECT 'AccountTransactions', COUNT(*) FROM AccountTransactions"
) else (
    osql -S %SERVER% -d %DATABASE% -U %USERNAME% -P %PASSWORD% -Q "SELECT 'Ships' AS TableName, COUNT(*) AS RecordCount FROM Ships UNION ALL SELECT 'Ranks', COUNT(*) FROM Ranks UNION ALL SELECT 'CrewMembers', COUNT(*) FROM CrewMembers UNION ALL SELECT 'CrewServiceHistory', COUNT(*) FROM CrewServiceHistory UNION ALL SELECT 'Users', COUNT(*) FROM Users UNION ALL SELECT 'UserShipAssignments', COUNT(*) FROM UserShipAssignments UNION ALL SELECT 'ChartOfAccounts', COUNT(*) FROM ChartOfAccounts UNION ALL SELECT 'BudgetData', COUNT(*) FROM BudgetData UNION ALL SELECT 'AccountTransactions', COUNT(*) FROM AccountTransactions"
)

echo.
pause

