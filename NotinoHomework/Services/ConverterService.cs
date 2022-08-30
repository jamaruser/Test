namespace NotinoHomework.Services;

using Abstraction;
using Converters;
using Helpers;
using Models;

public class ConverterService : IConverterService
{
    private static readonly Dictionary<string, IFormatConverter> Converters = new(StringComparer.OrdinalIgnoreCase)
    {
        ["json"] = new JsonConverter(),
        ["xml"] = new XmlConverter()
    };

    private readonly IFileService _fileService;

    public ConverterService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<ConvertResult> Convert(FileDto input, string target)
    {
        var (stream, fileName) = input;
        var fileNameExtension = FileHelper.GetExtension(fileName);
        ValidateInput(target, fileNameExtension);
        var fileContent = await _fileService.ReadFile(stream);
        var targetConverter = Converters[target];

        var sourceConverter = Converters[fileNameExtension];
        var stringResult = targetConverter.ConvertTo(sourceConverter.ConvertFrom(fileContent));
        var newFileName = FileHelper.ChangeExtension(fileName, target);
        return new ConvertResult(FileHelper.GenerateStreamFromString(stringResult), newFileName);
    }

    private static void ValidateInput(string target, string fileNameExtension)
    {
        if (string.IsNullOrWhiteSpace(fileNameExtension))
        {
            throw new ArgumentException(ValidationMessages.TargetNotDetermined);
        }

        if (target.Equals(fileNameExtension, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ArgumentException(ValidationMessages.SourceTargetEquals);
        }

        if (!Converters.ContainsKey(fileNameExtension))
        {
            throw new ArgumentException(ValidationMessages.InputNotSupported);
        }

        if (!Converters.ContainsKey(target))
        {
            throw new ArgumentException(ValidationMessages.TargetNotSupported);
        }
    }
}