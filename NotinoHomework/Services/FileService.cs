namespace NotinoHomework.Services;

using Abstraction;

public class FileService : IFileService
{
    public async Task<string> ReadFile(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}