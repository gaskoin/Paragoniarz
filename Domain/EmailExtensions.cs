using MimeKit;

namespace Paragoniarz.Domain;

public static class EmailExtensions
{
    public static BodyBuilder WithAttachment(this BodyBuilder bodyBuilder, string path)
    {
        bodyBuilder.Attachments.Add(path);
        return bodyBuilder;
    }

    public static BodyBuilder WithTextBody(this BodyBuilder bodyBuilder, string body)
    {
        bodyBuilder.TextBody = body;
        return bodyBuilder;
    }

    public static MimeMessage WithSubject(this MimeMessage message, string subject)
    {
        message.Subject = subject;
        return message;
    }

    public static MimeMessage WithBody(this MimeMessage message, MimeEntity body)
    {
        message.Body = body;
        return message;
    }

    public static MimeMessage WithFrom(this MimeMessage message, string name, string address)
    {
        message.From.Add(new MailboxAddress(name, address));
        return message;
    }

    public static MimeMessage WithTo(this MimeMessage message, string name, string address)
    {
        message.To.Add(new MailboxAddress(name, address));
        return message;
    }
}
