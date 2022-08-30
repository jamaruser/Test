namespace NotinoHomework.Abstraction;

using Models;

public interface IFileClient
{
    Task<FileDto> DownloadFile(string fileName);
}