namespace stutvds.Logic.Services;

public interface ICurrentLanguage
{
    string Culture { get; set; }
}

public class CurrentLanguage : ICurrentLanguage
{
    public string Culture { get; set; }
}