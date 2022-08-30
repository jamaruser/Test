namespace NotinoHomework.Abstraction;

using Azure.Storage.Blobs.Models;
using Models;

public interface IBlobStorageService
{
    Task<BlobContentInfo> UploadFile(string blobName, Stream file);
    Task<FileDto> DownloadFile(string blobName);
}