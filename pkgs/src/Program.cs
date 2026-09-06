namespace CaelestiaStylixSync;

public static class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            var settings = await GetSettings(GetConfigPath());
            var host = BuildHost(args, settings);
            await host.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex}");
            return 1;
        }
    }

    static string GetConfigPath() =>
        Environment.GetEnvironmentVariable("CAELESTIA_SYNC_CONFIG")
            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config/caelestia-stylix-sync/settings.yaml");

    static async Task<Settings> GetSettings(string configPath)
    {
        if (File.Exists(configPath))
        {
            var yaml = await File.ReadAllTextAsync(configPath);
            var deserializer = new DeserializerBuilder().Build();
            var settings = deserializer.Deserialize<Settings>(yaml);
            Console.WriteLine($"Loaded config from {configPath}");
            return settings;
        }
        else
        {
            Console.WriteLine($"Config file not found at {configPath}, using defaults");
            return new Settings();
        }
    }

    static IHost BuildHost(string[] args, Settings settings) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(settings);
                services.AddHostedService<ThemeSyncService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(settings.LogLevel);
            })
            .Build();
}
