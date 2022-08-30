namespace NotinoHomework.Abstraction;

using Models;

public interface IFileProcessor
{
    Task ProcessFileFromBlobStorage(string fileName, string target);
    Task<ConvertResult> ProcessFileFromUrl(string fileName, string target);

    Task<ConvertResult> ProcessIncomingFile(IFormFile file, MailRequest mailRequest, string target);
}