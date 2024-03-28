using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paragoniarz.Domain.Settings;

namespace Paragoniarz.UI;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IConfigurationService configurationService;
    private readonly IMessenger messenger;

    [ObservableProperty]
    private EmailConfiguration emailConfiguration;

    [ObservableProperty]
    private SellerConfiguration sellerConfiguration;

    public SettingsViewModel(IConfigurationService configurationService, IMessenger messenger)
    {
        this.messenger = messenger;
        this.configurationService = configurationService;
        var configuration = configurationService.LoadConfiguration();
        EmailConfiguration = configuration.EmailConfiguration;
        SellerConfiguration = configuration.SellerConfiguration;
    }

    [RelayCommand]
    private void Close()
    {
        configurationService.SaveConfiguration(new Configuration(EmailConfiguration, SellerConfiguration));
        messenger.Send(new CloseSettingsMessage());
    }
}