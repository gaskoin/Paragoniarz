using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace Paragoniarz.UI;

public partial class MainWindowViewModel : ObservableObject, IRecipient<FileSelectedMessage>, IRecipient<CloseSettingsMessage>, IRecipient<ErrorMessage>, IRecipient<SelectNewFileMessage>
{
    private static readonly ILog log = LogManager.GetLogger(typeof(MainWindowViewModel));

    [ObservableProperty]
    private object contentViewModel;
    private object previous;

    private readonly IServiceProvider serviceProvider;
    private readonly IMessenger messenger;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.messenger = serviceProvider.GetRequiredService<IMessenger>();

        messenger.Register(this as IRecipient<FileSelectedMessage>);
        messenger.Register(this as IRecipient<SelectNewFileMessage>);
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

    public void Receive(FileSelectedMessage message)
    {
        var model = serviceProvider.GetRequiredService<ProcessingViewModel>();
        ContentViewModel = model;
        model.ProcessFile(message.Value);
    }

    public void Receive(CloseSettingsMessage message)
    {
        ContentViewModel = previous;
    }

    public void Receive(ErrorMessage message)
    {
        log.Error(message.Value);
        new ErrorWindow()
        {
            DataContext = new ErrorWindowViewModel(message.Value),
        }.Show();
    }

    public void Receive(SelectNewFileMessage message)
    {
        ContentViewModel = serviceProvider.GetRequiredService<FileSelectorViewModel>();
    }
}