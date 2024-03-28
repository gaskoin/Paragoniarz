using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using log4net;
using log4net.Config;

namespace Paragoniarz;

sealed class Program
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    private static readonly string loggerConfiguration = $"{AppDomain.CurrentDomain.BaseDirectory}/log4net.config";

    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            XmlConfigurator.Configure(new FileInfo(loggerConfiguration));
            RegisterUnobservedExceptionHandler();
            log.Info("Starting application");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            log.Error(e);
        }
    }

    private static void RegisterUnobservedExceptionHandler()
    {
        TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
        {
            eventArgs.SetObserved();
            eventArgs.Exception.Handle(e =>
            {
                log.Error(e);
                return true;
            });
        };
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
