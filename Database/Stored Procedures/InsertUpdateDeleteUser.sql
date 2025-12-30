CREATE PROCEDURE InsertUpdateDeleteUser
(
    @OperationType INT,
    @UserId INT,

    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @Password NVARCHAR(256) = NULL,
    @PasswordHash VARBINARY(MAX) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,

    @IsActive BIT = 1
)
AS
BEGIN
    SET NOCOUNT ON;

    -- =========================
    -- INSERT USER
    -- =========================
    IF @OperationType = 1
    BEGIN
        INSERT INTO Users
        (
            Name,
            Email,
            Password,
            PasswordHash,
            PhoneNumber,
            CreatedDate,
            IsActive,
            IsDeleted
        )
        VALUES
        (
            @Name,
            @Email,
            @Password,
            @PasswordHash,
            @PhoneNumber,
            GETUTCDATE(),
            @IsActive,
            0
        );

        SELECT SCOPE_IDENTITY() AS UserId;
        RETURN;
    END

    -- =========================
    -- UPDATE USER
    -- =========================
    IF @OperationType = 2
    BEGIN
        UPDATE Users
        SET
            Name = ISNULL(@Name, Name),
            Email = ISNULL(@Email, Email),
            Password = ISNULL(@Password, Password),
            PasswordHash = ISNULL(@PasswordHash, PasswordHash),
            PhoneNumber = ISNULL(@PhoneNumber, PhoneNumber),
            IsActive = ISNULL(@IsActive, IsActive),
            ModifiedDate = GETUTCDATE()
        WHERE
            Id = @UserId
            AND IsDeleted = 0;

        SELECT @UserId AS UserId;
        RETURN;
    END

    -- =========================
    -- SOFT DELETE USER
    -- =========================
    IF @OperationType = 3
    BEGIN
        UPDATE Users
        SET
            IsDeleted = 1,
            IsActive = 0,
            ModifiedDate = GETUTCDATE()
        WHERE
            Id = @UserId
            AND IsDeleted = 0;

        SELECT @UserId AS UserId;
        RETURN;
    END
END
