CREATE OR ALTER PROCEDURE dbo.ChatGetUsers
    @MyUserId NVARCHAR(450)
    AS
BEGIN
    SET NOCOUNT ON;

SELECT
    u.Id,
    u.UserName,
    lm.LastMessageDate
FROM dbo.AspNetUsers u
    OUTER APPLY
    (
        SELECT TOP(1) m.SentAt AS LastMessageDate
        FROM dbo.ChatMessages m
        WHERE 
            (m.SenderId = @MyUserId AND m.ReceiverId = u.Id)
            OR
            (m.SenderId = u.Id AND m.ReceiverId = @MyUserId)
        ORDER BY m.SentAt DESC
    ) lm
WHERE u.Id <> @MyUserId
ORDER BY
    lm.LastMessageDate DESC,   -- сначала с диалогами
    u.UserName ASC;            -- потом без диалогов

END