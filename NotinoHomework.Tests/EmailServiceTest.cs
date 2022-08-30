namespace NotinoHomework.Tests;

using System;
using System.Linq;
using System.Threading.Tasks;
using Abstraction;
using Configuration;
using FluentAssertions;
using Helpers;
using Microsoft.Extensions.Options;
using MimeKit;
using Models;
using Moq;
using Services;
using Xunit;

public class EmailServiceTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("this is not an email address")]
    public async Task SendEmailWithInvalidSenderTest(string sender)
    {
        var smtpClientMock = new Mock<ISmtpClient>();
        var emailConfig = new EmailConfiguration {From = sender};

        var mock = new Mock<IOptions<EmailConfiguration>>();
        mock.Setup(ap => ap.Value).Returns(emailConfig);
        smtpClientMock.Setup(q => q.Send(It.IsAny<MimeMessage>())).Verifiable();
        var emailService = new EmailService(mock.Object, smtpClientMock.Object);
        Func<Task> act = async () => await emailService.SendEmail(new MailRequest(), "file.txt", null);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(ValidationMessages.InvalidSenderAddress);
        smtpClientMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("this is not an email address")]
    public async Task SendEmailWithInvalidReceiverTest(string receiver)
    {
        var smtpClientMock = new Mock<ISmtpClient>();
        var emailConfig = new EmailConfiguration {From = "good@email.address"};

        var mock = new Mock<IOptions<EmailConfiguration>>();
        mock.Setup(ap => ap.Value).Returns(emailConfig);
        smtpClientMock.Setup(q => q.Send(It.IsAny<MimeMessage>())).Verifiable();
        var emailService = new EmailService(mock.Object, smtpClientMock.Object);
        Func<Task> act = async () => await emailService.SendEmail(new MailRequest {To = receiver}, "file.txt", null);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(ValidationMessages.InvalidReceiverAddress);
        smtpClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendEmailTest()
    {
        var sender = "sender@email.address";
        var receiver = "receiver@email.address";
        var smtpClientMock = new Mock<ISmtpClient>();
        var emailConfig = new EmailConfiguration {From = sender};
        var mock = new Mock<IOptions<EmailConfiguration>>();
        mock.Setup(ap => ap.Value).Returns(emailConfig);
        smtpClientMock.Setup(q => q.Send(It.Is<MimeMessage>(q =>
            !q.Attachments.Any() &&
            q.Sender.Address == sender &&
            q.To.Mailboxes.Any(s => s.Address == receiver)))).Verifiable();
        var emailService = new EmailService(mock.Object, smtpClientMock.Object);
        await emailService.SendEmail(new MailRequest {To = receiver}, "file.txt", null);
        smtpClientMock.Verify();
    }
}