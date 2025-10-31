-- =============================================
-- Ship Management System - Master Setup Script
-- =============================================
-- This script sets up the complete database schema, sample data, and stored procedures
-- Execute this script on a fresh database to initialize the system
--
-- Execution Order:
-- 1. Schema (tables, constraints, indexes)
-- 2. Functions
-- 3. Sample Data (ships, crew, accounts, financial data)
-- 4. Stored Procedures
-- =============================================

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ShipManagement')
BEGIN
    CREATE DATABASE ShipManagement;
    PRINT 'Database ShipManagement created.';
END
ELSE
BEGIN
    PRINT 'Database ShipManagement already exists.';
END
GO

USE ShipManagement;
GO

PRINT '========================================';
PRINT 'Starting Ship Management System Setup';
PRINT '========================================';
PRINT '';

-- =============================================
-- STEP 1: CREATE SCHEMA (TABLES)
-- =============================================
PRINT 'STEP 1: Creating database schema (tables)...';
PRINT '';

:r schema\01_create_tables.sql
GO

-- =============================================
-- STEP 2: CREATE INDEXES
-- =============================================
PRINT 'STEP 2: Creating indexes...';
PRINT '';

:r schema\02_create_indexes.sql
GO

-- =============================================
-- STEP 3: CREATE FUNCTIONS
-- =============================================
PRINT 'STEP 3: Creating functions...';
PRINT '';

:r functions\fn_GetFiscalYearStartDate.sql
GO

:r functions\fn_CalculateCrewStatus.sql
GO

-- =============================================
-- STEP 4: INSERT SAMPLE DATA
-- =============================================
PRINT 'STEP 4: Inserting sample data...';
PRINT '';

:r data\01_insert_ships_and_ranks.sql
GO

:r data\02_insert_crew_members.sql
GO

:r data\03_insert_crew_service_history.sql
GO

:r data\04_insert_chart_of_accounts.sql
GO

:r data\05_insert_budget_data.sql
GO

:r data\06_insert_account_transactions.sql
GO

:r data\07_insert_users_and_assignments.sql
GO

-- =============================================
-- STEP 5: CREATE STORED PROCEDURES
-- =============================================
PRINT 'STEP 5: Creating stored procedures...';
PRINT '';

-- User Management SPs
:r stored-procedures\sp_CreateUser.sql
GO
:r stored-procedures\sp_GetAllUsers.sql
GO
:r stored-procedures\sp_GetUserById.sql
GO
:r stored-procedures\sp_UpdateUser.sql
GO
:r stored-procedures\sp_DeleteUser.sql
GO
:r stored-procedures\sp_UserExists.sql
GO

-- Ship Management SPs
:r stored-procedures\sp_CreateShip.sql
GO
:r stored-procedures\sp_GetAllShips.sql
GO
:r stored-procedures\sp_GetShips.sql
GO
:r stored-procedures\sp_GetShipByCode.sql
GO
:r stored-procedures\sp_GetShipsByStatus.sql
GO
:r stored-procedures\sp_UpdateShip.sql
GO
:r stored-procedures\sp_DeleteShip.sql
GO
:r stored-procedures\sp_ShipExists.sql
GO

-- Crew Management SPs
:r stored-procedures\sp_GetCrewList.sql
GO
:r stored-procedures\sp_GetCrewMemberHistory.sql
GO

-- Financial Report SPs
:r stored-procedures\sp_GetFinancialReport.sql
GO

-- User-Ship Assignment SPs
:r stored-procedures\sp_GetShipsByUser.sql
GO
:r stored-procedures\sp_AssignShipToUser.sql
GO
:r stored-procedures\sp_UnassignShipFromUser.sql
GO
:r stored-procedures\sp_GetUserShipAssignments.sql
GO

-- =============================================
-- VERIFICATION
-- =============================================
PRINT '========================================';
PRINT 'Setup Complete! Running Verification...';
PRINT '========================================';
PRINT '';

PRINT 'Record Counts:';
SELECT 'Ships' AS TableName, COUNT(*) AS RecordCount FROM Ships
UNION ALL
SELECT 'Ranks', COUNT(*) FROM Ranks
UNION ALL
SELECT 'CrewMembers', COUNT(*) FROM CrewMembers
UNION ALL
SELECT 'CrewServiceHistory', COUNT(*) FROM CrewServiceHistory
UNION ALL
SELECT 'Users', COUNT(*) FROM Users
UNION ALL
SELECT 'UserShipAssignments', COUNT(*) FROM UserShipAssignments
UNION ALL
SELECT 'ChartOfAccounts', COUNT(*) FROM ChartOfAccounts
UNION ALL
SELECT 'BudgetData', COUNT(*) FROM BudgetData
UNION ALL
SELECT 'AccountTransactions', COUNT(*) FROM AccountTransactions;

PRINT '';
PRINT 'Stored Procedures:';
SELECT name AS StoredProcedure, create_date AS CreatedDate
FROM sys.procedures
WHERE schema_id = SCHEMA_ID('dbo')
ORDER BY name;

PRINT '';
PRINT 'Functions:';
SELECT name AS FunctionName, create_date AS CreatedDate
FROM sys.objects
WHERE type = 'FN' AND schema_id = SCHEMA_ID('dbo')
ORDER BY name;

PRINT '';
PRINT '========================================';
PRINT 'Ship Management System Setup Complete!';
PRINT '========================================';
GO
