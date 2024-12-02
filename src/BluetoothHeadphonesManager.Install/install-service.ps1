$serviceName = "BluetoothHeadphonesManager"
$serviceDisplayName = "Bluetooth Headphones Manager"
$serviceDescription = "Manages Bluetooth headphones connection based on audio activity"
$exePath = Join-Path $PSScriptRoot "..\BluetoothHeadphonesManager.Service.exe"

New-Service -Name $serviceName `
            -DisplayName $serviceDisplayName `
            -Description $serviceDescription `
            -BinaryPathName $exePath `
            -StartupType Automatic

Start-Service -Name $serviceName
