using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace Paragoniarz.UI;

public partial class MainWindowViewModel : ObservableObject, IRecipient<FileChosenMessage>, IRecipient<CloseSettingsMessage>, IRecipient<ErrorMessage>
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));

    [ObservableProperty]
    private object? contentViewModel;
    private object? previous;

    private readonly IServiceProvider serviceProvider;
    private readonly IMessenger messenger;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.messenger = serviceProvider.GetRequiredService<IMessenger>();

        messenger.Register(this as IRecipient<FileChosenMessage>);
        messenger.Register(this as IRecipient<CloseSettingsMessage>);
        messenger.Register(this as IRecipient<ErrorMessage>);
        contentViewModel = serviceProvider.GetRequiredService<FileSelectorViewModel>();
    }

    public void OpenConfiguration()
    {
        if (ContentViewModel is SettingsViewModel)
            return;

        previous = ContentViewModel;
        ContentViewModel = serviceProvider.GetRequiredService<SettingsViewModel>();
    }

    public void Receive(FileChosenMessage message)
    {
        var model = serviceProvider.GetRequiredService<ProcessingViewModel>();
        model.ProcessFile(message.Value);
        ContentViewModel = model;
    }

    public void Receive(CloseSettingsMessage message)
    {
        ContentViewModel = previous;
    }

    public void Receive(ErrorMessage message)
    {
        log.Error(message.Value);
        ContentViewModel = new ErrorViewModel(message.Value);
    }
}