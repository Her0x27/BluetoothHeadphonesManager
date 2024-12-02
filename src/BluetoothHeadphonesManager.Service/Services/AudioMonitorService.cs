using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

using NAudio.CoreAudioApi;

public class AudioMonitorService
{
    private readonly ILogger<AudioMonitorService> _logger;
    private readonly MMDeviceEnumerator _deviceEnumerator;

    public AudioMonitorService(ILogger<AudioMonitorService> logger)
    {
        _logger = logger;
        _deviceEnumerator = new MMDeviceEnumerator();
    }

    public bool IsAudioPlaying()
    {
        try
        {
            using (var device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
            {
                // Check if audio level is above 0
                return device.AudioMeterInformation.MasterPeakValue > 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking audio status");
            return false;
        }
    }
}
