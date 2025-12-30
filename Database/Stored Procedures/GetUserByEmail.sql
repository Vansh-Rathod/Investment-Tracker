CREATE PROCEDURE GetUserByEmail
@Email NVARCHAR(200),
@IsActive BIT = 1
AS

--DECLARE
--@Email NVARCHAR(200) = 'therv429@gmail.com',
--@IsActive BIT = 1

BEGIN

SET NOCOUNT ON;

select Id,  
        Name,  
        Email,  
        PhoneNumber,  
        LastLogin,  
        CreatedDate,  
        ModifiedDate,  
        IsActive,  
        IsDeleted
        FROM Users with(nolock)
        where Email =@Email
        AND IsActive = @IsActive

END