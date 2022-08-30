namespace NotinoHomework.Tests;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Abstraction;
using Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Xunit;

public class FileConverterControllerTest
{
    [Fact]
    public async Task ConvertEndpointTest()
    {
        var bytes = Encoding.UTF8.GetBytes("my file");
        var incomingFile = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "file.txt");
        var resultStream = new MemoryStream(bytes);

        var target = "format";
        var resultFileNme = "resultFileName";
        var mailRequest = new MailRequest();
        var fileProcessorMock = new Mock<IFileProcessor>();
        fileProcessorMock.Setup(mock => mock.ProcessIncomingFile(incomingFile, mailRequest, target))
            .Returns(() => Task.FromResult(new ConvertResult(resultStream, resultFileNme)))
            .Verifiable();
        var fileConverterController = new FileConverterController(fileProcessorMock.Object);
        var result = await fileConverterController.Convert(incomingFile, mailRequest, target);
        var fileStreamResult = result.Should().BeOfType<FileStreamResult>();
        fileStreamResult.Subject.FileDownloadName.Should().Be(resultFileNme);
        fileStreamResult.Subject.ContentType.Should().Be("application/octet-stream");
        fileProcessorMock.Verify();
    }

    [Fact]
    public async Task HttpConvertEnpointTest()
    {
        var bytes = Encoding.UTF8.GetBytes("my file");
        var requestFile = "file.txt";
        var resultStream = new MemoryStream(bytes);
        var target = "format";
        var resultFileNme = "resultFileName";
        var fileProcessorMock = new Mock<IFileProcessor>();
        fileProcessorMock.Setup(mock => mock.ProcessFileFromUrl(requestFile, target))
            .Returns(() => Task.FromResult(new ConvertResult(resultStream, resultFileNme)))
            .Verifiable();
        var fileConverterController = new FileConverterController(fileProcessorMock.Object);
        var result = await fileConverterController.ConvertDownloadedFile(new ConvertRequest(requestFile, target));
        var fileStreamResult = result.Should().BeOfType<FileStreamResult>();
        fileStreamResult.Subject.FileDownloadName.Should().Be(resultFileNme);
        fileStreamResult.Subject.ContentType.Should().Be("application/octet-stream");
        fileProcessorMock.Verify();
    }

    [Fact]
    public async Task BlobStorageConvertTest()
    {
        var requestFile = "file.txt";
        var target = "format";
        var fileProcessorMock = new Mock<IFileProcessor>();
        fileProcessorMock.Setup(mock => mock.ProcessFileFromBlobStorage(requestFile, target))
            .Verifiable();
        var fileConverterController = new FileConverterController(fileProcessorMock.Object);
        var result = await fileConverterController.BlobStorage(new ConvertRequest(requestFile, target));
        result.Should().BeOfType<OkResult>();
        fileProcessorMock.Verify();
    }
}