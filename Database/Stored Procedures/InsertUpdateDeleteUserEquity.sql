CREATE PROCEDURE InsertUpdateDeleteUserEquity
(  
    @OperationType INT,
    @Id INT,
    @UserId INT,  
  
    @EquityName NVARCHAR(MAX) = NULL,  
    @EquityShortForm NVARCHAR(100) = NULL,
    @EquityType INT = NULL,
    @PurchasePrice DECIMAL(10,2) = NULL,  
    @Quantity DECIMAL(10,2) = NULL,  
    @InvestedAmount DECIMAL(10,2) = NULL,  
    @CurrentPrice DECIMAL(10,2) = NULL,
    @InvestmentDate DATETIME2 = NULL,
    @OrderId NVARCHAR(MAX) = NULL,  
    @CompanyName NVARCHAR(256) = NULL,
  
    @IsActive BIT = 1  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    -- =========================  
    -- INSERT USER EQUITY 
    -- =========================  
    IF @OperationType = 1  
    BEGIN  
        INSERT INTO UserEquities  
        (  
            UserId,  
            EquityName,  
            EquityShortForm,  
            EquityType,  
            PurchasePrice,  
            Quantity,  
            InvestedAmount,  
            CurrentPrice,
            InvestmentDate,
            OrderId,
            CreatedDate,
            IsActive,
            IsDeleted,
            CompanyName
        )  
        VALUES  
        (  
            @UserId,  
            @EquityName,  
            @EquityShortForm,  
            @EquityType,  
            @PurchasePrice,  
            @Quantity,  
            @InvestedAmount,
            @CurrentPrice,
            @InvestmentDate,
            @OrderId,
            GETUTCDATE(),
            @IsActive,
            0,
            @CompanyName
        );  
  
        SELECT SCOPE_IDENTITY() AS Id;  
        RETURN;  
    END  
  
    -- =========================  
    -- UPDATE USER EQUITY  
    -- =========================  
    IF @OperationType = 2  
    BEGIN  
        UPDATE UserEquities  
        SET  
            EquityName = ISNULL(@EquityName, EquityName), 
            EquityShortForm = ISNULL(@EquityShortForm, EquityShortForm),  
            PurchasePrice = ISNULL(@PurchasePrice, PurchasePrice),  
            Quantity = ISNULL(@Quantity, Quantity),  
            InvestedAmount = ISNULL(@InvestedAmount, InvestedAmount),  
            CurrentPrice = ISNULL(@CurrentPrice, CurrentPrice),
            InvestmentDate = ISNULL(@InvestmentDate, InvestmentDate),
            OrderId = ISNULL(@OrderId, OrderId),
            ModifiedDate = GETUTCDATE(),  
            IsActive = ISNULL(@IsActive, IsActive),  
            CompanyName = ISNULL(@CompanyName, CompanyName)
        WHERE
            Id = @Id
            AND UserId = @UserId  
            AND IsDeleted = 0;  
  
        SELECT @Id AS Id;  
        RETURN;  
    END  
  
    -- =========================  
    -- SOFT DELETE USER EQUITY 
    -- =========================  
    IF @OperationType = 3  
    BEGIN  
        UPDATE UserEquities  
        SET  
            IsDeleted = 1,  
            IsActive = 0,  
            ModifiedDate = GETUTCDATE()  
        WHERE  
            Id = @Id
            AND UserId = @UserId
            AND IsDeleted = 0;  
  
        SELECT @Id AS Id;  
        RETURN;  
    END  
END  