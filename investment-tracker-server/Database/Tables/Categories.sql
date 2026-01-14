CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CategoryType INT NOT NULL, -- Equity, Debt, Hybrid, Commodities
    CategoryName NVARCHAR(100) NOT NULL
);