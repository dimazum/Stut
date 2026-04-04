CREATE OR ALTER PROCEDURE dbo.UpdateVoiceAnalysis
(
    @dailyLessonId INT,
    @text NVARCHAR(MAX),
    @wordsSpoken INT
)
AS
BEGIN
    SET NOCOUNT ON;

UPDATE dbo.[DayLessons]
SET
    SpokenText = CONCAT(ISNULL(SpokenText, ''), ' ', @text),
    WordsSpoken = @wordsSpoken,

    -- считаем сколько времени прошло с прошлого апдейта
    LeftInSec = CASE
                    WHEN LeftInSec - DATEDIFF(SECOND, StartRangeTime, SYSUTCDATETIME()) < 0
                        THEN 0
                    ELSE LeftInSec - DATEDIFF(SECOND, StartRangeTime, SYSUTCDATETIME())
        END,

    -- обновляем время последнего апдейта
    StartRangeTime = SYSUTCDATETIME()
WHERE Id = @dailyLessonId;
END
GO