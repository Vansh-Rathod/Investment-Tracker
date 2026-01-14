CREATE TABLE SipExecutions
(
    SipExecutionId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SipExecution_Id PRIMARY KEY,
    SipId INT NOT NULL,
    PortfolioId INT NOT NULL,
    AssetTypeId INT NOT NULL,
    AssetId INT NOT NULL,
    ScheduledDate DATETIME NULL,
    ExecutedDate DATETIME NOT NULL DEFAULT (GETDATE()),
    SipAmount DECIMAL(10,2) NOT NULL,
    NAVAtExecution DECIMAL(10,2) NOT NULL,
    UnitsAllocated DECIMAL(10,2) NOT NULL,
    OrderReference NVARCHAR(100) NULL,
    ExecutionStatus INT NOT NULL DEFAULT (1), -- SUCCESS / FAILED / PARTIAL
    FailureReason NVARCHAR(200) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT (GETDATE()),
    ModifiedDate DATETIME NULL
);