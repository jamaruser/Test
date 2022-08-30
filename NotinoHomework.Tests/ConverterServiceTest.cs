namespace NotinoHomework.Tests;

using System;
using System.IO;
using System.Threading.Tasks;
using Abstraction;
using FluentAssertions;
using Helpers;
using Models;
using Moq;
using Services;
using Xunit;

public class ConverterServiceTest
{
    private const string XmlText = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Title>title</Title><Text>text</Text></Document>";
    private const string JsonText = "{\"Title\":\"title\",\"Text\":\"text\"}";
    private const string InvalidText = "Invalid text";

    [Theory]
    [InlineData("json", "xml", JsonText, XmlText)]
    [InlineData("xml", "json", XmlText, JsonText)]
    [InlineData("JsoN", "XmL", JsonText, XmlText)]
    [InlineData("XmL", "jSOn", XmlText, JsonText)]
    public async Task ConvertSupportedTest(string sourceFormat, string targetFormat, string sourceText, string targetText)
    {
        var fileServiceMock = new Mock<IFileService>();
        var memoryStream = new MemoryStream();
        var fileName = $"test.{sourceFormat}";
        var resultFileName = $"test.{targetFormat}";
        var fileDto = new FileDto(memoryStream, fileName);
        fileServiceMock.Setup(q => q.ReadFile(memoryStream)).Returns(Task.FromResult(sourceText));

        var converterService = new ConverterService(fileServiceMock.Object);
        var result = await converterService.Convert(fileDto, targetFormat);
        result.FileName.Should().Be(resultFileName);
        result.ConvertedFile.Length.Should().Be(targetText.Length);
        result.ConvertedFile.Position.Should().Be(0);
        var reader = new StreamReader(result.ConvertedFile);
        var text = await reader.ReadToEndAsync();
        text.Should().Be(targetText);
        fileServiceMock.Verify();
    }


    [Theory]
    [InlineData("test.blabla", "json", ValidationMessages.InputNotSupported)]
    [InlineData("test.niceTry", "xml", ValidationMessages.InputNotSupported)]
    [InlineData("test.not", "supported", ValidationMessages.InputNotSupported)]
    [InlineData("test.json", "blabla", ValidationMessages.TargetNotSupported)]
    [InlineData("test.xml", "nicetry", ValidationMessages.TargetNotSupported)]
    [InlineData("test.json", "json", ValidationMessages.SourceTargetEquals)]
    [InlineData("test.xml", "xml", ValidationMessages.SourceTargetEquals)]
    [InlineData("test.xmL", "xMl", ValidationMessages.SourceTargetEquals)]
    [InlineData("test.bla", "bla", ValidationMessages.SourceTargetEquals)]
    [InlineData("test", "bla", ValidationMessages.TargetNotDetermined)]
    [InlineData("test.", "bla", ValidationMessages.TargetNotDetermined)]
    public async Task ConvertUnsupportedSourceTest(string fileName, string targetFormat, string message)
    {
        var fileServiceMock = new Mock<IFileService>();
        var fileDto = new FileDto(default, fileName);
        var converterService = new ConverterService(fileServiceMock.Object);
        Func<Task> act = async () => await converterService.Convert(fileDto, targetFormat);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(message);
    }

    [Theory]
    [InlineData("json", "xml")]
    [InlineData("xml", "json")]
    public async Task ConvertInvalidTextTest(string sourceFormat, string targetFormat)
    {
        var fileServiceMock = new Mock<IFileService>();
        var memoryStream = new MemoryStream();
        var fileName = $"test.{sourceFormat}";
        var resultFileName = $"test.{targetFormat}";
        var fileDto = new FileDto(memoryStream, fileName);
        fileServiceMock.Setup(q => q.ReadFile(memoryStream)).Returns(Task.FromResult(InvalidText));

        var converterService = new ConverterService(fileServiceMock.Object);
        Func<Task> act = async () => await converterService.Convert(fileDto, targetFormat);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage(ValidationMessages.InputFileNotInCorrectFormat);
    }
}