using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Paragoniarz.Domain;
using Paragoniarz.Domain.Orders;
using Paragoniarz.Domain.Settings;

namespace Paragoniarz.UI;

public partial class ProcessingViewModel(IMessenger messenger, IConfigurationService configurationService, IOrderSummaryService orderSummaryService, IDocumentService documentService, IEmailService emailService) : ObservableObject
{
    [ObservableProperty]
    private bool isFileRead = false;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendEmailsCommand))]
    private bool areDocumentsGenerated = false;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendEmailsCommand))]
    private bool areMessagesSent = false;

    [ObservableProperty]
    private int progress = 0;

    private readonly string documentsDirectory = configurationService.LoadConfiguration().DocumentsDirectory;
    private OrderSummary? orderSummary;


    public async Task ProcessFile(Uri uri)
    {
        try
        {
            orderSummary = await orderSummaryService.ReadFile(uri);
            Dispatcher.UIThread.Post(() => IsFileRead = true);

            orderSummary = await documentService.GenerateDocuments(orderSummary, new Progress<int>(UpdateProgress));
            Dispatcher.UIThread.Post(() => AreDocumentsGenerated = true);
        }
        catch (Exception e)
        {
            messenger.Send(new ErrorMessage(e));
        }
    }

    [RelayCommand]
    private void OpenDocumentsLocation()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = documentsDirectory,
            UseShellExecute = true
        });
    }

    [RelayCommand(CanExecute = nameof(CanSendEmail))]
    public async Task SendEmails()
    {
        try
        {
            Progress = 0;
            IProgress<int> progress = new Progress<int>(UpdateProgress);
            await emailService.SendEmails(orderSummary!, new Progress<int>(UpdateProgress));

            Dispatcher.UIThread.Post(() => AreMessagesSent = true);
        }
        catch (Exception e)
        {
            messenger.Send(new ErrorMessage(e));
        }
    }

    private bool CanSendEmail() => AreDocumentsGenerated && !AreMessagesSent;

    private void UpdateProgress(int value) => Dispatcher.UIThread.Post(() => Progress += 100 / orderSummary!.Orders.Count());
}