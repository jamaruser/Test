namespace NotinoHomework.Services;

using Abstraction;
using Configuration;
using Helpers;
using Microsoft.Extensions.Options;
using MimeKit;
using Models;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ISmtpClient _smtpClient;

    public EmailService(IOptions<EmailConfiguration> emailConfiguration, ISmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
        _emailConfiguration = emailConfiguration.Value;
    }

    public async Task SendEmail(MailRequest mailRequest, string fileName, Stream fileContent)
    {
        var email = new MimeMessage();
        if (string.IsNullOrEmpty(_emailConfiguration.From) || !MailboxAddress.TryParse(_emailConfiguration.From, out var sender))
        {
            throw new ArgumentException(ValidationMessages.InvalidSenderAddress);
        }

        if (string.IsNullOrEmpty(mailRequest.To) || !MailboxAddress.TryParse(mailRequest.To, out var receiver))
        {
            throw new ArgumentException(ValidationMessages.InvalidReceiverAddress);
        }

        email.Sender = sender;
        email.To.Add(receiver);
        email.Subject = mailRequest.Subject ?? string.Empty;
        var builder = new BodyBuilder();
        if (fileContent is {Length: > 0})
        {
            await builder.Attachments.AddAsync(fileName, fileContent);
        }

        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();
        await _smtpClient.Send(email);
    }
}