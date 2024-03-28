using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paragoniarz.Domain.Settings;

namespace Paragoniarz.UI;

public partial class SettingsViewModel : ObservableObject
{
    private static readonly string PasswordMaskingSymbol = "*";

    private readonly IConfigurationService configurationService;
    private readonly IMessenger messenger;

    [ObservableProperty]
    private EmailConfiguration emailConfiguration;

    [ObservableProperty]
    private Configuration config;

    [ObservableProperty]
    private SellerConfiguration sellerConfiguration;

    [ObservableProperty]
    private bool isPasswordVisible;

    [ObservableProperty]
    private bool isLocked;

    [ObservableProperty]
    private string passwordMaskSymbol = PasswordMaskingSymbol;

    public SettingsViewModel(IConfigurationService configurationService, IMessenger messenger)
    {
        this.messenger = messenger;
        this.configurationService = configurationService;
        Config = configurationService.LoadConfiguration();
        EmailConfiguration = Config.EmailConfiguration;
        SellerConfiguration = Config.SellerConfiguration;
        IsLocked = Config.IsLocked;
    }

    [RelayCommand]
    private void Close()
    {
        configurationService.SaveConfiguration(Config);
        messenger.Send(new CloseSettingsMessage());
    }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordVisible = !IsPasswordVisible;
        PasswordMaskSymbol = IsPasswordVisible ? string.Empty : PasswordMaskingSymbol;
    }

    [RelayCommand]
    private void ToggleLock()
    {
        IsLocked = !IsLocked;
        Config.IsLocked = IsLocked;
    }
}