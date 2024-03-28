using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Paragoniarz.UI
{
    public class FileChosenMessage(Uri value) : ValueChangedMessage<Uri>(value);
}
