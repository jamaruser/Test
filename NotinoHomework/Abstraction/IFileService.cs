namespace NotinoHomework.Abstraction;

public interface IFileService
{
    Task<string> ReadFile(Stream stream);
}