CREATE PROCEDURE GetUserEquities
(
    @UserId INT = 0,
    @Page INT = 1,
    @PageSize INT = 10,
    @SearchText NVARCHAR(200) = '',
    @SortOrder NVARCHAR(4) = 'DESC',
    @SortField NVARCHAR(100) = 'CreatedDate',
    @IsActive BIT = 1,
    @IsDeleted BIT = 0,
    @EquityType NVARCHAR(200) = NULL,
    @FromDate DATETIME2 = NULL,
    @ToDate DATETIME2 = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    --------------------------------------------------------------------
    -- 1. Validate SortOrder
    --------------------------------------------------------------------
    SET @SortOrder = UPPER(@SortOrder);
    IF @SortOrder NOT IN ('ASC', 'DESC')
        SET @SortOrder = 'DESC';

    --------------------------------------------------------------------
    -- 2. Validate SortField: WHITELIST (VERY IMPORTANT)
    --------------------------------------------------------------------
    IF UPPER(@SortField) NOT IN (
        'ID','USERID','EQUITYNAME','EQUITYSHORTFORM','EQUITYTYPE',
        'PURCHASEPRICE','QUANTITY','INVESTEDAMOUNT','CURRENTPRICE',
        'INVESTMENTDATE','ORDERID','CREATEDDATE','MODIFIEDDATE'
    )
        SET @SortField = 'CreatedDate';

    --------------------------------------------------------------------
    -- 3. Pagination
    --------------------------------------------------------------------
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    --------------------------------------------------------------------
    -- 4. Base SQL
    --------------------------------------------------------------------
    DECLARE @SQL NVARCHAR(MAX) = '
        SELECT
            ue.Id,
            ue.UserId,
            us.Name AS UserName,
            us.Email AS UserEmail,
            ue.EquityName,
            ue.EquityShortForm,
            ue.EquityType,
            ue.PurchasePrice,
            ue.Quantity,
            ue.InvestedAmount,
            ue.CurrentPrice,
            ue.InvestmentDate,
            ue.OrderId,
            ue.CreatedDate,
            ue.ModifiedDate,
            ue.IsActive,
            ue.IsDeleted
        FROM UserEquities ue WITH (NOLOCK)
        INNER JOIN Users us WITH (NOLOCK) ON ue.UserId = us.Id
        WHERE 1 = 1
    ';

    --------------------------------------------------------------------
    -- 5. Append Static Filter Conditions
    --------------------------------------------------------------------
    IF @UserId <> 0
        SET @SQL += ' AND ue.UserId = @UserId ';

    SET @SQL += ' AND ue.IsActive = @IsActive AND ue.IsDeleted = @IsDeleted ';

    IF ISNULL(@EquityType,'') <> ''
        SET @SQL += ' AND ue.EquityType = @EquityType ';

    IF ISNULL(@FromDate,'') <> ''
        SET @SQL += ' AND ue.InvestmentDate >= @FromDate ';

    IF ISNULL(@ToDate,'') <> ''
        SET @SQL += ' AND ue.InvestmentDate <= @ToDate ';

    --------------------------------------------------------------------
    -- 6. Search Condition
    --------------------------------------------------------------------
    SET @SQL += '
        AND (
            ue.EquityName LIKE ''%'' + @SearchText + ''%''
            OR ue.EquityShortForm LIKE ''%'' + @SearchText + ''%''
            OR us.Name LIKE ''%'' + @SearchText + ''%''
            OR us.Email LIKE ''%'' + @SearchText + ''%''
        )
    ';

    --------------------------------------------------------------------
    -- 7. Order By (Safe — QUOTENAME)
    --------------------------------------------------------------------
    SET @SQL += '
        ORDER BY ' + QUOTENAME(@SortField) + ' ' + @SortOrder + '
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    ';

    --------------------------------------------------------------------
    -- 8. Execute Securely
    --------------------------------------------------------------------
    EXEC sp_executesql 
        @SQL,
        N'@UserId INT, @Page INT, @PageSize INT, @Offset INT,
          @SearchText NVARCHAR(200), @SortOrder NVARCHAR(4), @SortField NVARCHAR(100),
          @IsActive BIT, @IsDeleted BIT, @EquityType NVARCHAR(200),
          @FromDate DATETIME2, @ToDate DATETIME2',
        @UserId, @Page, @PageSize, @Offset,
        @SearchText, @SortOrder, @SortField,
        @IsActive, @IsDeleted, @EquityType,
        @FromDate, @ToDate;
END
GO
