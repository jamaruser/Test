namespace NotinoHomework.Services;

using Abstraction;
using Azure.Storage.Blobs.Models;
using Models;

public class BlobStorageService : IBlobStorageService
{
    private readonly IBlobContainerClient _blobContainerClient;

    public BlobStorageService(IBlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }
    public async Task<BlobContentInfo> UploadFile(string blobName, Stream file)
    {
        var container = _blobContainerClient.GetBlobContainerClient();
        var client = container.GetBlobClient(blobName);
        await client.DeleteIfExistsAsync();
        var result = await client.UploadAsync(file);
        return result;
    }

    public async Task<FileDto> DownloadFile(string blobName)
    {
        var container = _blobContainerClient.GetBlobContainerClient();
        var file = container.GetBlobClient(blobName);
        if (await file.ExistsAsync())
        {
            return new FileDto(await file.OpenReadAsync(), blobName);
        }

        throw new ArgumentException("Blob does not exist");
    }
}