namespace NotinoHomework.Tests;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Abstraction;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Models;
using Moq;
using Services;
using Xunit;

public class FileProcessorTest
{
    [Fact]
    public async Task ProcessIncomingFileTest()
    {
        var converterServiceMock = new Mock<IConverterService>();
        var emailServiceMock = new Mock<IEmailService>();
        var target = "format";
        var resultFileNme = "resultFileName";
        var bytes = Encoding.UTF8.GetBytes("my file");
        var incomingFile = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "file.txt");
        var resultStream = new MemoryStream(bytes);
        var mailRequest = new MailRequest()
        {
         To   = "seznam@gmail.com"
        };
        converterServiceMock.Setup(mock => mock.Convert(It.IsAny<FileDto>(), target))
            .Returns(() => Task.FromResult(new ConvertResult(resultStream, resultFileNme)))
            .Verifiable();
        emailServiceMock.Setup(s => s.SendEmail(mailRequest, resultFileNme, resultStream)).Verifiable();
        var fileProcessor = new FileProcessor(new Mock<IBlobStorageService>().Object, converterServiceMock.Object, new Mock<IFileClient>().Object,
            emailServiceMock.Object);
        var result = await fileProcessor.ProcessIncomingFile(incomingFile, mailRequest, target);
        result.ConvertedFile.Should().BeSameAs(resultStream);
        result.FileName.Should().Be(resultFileNme);
        converterServiceMock.Verify();
        emailServiceMock.Verify();
    }

    [Fact]
    public async Task ProcessFileFromUrlTest()
    {
        var converterServiceMock = new Mock<IConverterService>();
        var fileClientMock = new Mock<IFileClient>();
        var target = "format";
        var fileName = "file.txt";
        var bytes = Encoding.UTF8.GetBytes("my file");
        var resultStream = new MemoryStream(bytes);
        var resultFileNme = "resultFileName";
        converterServiceMock.Setup(mock => mock.Convert(It.IsAny<FileDto>(), target))
            .Returns(() => Task.FromResult(new ConvertResult(resultStream, resultFileNme)))
            .Verifiable();
        fileClientMock.Setup(s => s.DownloadFile(fileName)).Verifiable();
        var fileProcessor = new FileProcessor(new Mock<IBlobStorageService>().Object, converterServiceMock.Object,
            fileClientMock.Object, new Mock<IEmailService>().Object);
        var result = await fileProcessor.ProcessFileFromUrl(fileName, target);
        result.ConvertedFile.Should().BeSameAs(resultStream);
        result.FileName.Should().Be(resultFileNme);
        converterServiceMock.Verify();
        fileClientMock.Verify();
    }

    [Fact]
    public async Task ProcessFileFromBlobStorageTest()
    {
        var converterServiceMock = new Mock<IConverterService>();
        var blobStorageServiceMock = new Mock<IBlobStorageService>();
        var target = "format";
        var fileName = "file.txt";
        var resultFileNme = "resultFileName";
        var bytes = Encoding.UTF8.GetBytes("my file");
        var resultStream = new MemoryStream(bytes);
        converterServiceMock.Setup(mock => mock.Convert(It.IsAny<FileDto>(), target))
            .Returns(() => Task.FromResult(new ConvertResult(resultStream, resultFileNme)))
            .Verifiable();
        blobStorageServiceMock.Setup(s => s.DownloadFile(fileName)).Verifiable();
        blobStorageServiceMock.Setup(s => s.UploadFile(resultFileNme, resultStream)).Verifiable();
        var fileProcessor = new FileProcessor(blobStorageServiceMock.Object, converterServiceMock.Object,
            new Mock<IFileClient>().Object, new Mock<IEmailService>().Object);
        await fileProcessor.ProcessFileFromBlobStorage(fileName, target);
        converterServiceMock.Verify();
        blobStorageServiceMock.Verify();
    }
}