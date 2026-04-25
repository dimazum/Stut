namespace stutvds.Localization;

public interface ILocalizationVersionStore
{
    string CurrentVersion { get; }
    void SetCurrentVersion(string version);
}