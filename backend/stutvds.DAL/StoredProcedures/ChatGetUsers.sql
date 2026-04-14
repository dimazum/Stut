CREATE OR ALTER PROCEDURE dbo.ChatGetUsers
    @MyUserId NVARCHAR(450)
    AS
BEGIN
    SET NOCOUNT ON;

SELECT
    u.Id,
    u.UserName                AS Name,
    lm.LastMessage,
    lm.LastMessageDate,

    ISNULL(uc.UnreadCount, 0) AS UnreadCount

FROM dbo.AspNetUsers u
    OUTER APPLY
    (
        SELECT TOP(1)
            m.Message AS LastMessage,
            m.SentAt AS LastMessageDate
        FROM dbo.ChatMessages m
        WHERE 
            (m.SenderId = @MyUserId AND m.ReceiverId = u.Id)
            OR
            (m.SenderId = u.Id AND m.ReceiverId = @MyUserId)
        ORDER BY m.SentAt DESC
    ) lm
    OUTER APPLY
    (
        SELECT COUNT(*) AS UnreadCount
        FROM dbo.ChatMessages m
        WHERE m.SenderId = u.Id
          AND m.ReceiverId = @MyUserId
          AND m.ReadAt IS NULL
    ) uc

WHERE u.Id <> @MyUserId

ORDER BY
    lm.LastMessageDate DESC,
    u.UserName ASC;
END