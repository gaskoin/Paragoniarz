using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Paragoniarz.UI
{
    public class FileSelectedMessage(Uri value) : ValueChangedMessage<Uri>(value);
}
