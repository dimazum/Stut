CREATE OR ALTER PROCEDURE dbo.GetChatMessages
    @SenderId NVARCHAR(100),
    @ReceiverId NVARCHAR(100)
    AS
BEGIN
    SET NOCOUNT ON;

SELECT *
FROM (
         SELECT TOP (100) *
         FROM ChatMessages
         WHERE
             (SenderId = @SenderId AND ReceiverId = @ReceiverId)
            OR
             (SenderId = @ReceiverId AND ReceiverId = @SenderId)
         ORDER BY SentAt DESC
     ) AS LastMessages
ORDER BY SentAt ASC;
END