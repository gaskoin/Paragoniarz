using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Paragoniarz.UI;

public partial class FileSelectorViewModel(IStorageProvider storageProvider, IMessenger messenger) : ObservableObject
{
    private static readonly FilePickerFileType XMLDocument = new("XML Document")
    {
        Patterns = new[] { "*.xml", },
        AppleUniformTypeIdentifiers = new[] { "public.xml" },
        MimeTypes = new[] { "text/xml", "text/x-opml", "application/xml" }
    };

    [RelayCommand]
    private async Task OpenFileBrowser()
    {
        FilePickerOpenOptions options = new() { FileTypeFilter = [XMLDocument] };

        var chosenFiles = await storageProvider.OpenFilePickerAsync(options);
        ProcessFile(chosenFiles);
    }

    public void ProcessFile(IEnumerable<IStorageItem> items)
    {
        if (!items.Any())
            return;

        var path = items.First().Path;
        messenger.Send(new FileSelectedMessage(path));
    }
}