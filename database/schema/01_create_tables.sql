CREATE TABLE Ships (
    Code VARCHAR(20) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    FiscalYear CHAR(4) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CONSTRAINT PK_Ships PRIMARY KEY (Code),
    CONSTRAINT CK_Ships_Status CHECK (Status IN ('Active', 'Inactive')),
    CONSTRAINT CK_Ships_FiscalYear CHECK (LEN(FiscalYear) = 4 AND FiscalYear LIKE '[0-9][0-9][0-9][0-9]')
);

CREATE TABLE Ranks (
    RankId INT IDENTITY(1,1) NOT NULL,
    RankName NVARCHAR(100) NOT NULL,
    RankOrder INT NOT NULL,
    CONSTRAINT PK_Ranks PRIMARY KEY (RankId),
    CONSTRAINT UQ_Ranks_Name UNIQUE (RankName),
    CONSTRAINT UQ_Ranks_Order UNIQUE (RankOrder)
);

CREATE TABLE CrewMembers (
    CrewMemberId VARCHAR(20) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    Nationality NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_CrewMembers PRIMARY KEY (CrewMemberId)
);

CREATE TABLE CrewServiceHistory (
    ServiceHistoryId INT IDENTITY(1,1) NOT NULL,
    CrewMemberId VARCHAR(20) NOT NULL,
    ShipCode VARCHAR(20) NOT NULL,
    RankId INT NOT NULL,
    SignOnDate DATE NOT NULL,
    SignOffDate DATE NULL,
    EndOfContractDate DATE NOT NULL,
    CONSTRAINT PK_CrewServiceHistory PRIMARY KEY (ServiceHistoryId),
    CONSTRAINT FK_CrewServiceHistory_CrewMember FOREIGN KEY (CrewMemberId) REFERENCES CrewMembers(CrewMemberId),
    CONSTRAINT FK_CrewServiceHistory_Ship FOREIGN KEY (ShipCode) REFERENCES Ships(Code),
    CONSTRAINT FK_CrewServiceHistory_Rank FOREIGN KEY (RankId) REFERENCES Ranks(RankId),
    CONSTRAINT CK_CrewServiceHistory_Dates CHECK (SignOnDate <= EndOfContractDate),
    CONSTRAINT CK_CrewServiceHistory_SignOff CHECK (SignOffDate IS NULL OR SignOffDate >= SignOnDate)
);

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Role NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (UserId)
);

CREATE TABLE UserShipAssignments (
    UserId INT NOT NULL,
    ShipCode VARCHAR(20) NOT NULL,
    AssignedDate DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_UserShipAssignments PRIMARY KEY (UserId, ShipCode),
    CONSTRAINT FK_UserShipAssignments_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserShipAssignments_Ship FOREIGN KEY (ShipCode) REFERENCES Ships(Code)
);

CREATE TABLE ChartOfAccounts (
    AccountNumber VARCHAR(20) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    ParentAccountNumber VARCHAR(20) NULL,
    AccountType VARCHAR(20) NOT NULL,
    CONSTRAINT PK_ChartOfAccounts PRIMARY KEY (AccountNumber),
    CONSTRAINT FK_ChartOfAccounts_Parent FOREIGN KEY (ParentAccountNumber) REFERENCES ChartOfAccounts(AccountNumber),
    CONSTRAINT CK_ChartOfAccounts_Type CHECK (AccountType IN ('Parent', 'Child'))
);

CREATE TABLE BudgetData (
    BudgetId INT IDENTITY(1,1) NOT NULL,
    ShipCode VARCHAR(20) NOT NULL,
    AccountNumber VARCHAR(20) NOT NULL,
    AccountPeriod DATE NOT NULL,
    BudgetValue DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_BudgetData PRIMARY KEY (BudgetId),
    CONSTRAINT FK_BudgetData_Ship FOREIGN KEY (ShipCode) REFERENCES Ships(Code),
    CONSTRAINT FK_BudgetData_Account FOREIGN KEY (AccountNumber) REFERENCES ChartOfAccounts(AccountNumber),
    CONSTRAINT CK_BudgetData_Value CHECK (BudgetValue >= 0)
);

CREATE TABLE AccountTransactions (
    TransactionId INT IDENTITY(1,1) NOT NULL,
    ShipCode VARCHAR(20) NOT NULL,
    AccountNumber VARCHAR(20) NOT NULL,
    AccountPeriod DATE NOT NULL,
    ActualValue DECIMAL(18,2) NOT NULL,
    TransactionDate DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_AccountTransactions PRIMARY KEY (TransactionId),
    CONSTRAINT FK_AccountTransactions_Ship FOREIGN KEY (ShipCode) REFERENCES Ships(Code),
    CONSTRAINT FK_AccountTransactions_Account FOREIGN KEY (AccountNumber) REFERENCES ChartOfAccounts(AccountNumber),
    CONSTRAINT CK_AccountTransactions_Value CHECK (ActualValue >= 0)
);

