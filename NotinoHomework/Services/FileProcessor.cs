namespace NotinoHomework.Services;

using Abstraction;
using Models;

public class FileProcessor : IFileProcessor
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IConverterService _converterService;
    private readonly IEmailService _emailService;
    private readonly IFileClient _fileClient;

    public FileProcessor(IBlobStorageService blobStorageService, IConverterService converterService, IFileClient fileClient, IEmailService emailService)
    {
        _blobStorageService = blobStorageService;
        _converterService = converterService;
        _fileClient = fileClient;
        _emailService = emailService;
    }

    public async Task ProcessFileFromBlobStorage(string fileName, string target)
    {
        var downloadedFile = await _blobStorageService.DownloadFile(fileName);
        var convertedFile = await _converterService.Convert(downloadedFile, target);
        await _blobStorageService.UploadFile(convertedFile.FileName, convertedFile.ConvertedFile);
    }

    public async Task<ConvertResult> ProcessFileFromUrl(string fileName, string target)
    {
        var downloadedFile = await _fileClient.DownloadFile(fileName);
        return await _converterService.Convert(downloadedFile, target);
    }

    public async Task<ConvertResult> ProcessIncomingFile(IFormFile file, MailRequest mailRequest, string target)
    {
        var convertResult = await _converterService.Convert(new FileDto(file.OpenReadStream(), file.FileName), target);
        if (!string.IsNullOrEmpty(mailRequest.To))
        {
            await _emailService.SendEmail(mailRequest, convertResult.FileName, convertResult.ConvertedFile);
        }

        return convertResult;
    }
}

