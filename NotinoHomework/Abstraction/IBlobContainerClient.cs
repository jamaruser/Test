namespace NotinoHomework.Abstraction;

using Azure.Storage.Blobs;

public interface IBlobContainerClient
{
    BlobContainerClient GetBlobContainerClient();
}