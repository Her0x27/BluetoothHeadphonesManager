using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .UseWindowsService(options =>
            {
                options.ServiceName = "Bluetooth Headphones Manager";
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppSettings>(
                    hostContext.Configuration.GetSection("AppSettings"));
                
                services.AddSingleton<BluetoothService>();
                services.AddSingleton<AudioMonitorService>();
                services.AddSingleton<ConfigurationService>();
                services.AddHostedService<Worker>();

                services.Configure<HostOptions>(opts => 
                {
                    opts.ShutdownTimeout = TimeSpan.FromSeconds(30);
                });
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.AddEventLog(settings =>
                {
                    settings.SourceName = "Bluetooth Headphones Manager";
                });
            })
            .Build();

        host.Run();
    }
}
