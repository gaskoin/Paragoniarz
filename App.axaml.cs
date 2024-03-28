using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Paragoniarz.Domain;
using Paragoniarz.Domain.Orders;
using Paragoniarz.Domain.Settings;
using Paragoniarz.UI;

namespace Paragoniarz;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = BuildServiceProvider();
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection().AddSingleton(new Window().StorageProvider)
                                      .AddSingleton<IConfigurationService, ConfigurationService>()
                                      .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
                                      .AddTransient<MainWindowViewModel>()
                                      .AddTransient<FileSelectorViewModel>()
                                      .AddTransient<SettingsViewModel>()
                                      .AddTransient<ProcessingViewModel>()
                                      .AddTransient<IOrderSummaryService, OrderSummaryService>()
                                      .AddTransient<IEmailService, EmailService>()
                                      .AddTransient<IDocumentService, DocumentService>()
                                      .BuildServiceProvider();
    }
}