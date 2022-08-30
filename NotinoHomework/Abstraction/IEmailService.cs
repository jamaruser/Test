namespace NotinoHomework.Abstraction;

using Models;

public interface IEmailService
{
    Task SendEmail(MailRequest mailRequest, string fileName, Stream attachment);
}