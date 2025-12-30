ALTER PROCEDURE GetUsers
(
    @UserId INT,
    @IsActive BIT = 1,
    @IsDeleted BIT = 0,
    @Page INT = 1,
    @PageSize INT = 10,
    @SearchText NVARCHAR(200) = '',
    @SortOrder NVARCHAR(50) = 'DESC',      -- ASC/DESC
    @SortField NVARCHAR(100) = 'CreatedDate'
)
AS

--DECLARE
--@UserId INT = 0,
--@IsActive BIT = 1,
--@IsDeleted BIT = 0,
--@Page INT = 1,
--@PageSize INT = 10,
--@SearchText NVARCHAR(200) = '',
--@SortOrder NVARCHAR(50) = 'DESC',
--@SortField NVARCHAR(100) = 'CreatedDate'

BEGIN
    SET NOCOUNT ON;

    ---------------------------------------------------------
    -- 1. Validate SortOrder
    ---------------------------------------------------------
    IF UPPER(@SortOrder) NOT IN ('ASC', 'DESC')
        SET @SortOrder = 'DESC';

    ---------------------------------------------------------
    -- 2. Validate SortField against allowable columns
    ---------------------------------------------------------
    IF UPPER(@SortField) NOT IN (
        'ID', 'NAME', 'EMAIL', 'PHONENUMBER', 
        'LASTLOGIN', 'CREATEDDATE', 'MODIFIEDDATE'
    )
        SET @SortField = 'CreatedDate';

    ---------------------------------------------------------
    -- 3. Pagination calculations
    ---------------------------------------------------------
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    ---------------------------------------------------------
    -- 4. Dynamic SQL for sorting
    ---------------------------------------------------------
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = '
    SELECT 
        Id,
        Name,
        Email,
        PhoneNumber,
        LastLogin,
        CreatedDate,
        ModifiedDate,
        IsActive,
        IsDeleted
    FROM Users WITH (NOLOCK)
    WHERE 
        (Id = @UserId OR @UserId = 0)
        AND (IsActive = @IsActive)
        AND (IsDeleted = @IsDeleted)
        AND (
            Name LIKE ''%'' + @SearchText + ''%''
            OR Email LIKE ''%'' + @SearchText + ''%''
        )
    ORDER BY ' + QUOTENAME(@SortField) + ' ' + @SortOrder + '
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    ';

    ---------------------------------------------------------
    -- 5. Execute
    ---------------------------------------------------------
    EXEC sp_executesql 
        @SQL,
        N'@UserId INT, @IsActive BIT, @IsDeleted BIT, @SearchText NVARCHAR(200), @Offset INT, @PageSize INT',
        @UserId, @IsActive, @IsDeleted, @SearchText, @Offset, @PageSize;
END
GO
