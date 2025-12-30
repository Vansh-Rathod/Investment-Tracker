CREATE TABLE UserEquities
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_UserEquities_Id PRIMARY KEY,
    UserId INT NOT NULL CONSTRAINT FK_UserEquities_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id), 
    EquityName NVARCHAR(MAX) NOT NULL,
    EquityShortForm NVARCHAR(50) NULL,
    EquityType NVARCHAR(200) NOT NULL,
    PurchasePrice DECIMAL(10,2) NOT NULL,
    Quantity DECIMAL(10,2) NOT NULL,
    InvestedAmount DECIMAL(10,2) NOT NULL,
    CurrentPrice DECIMAL(10,2) NOT NULL,
    InvestmentDate DATETIME2(7) NOT NULL,
    OrderId NVARCHAR(200) NULL,
    CreatedDate DATETIME2(7) NOT NULL CONSTRAINT DF_UserEquities_CreatedDate DEFAULT (SYSUTCDATETIME()),
    ModifiedDate DATETIME2(7) NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_UserEquities_IsActive DEFAULT (1),
    IsDeleted BIT NOT NULL CONSTRAINT DF_UserEquities_IsDeleted DEFAULT (0),
    CONSTRAINT UQ_UserEquities_OrderId UNIQUE (OrderId)
);