namespace NotinoHomework.Services;

using Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ISmtpClient = Abstraction.ISmtpClient;

public class SmtpClientWrapper : ISmtpClient
{
    private readonly SmtpConfiguration _smtpConfiguration;

    public SmtpClientWrapper(IOptions<SmtpConfiguration> smtpConfiguration)
    {
        _smtpConfiguration = smtpConfiguration.Value;
    }

    public async Task Send(MimeMessage email)
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpConfiguration.UserName, _smtpConfiguration.Host);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}