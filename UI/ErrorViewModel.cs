using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net;
using Paragoniarz.Domain;

namespace Paragoniarz.UI;

public partial class ErrorViewModel(Exception exception) : ObservableObject
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DocumentService));

    [ObservableProperty]
    private string thrownException = exception.ToString();

    [RelayCommand]
    private void Close()
    {
        log.Info("Exiting application after fatal error. Bye bye");
        Environment.Exit(1);
    }
}