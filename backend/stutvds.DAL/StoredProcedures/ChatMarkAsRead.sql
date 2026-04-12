CREATE OR ALTER PROCEDURE dbo.ChatMarkAsRead
    @MyUserId NVARCHAR(450),
    @OtherUserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

UPDATE ChatMessages
SET ReadAt = SYSUTCDATETIME()
WHERE SenderId = @OtherUserId
  AND ReceiverId = @MyUserId
  AND ReadAt IS NULL;
END