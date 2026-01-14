CREATE TABLE Transactions
(
    TransactionId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Transaction_Id PRIMARY KEY,
    PortfolioId INT NOT NULL,
    AssetTypeId INT NOT NULL,
    AssetId INT NOT NULL,
    TransactionType INT NOT NULL, -- BUY / SELL / DIVIDEND / SPLIT / BONUS
    Units DECIMAL(10,2) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    TransactionDate DATETIME NOT NULL DEFAULT (GETDATE()),
    SourceType NVARCHAR(200) NULL,
    SourceId INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT (GETDATE()),
    ModifiedDate DATETIME NULL
);