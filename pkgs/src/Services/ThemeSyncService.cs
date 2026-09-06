namespace CaelestiaStylixSync.Services;

public class ThemeSyncService : BackgroundService
{
    private readonly ILogger<ThemeSyncService> _logger;
    private readonly Settings _settings;
    private readonly string _schemeFilePath;
    private readonly string _themeFilePath;
    private DateTime _lastWriteTime = DateTime.MinValue;
    private readonly SemaphoreSlim _processingLock = new(1, 1);
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ThemeSyncService(ILogger<ThemeSyncService> logger, Settings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        if (string.IsNullOrWhiteSpace(_settings.SchemeFilePath))
            throw new ArgumentException("SchemeFilePath must be provided.", nameof(settings));
        if (string.IsNullOrWhiteSpace(_settings.ThemeFilePath))
            throw new ArgumentException("ThemeFilePath must be provided.", nameof(settings));
        if (_settings.PollingIntervalSeconds <= 0)
            throw new ArgumentException("PollingIntervalSeconds must be positive.", nameof(settings));

        _schemeFilePath = Path.GetFullPath(_settings.SchemeFilePath);
        _themeFilePath = Path.GetFullPath(_settings.ThemeFilePath);

        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Watching scheme file: {SchemeFilePath}", _schemeFilePath);

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ThemeSyncService started.");
        await ProcessFileIfChangedAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.PollingIntervalSeconds), stoppingToken);
            await ProcessFileIfChangedAsync(stoppingToken);
        }
    }

    private async Task ProcessFileIfChangedAsync(CancellationToken cancellationToken)
    {
        if (!await _processingLock.WaitAsync(0, cancellationToken))
        {
            _logger.LogDebug("Processing already in progress, skipping this poll.");
            return;
        }

        try
        {
            await ProcessFileInternalAsync(cancellationToken);
        }
        finally
        {
            _processingLock.Release();
        }
    }

    private async Task ProcessFileInternalAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(_schemeFilePath))
            {
                _logger.LogWarning("Scheme file not found at {Path}", _schemeFilePath);
                return;
            }

            var currentWriteTime = File.GetLastWriteTimeUtc(_schemeFilePath);
            if (currentWriteTime == _lastWriteTime)
                return;

            var scheme = await ReadSchemeWithRetryAsync(cancellationToken);
            if (scheme == null)
                return;

            await GenerateAndSaveThemeAsync(scheme, cancellationToken);

            _lastWriteTime = currentWriteTime;

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Theme successfully updated from scheme '{SchemeName}' (mode: {Mode}) using generator '{Generator}'",
                    scheme.Name, scheme.Mode, _settings.GenerateWith);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing theme update");
        }
    }

    private async Task<CaelestiaScheme?> ReadSchemeWithRetryAsync(CancellationToken cancellationToken, int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var json = await File.ReadAllTextAsync(_schemeFilePath, Encoding.UTF8, cancellationToken);
                var scheme = JsonSerializer.Deserialize<CaelestiaScheme>(json, _jsonOptions);
                if (scheme == null)
                {
                    _logger.LogError("Deserialization of scheme.json returned null (invalid JSON).");
                    return null;
                }

                if (attempt > 1 && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug("Successfully read scheme.json after {Attempt} attempts.", attempt);

                return scheme;
            }
            catch (IOException ex) when (attempt < maxRetries)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                    _logger.LogWarning(ex,
                        "IO error reading scheme.json (attempt {Attempt}/{MaxRetries}). Retrying...",
                        attempt, maxRetries);

                await Task.Delay(100 * attempt, cancellationToken);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON format in scheme.json");
                return null;
            }
        }

        _logger.LogError("Failed to read scheme.json after {MaxRetries} attempts.", maxRetries);
        return null;
    }

    private async Task GenerateAndSaveThemeAsync(CaelestiaScheme scheme, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var yaml = ThemeGenerator.GenerateYaml(scheme, _settings.GenerateWith);
        await File.WriteAllTextAsync(_themeFilePath, yaml, Encoding.UTF8, cancellationToken);
    }
}
