using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly BluetoothService _bluetoothService;
    private readonly AudioMonitorService _audioMonitorService;
    private readonly IConfiguration _configuration;
    private DateTime _lastAudioActivity;

    public Worker(ILogger<Worker> logger, 
                 BluetoothService bluetoothService,
                 AudioMonitorService audioMonitorService,
                 IConfiguration configuration)
    {
        _logger = logger;
        _bluetoothService = bluetoothService;
        _audioMonitorService = audioMonitorService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = _configuration.GetSection("AppSettings").Get<AppSettings>();

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_audioMonitorService.IsAudioPlaying())
            {
                _lastAudioActivity = DateTime.Now;
                
                if (settings.AutoConnectEnabled && !_bluetoothService.IsConnected())
                {
                    await _bluetoothService.ConnectAsync();
                }
            }
            else
            {
                var inactiveTime = DateTime.Now - _lastAudioActivity;
                if (inactiveTime.TotalMinutes >= settings.InactivityTimeoutMinutes)
                {
                    await _bluetoothService.DisconnectAsync();
                }
            }

            await Task.Delay(settings.AudioCheckIntervalSeconds * 1000, stoppingToken);
        }
    }
}
