using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorageExample.Infrastructure;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorageExample.Services;

public class ContainerService : IContainerService
{
  private readonly BlobServiceClient _blobServiceClient;

  public ContainerService(IAzureClientFactory<BlobServiceClient> factory) => 
    _blobServiceClient = factory.CreateClient(Constants.AzureBlobStorage);

  public async Task<bool> DeleteAsync(string name) =>
    await _blobServiceClient
      .GetBlobContainerClient(name)
      .DeleteIfExistsAsync();

  public async Task<IEnumerable<string>> GetAsync()
  {
    List<string> containers = new ();

    await foreach(var container in _blobServiceClient.GetBlobContainersAsync())
    {
      containers.Add(container.Name);
    }

    return containers;
  }
}
