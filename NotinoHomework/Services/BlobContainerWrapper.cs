namespace NotinoHomework.Services;

using Abstraction;
using Azure.Storage.Blobs;
using Configuration;
using Microsoft.Extensions.Options;

public class BlobContainerWrapper : IBlobContainerClient
{
    private readonly BlobStorageConfiguration _configuration;

    public BlobContainerWrapper(IOptions<BlobStorageConfiguration> options)
    {
        _configuration = options.Value;
    }

    public BlobContainerClient GetBlobContainerClient()
    {
        return new BlobContainerClient(_configuration.ConnectionString, _configuration.ContainerName);
    }
}