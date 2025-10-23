-- =============================================
-- Ship Management System - Master Setup Script
-- =============================================
-- This script sets up the complete database schema, sample data, and stored procedures
-- Execute this script on a fresh database to initialize the system
--
-- Execution Order:
-- 1. Schema (tables, constraints, indexes)
-- 2. Sample Data (ships, crew, accounts, financial data)
-- 3. Stored Procedures
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
-- STEP 1: CREATE SCHEMA
-- =============================================
PRINT 'STEP 1: Creating database schema...';
PRINT '';

-- Note: For script file references (:r), ensure paths are relative to this script's location
-- If running from SSMS, you may need to adjust paths or run scripts individually

PRINT 'Execute the following scripts in order:';
PRINT '1. schema/01_create_tables.sql';
PRINT '2. schema/02_create_indexes.sql';
PRINT '3. data/01_insert_ships_and_ranks.sql';
PRINT '4. data/02_insert_crew_members.sql';
PRINT '5. data/03_insert_crew_service_history.sql';
PRINT '6. data/04_insert_chart_of_accounts.sql';
PRINT '7. data/05_insert_budget_data.sql';
PRINT '8. data/06_insert_account_transactions.sql';
PRINT '9. data/07_insert_users_and_assignments.sql';
PRINT '10. stored-procedures/sp_GetCrewList.sql';
PRINT '11. stored-procedures/sp_GetFinancialReport.sql';
PRINT '12. stored-procedures/sp_GetShips.sql';
PRINT '13. stored-procedures/sp_GetShipsByUser.sql';
PRINT '14. stored-procedures/sp_GetCrewMemberHistory.sql';
PRINT '15. stored-procedures/sp_AssignShipToUser.sql';
PRINT '16. stored-procedures/sp_UnassignShipFromUser.sql';
PRINT '17. stored-procedures/sp_GetUserShipAssignments.sql';
PRINT '';
PRINT 'Alternatively, run each script individually in SSMS.';
PRINT '';

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
WHERE name LIKE 'sp_Get%'
ORDER BY name;

PRINT '';
PRINT '========================================';
PRINT 'Ship Management System Setup Complete!';
PRINT '========================================';
GO

