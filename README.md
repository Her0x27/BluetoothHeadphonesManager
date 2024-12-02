# Bluetooth Headphones Manager

Windows service that automatically manages Bluetooth headphones based on audio activity.

## Features

- Automatic disconnection after period of inactivity
- Optional auto-connection when audio starts playing
- Configurable settings through JSON file
- Runs as a Windows service
- Event logging

## Installation

1. Download the latest release from GitHub releases
2. Extract the files to your desired location (e.g., `C:\Program Files\BluetoothHeadphonesManager`)
3. Open PowerShell as Administrator
4. Navigate to the installation directory
5. Run the installation script:
```powershell
.\install-service.ps1
```

# Configuration
Edit appsettings.json in the installation directory:
```
{
  "AppSettings": {
    "DeviceName": "Your Bluetooth Headphones Name",
    "InactivityTimeoutMinutes": 30,
    "AutoConnectEnabled": true,
    "AudioCheckIntervalSeconds": 5
  }
}
```

# Settings Description

    DeviceName: Exact name of your Bluetooth headphones as shown in Windows
    InactivityTimeoutMinutes: Time without audio before disconnecting
    AutoConnectEnabled: Enable/disable automatic connection when audio plays
    AudioCheckIntervalSeconds: How often to check for audio activity

# Usage

The service starts automatically with Windows. You can manage it through:

    Services app (services.msc)
    PowerShell commands:
```
Start-Service BluetoothHeadphonesManager
Stop-Service BluetoothHeadphonesManager
Restart-Service BluetoothHeadphonesManager
```

# Logs

Check Windows Event Viewer > Applications and Services Logs > Bluetooth Headphones Manager
System Requirements

    Windows 11
    .NET 7.0 Runtime
    Bluetooth adapter
    Administrator rights for installation

# Building from Source

    Clone the repository
    Install .NET 7.0 SDK
    Run:
```
dotnet build
dotnet publish -c Release
```

# License
MIT License
