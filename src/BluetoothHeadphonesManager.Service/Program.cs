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
                services.AddHostedService<Worker>();
            })
            .Build();

        host.Run();
    }
}
