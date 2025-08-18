using System.Collections.Generic;

public class DayData
{
    public string Date { get; set; } = null!; // формат "YYYY-MM-DD"
    public bool Done { get; set; }
    public bool Rewarded { get; set; }
    public int WordsRead { get; set; }
}

public class CalendarData
{
    public int Year { get; set; }
    public int Month { get; set; } // 0 = Январь, 11 = Декабрь
    public List<DayData> Days { get; set; }
}