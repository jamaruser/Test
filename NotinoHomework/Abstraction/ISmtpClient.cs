namespace NotinoHomework.Abstraction;

using MimeKit;

public interface ISmtpClient
{
    Task Send(MimeMessage mimeMessage);
}