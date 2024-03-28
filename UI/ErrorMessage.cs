using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Paragoniarz.UI
{
    public class ErrorMessage(Exception value) : ValueChangedMessage<Exception>(value);
}
