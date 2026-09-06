namespace CaelestiaStylixSync;

public class Settings
{
    [YamlMember(Alias = "themeFilePath")]
    public string ThemeFilePath { get; set; } = "/etc/nixos/caelestia-theme.yaml";

    [YamlMember(Alias = "schemeFilePath")]
    public string SchemeFilePath { get; set; } =
        Path.Combine(Environment.GetEnvironmentVariable("HOME")
                     ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                     ".local/state/caelestia/scheme.json");

    [YamlMember(Alias = "pollingIntervalSeconds")]
    public int PollingIntervalSeconds { get; set; } = 2;

    [YamlMember(Alias = "logLevel")]
    public string LogLevelString { get; set; } = "Information";

    [YamlMember(Alias = "generateWith")]
    public string GenerateWith { get; set; } = "base";

    public LogLevel LogLevel => Enum.TryParse<LogLevel>(LogLevelString, true, out var level) ? level : LogLevel.Information;
}
