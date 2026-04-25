namespace stutvds.Localization;

public class LocalizationVersionStore : ILocalizationVersionStore
{
    public string CurrentVersion { get; private set; } = "init";

    public void SetCurrentVersion(string version)
    {
        CurrentVersion = version;
    }
}