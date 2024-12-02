using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class ConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly string _configPath;
    private AppSettings _currentSettings;
    private readonly IOptionsMonitor<AppSettings> _settings;

    public ConfigurationService(
        ILogger<ConfigurationService> logger,
        IOptionsMonitor<AppSettings> settings)
    {
        _logger = logger;
        _settings = settings;
        _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        _currentSettings = _settings.CurrentValue;

        // Subscribe to configuration changes
        _settings.OnChange(settings =>
        {
            _currentSettings = settings;
            _logger.LogInformation("Configuration updated");
        });
    }

    public AppSettings GetCurrentSettings()
    {
        return _currentSettings;
    }

    public Task UpdateSettingsAsync(AppSettings newSettings)
    {
        var jsonConfig = await File.ReadAllTextAsync(_configPath);
        var config = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonConfig);
        
        config["AppSettings"] = newSettings;
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(
            _configPath, 
            JsonSerializer.Serialize(config, options)
        );

        _currentSettings = newSettings;
        _logger.LogInformation("Settings updated and saved to file");
    }

    public Task<bool> ValidateSettingsAsync(AppSettings settings)
    {
        if (string.IsNullOrEmpty(settings.DeviceName))
            return false;

        if (settings.InactivityTimeoutMinutes <= 0)
            return false;

        if (settings.AudioCheckIntervalSeconds <= 0)
            return false;

        return true;
    }
}
