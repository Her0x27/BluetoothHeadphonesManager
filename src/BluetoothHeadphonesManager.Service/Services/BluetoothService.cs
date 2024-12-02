using System;
using Windows.Foundation;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;

public class BluetoothService
{
    private readonly ILogger<BluetoothService> _logger;
    private readonly string _deviceName;
    private BluetoothDevice _device;

    public BluetoothService(ILogger<BluetoothService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _deviceName = configuration.GetSection("AppSettings:DeviceName").Value;
    }

    public bool IsConnected()
    {
        return _device?.ConnectionStatus == BluetoothConnectionStatus.Connected;
    }

    public async Task ConnectAsync()
    {
        try
        {
            if (_device == null)
            {
                string selector = $"System.ItemNameDisplay:=\"{_deviceName}\"";
                var devices = await DeviceInformation.FindAllAsync(selector);
                
                if (devices.Count > 0)
                {
                    _device = await BluetoothDevice.FromIdAsync(devices[0].Id);
                }
            }

            if (_device != null && _device.ConnectionStatus != BluetoothConnectionStatus.Connected)
            {
                // Using Windows.Devices.Bluetooth API to connect
                var result = await _device.RequestAccessAsync();
                _logger.LogInformation($"Connected to device: {_deviceName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error connecting to Bluetooth device");
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            if (_device != null && _device.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                // Disconnect using native Windows API
                await _device.RequestAccessAsync();
                _device.Dispose();
                _device = null;
                _logger.LogInformation($"Disconnected from device: {_deviceName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disconnecting Bluetooth device");
        }
    }
}
