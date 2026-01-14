CREATE TABLE Portfolios
(
    PortfolioId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Portfolio_Id PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    UserId INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT (GETDATE()),
    ModifiedDate DATETIME NULL,
    
    CONSTRAINT FK_Portfolios_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);