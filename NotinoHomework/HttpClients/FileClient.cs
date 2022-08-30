namespace NotinoHomework.HttpClients;

using Abstraction;
using Configuration;
using Microsoft.Extensions.Options;
using Models;

public class FileClient : IFileClient
{
    private readonly HttpClient _client;
    private readonly FileClientConfiguration _configuration;

    public FileClient(HttpClient client, IOptions<FileClientConfiguration> options)
    {
        _client = client;
        _configuration = options.Value;
    }

    public async Task<FileDto> DownloadFile(string fileName)
    {
        var stream = await _client.GetStreamAsync($"{_configuration.BaseUrl}/DownloadFile/?name={fileName}");
        return new FileDto(stream, fileName);
    }
}