using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using log4net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using Paragoniarz.Domain.Orders;
using Paragoniarz.Domain.Settings;

namespace Paragoniarz.Domain;
public class EmailService(IConfigurationService configurationService) : IEmailService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(EmailService));
    private readonly IConfigurationService configurationService = configurationService;

    public async Task SendEmails(OrderSummary summary, IProgress<int> progress)
    {
        var config = configurationService.LoadConfiguration();
        var emailTemplate = File.ReadAllText($"{config.AssetsDirectory}/email_template.txt");

        using SmtpClient smtpClient = await GetSmtpClient(config.EmailConfiguration);
        using ImapClient imapClient = await GetImapClient(config.EmailConfiguration);
        IMailFolder sentFolder = imapClient.GetFolder(SpecialFolder.Sent);
        await sentFolder.OpenAsync(FolderAccess.ReadWrite);

        foreach (Order order in summary.Orders)
        {
            MimeMessage message = CreateMessage(emailTemplate, order, config);

            log.Info($"Sending message for order({order.Id})");
            await smtpClient.SendAsync(message);
            log.Info($"Saving message in SENT mailbox for order({order.Id})");
            await sentFolder.AppendAsync(message);
            progress.Report(1);
        }

        log.Info($"Disconnecting from SMTP");
        await smtpClient.DisconnectAsync(true);

        log.Info($"Disconnecting from IMAP");
        await imapClient.DisconnectAsync(true);
    }

    private MimeMessage CreateMessage(string emailTemplate, Order order, Configuration config)
    {
        EmailConfiguration emailConfig = config.EmailConfiguration;

        string recipientName = order.Buyer.Email;
        if (emailConfig.UseRecipientName)
            recipientName = $"{order.Buyer.FirstName} {order.Buyer.LastName}";

        string recipientAddress = order.Buyer.Email;
        if (emailConfig.SendAllEmailsToSelf)
            recipientAddress = emailConfig.FromAddress;

        // TODO: Create document class instead of building path
        string attachment = $"{config.DocumentsDirectory}/potwierdzenie_{order.Id}.pdf";
        string subject = emailConfig.Subject.Replace("${orderId}", order.Id);
        string textBody = emailTemplate.Replace("${firstName}", order.Buyer.FirstName)
                                       .Replace("${lastName}", order.Buyer.LastName)
                                       .Replace("${orderId}", order.Id);

        return new MimeMessage().WithFrom(emailConfig.FromName, emailConfig.FromAddress)
                                .WithTo(recipientName, recipientAddress)
                                .WithSubject(subject)
                                .WithBody(
                                    new BodyBuilder().WithAttachment(attachment)
                                                     .WithTextBody(textBody)
                                                     .ToMessageBody()
                                );
    }

    private async Task<ImapClient> GetImapClient(EmailConfiguration config)
    {
        log.Info($"Connecting to IMAP at {config.ImapHost}:{config.ImapPort}");
        return await SetupClient(new ImapClient(), config.ImapHost, config.ImapPort, config);
    }

    private async Task<SmtpClient> GetSmtpClient(EmailConfiguration config)
    {
        log.Info($"Connecting to SMTP at {config.SmtpHost}:{config.SmtpPort}");
        return await SetupClient(new SmtpClient(), config.SmtpHost, config.SmtpPort, config);
    }

    private async Task<T> SetupClient<T>(T client, String host, int port, EmailConfiguration config) where T : IMailService
    {
        await client.ConnectAsync(host, port, true);
        await client.AuthenticateAsync(config.User, config.Password);
        client.ServerCertificateValidationCallback = CertificateValidationCallback;
        return client;
    }

    private bool CertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        log.Error($"Certificate error: {sslPolicyErrors}");
        return false;
    }
}
