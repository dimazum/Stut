using System;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.Models.ClientDto;

public class DailyLessonDto
{
    public Guid UserId { get; set; }
    public LessonStatus Status { get; set; }
    public bool Rewarded { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StartRangeTime { get; set; }
    public DateTimeOffset? FinishTime { get; set; }
    public int LeftInSec { get; set; }
    public int WordsSpoken { get; set; }
    public int WPS { get; set; }
    public int RewardPoints { get; set; }
}