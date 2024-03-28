using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace Paragoniarz.UI;

public partial class FileSelectorView : UserControl
{
    private FileSelectorViewModel ViewModel => (FileSelectorViewModel)DataContext!;

    public FileSelectorView()
    {
        InitializeComponent();
        DragDrop.SetAllowDrop(this, true);
        AddHandler(DragDrop.DropEvent, OnDrop);
        AddHandler(DragDrop.DragEnterEvent, OnDragOver);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
    }

    private void OnDrop(object? sender, DragEventArgs args)
    {
        ViewModel.ProcessFile(args.Data.GetFiles()!);
        ShowDragOrBrowseText();
    }

    private void OnDragOver(object? sender, DragEventArgs args)
    {
        ShowDropHereText();
    }
    private void OnDragLeave(object? sender, DragEventArgs args)
    {
        ShowDragOrBrowseText();
    }

    private void ShowDragOrBrowseText()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            DragOrBrowseText.IsVisible = true;
            DropHereText.IsVisible = false;
        });
    }
    private void ShowDropHereText()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            DragOrBrowseText.IsVisible = false;
            DropHereText.IsVisible = true;
        });
    }
}