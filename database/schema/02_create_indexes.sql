CREATE INDEX IX_CrewServiceHistory_ShipCode ON CrewServiceHistory(ShipCode);
CREATE INDEX IX_CrewServiceHistory_CrewMemberId ON CrewServiceHistory(CrewMemberId);
CREATE INDEX IX_CrewServiceHistory_Dates ON CrewServiceHistory(SignOnDate, SignOffDate, EndOfContractDate);
CREATE INDEX IX_CrewServiceHistory_RankId ON CrewServiceHistory(RankId);

CREATE INDEX IX_BudgetData_ShipCode_Period ON BudgetData(ShipCode, AccountPeriod);
CREATE INDEX IX_BudgetData_AccountNumber ON BudgetData(AccountNumber);

CREATE INDEX IX_AccountTransactions_ShipCode_Period ON AccountTransactions(ShipCode, AccountPeriod);
CREATE INDEX IX_AccountTransactions_AccountNumber ON AccountTransactions(AccountNumber);

CREATE INDEX IX_ChartOfAccounts_Parent ON ChartOfAccounts(ParentAccountNumber);

CREATE INDEX IX_UserShipAssignments_ShipCode ON UserShipAssignments(ShipCode);

