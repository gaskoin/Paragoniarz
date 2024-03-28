using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net;

namespace Paragoniarz.UI;

public partial class ErrorWindowViewModel(Exception exception) : ObservableObject
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ErrorWindowViewModel));

    [ObservableProperty]
    private string thrownException = exception.ToString();

    [RelayCommand]
    private void Close(Window window) => window.Close();
}