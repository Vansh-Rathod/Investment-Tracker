CREATE TABLE SIPs
(
    SipId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Sip_Id PRIMARY KEY,
    PortfolioId INT NOT NULL,
    AssetTypeId INT NOT NULL, -- 1 -> Stock, 2 -> Mututal Fund
    AssetId INT NOT NULL, -- StockId or FundId
    SipAmount DECIMAL(10,2) NOT NULL,
    Frequency INT NOT NULL DEFAULT (1), -- 1 -> Daily, 2 -> Weekly, 3 -> Monthly
    SipDate DATETIME NOT NULL,
    StartDate DATETIME NOT NULL DEFAULT (GETDATE()),
    EndDate  DATETIME NULL,
    Status INT NOT NULL DEFAULT (1), -- 1 -> Acive, 2 -> Paused, 3 -> Cancelled
    CreatedDate DATETIME NOT NULL DEFAULT (GETDATE()),
    ModifiedDate DATETIME NULL
);