using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorageExample.Infrastructure;
using BlobStorageExample.Models.Requests;
using BlobStorageExample.Models.Responses;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorageExample.Services;

public class BlobService : IBlobService
{
  private readonly BlobServiceClient _blobServiceClient;

  public async Task<IEnumerable<string>> GetListAsync(string container)
  {
    List<string> blobs = new();
    BlobContainerClient blobContainerClient = 
      _blobServiceClient.GetBlobContainerClient(container);

    await foreach(var blob in blobContainerClient.GetBlobsAsync())
    {
      blobs.Add(blob.Name);
    }

    return blobs;
  }

  public BlobService(IAzureClientFactory<BlobServiceClient> factory) => 
    _blobServiceClient = factory.CreateClient(Constants.AzureBlobStorage);

  public async Task<bool> DeleteAsync(DeleteBlobRequest request) =>
    await _blobServiceClient.GetBlobContainerClient(request.Container)?.DeleteBlobIfExistsAsync(request.Blob);

  public async Task<DownloadBlobResponse> DownloadAsync(DownloadBlobRequest downloadBlobRequest)
  {
    var blobContainer = _blobServiceClient.GetBlobContainerClient(downloadBlobRequest.Container);
    var blobClient = blobContainer.GetBlobClient(downloadBlobRequest.Blob);
    var downloadedContent = await blobClient.DownloadAsync();

    return new DownloadBlobResponse(blobClient.Name, downloadedContent.Value.Content, downloadedContent.Value.ContentType);
  }

  public async Task<string> GetAsync(GetBlobRequest request)
  {
    var blobContainer = _blobServiceClient.GetBlobContainerClient(request.Container);
    var blobClient = blobContainer.GetBlobClient(request.Blob);

    return await Task.FromResult(blobClient.Uri.AbsoluteUri);
  }
    
  public async Task<string> SaveAsync(SaveBlobRequest request)
  {
    var blobContainer = _blobServiceClient.GetBlobContainerClient(request.Container);
    await blobContainer.CreateIfNotExistsAsync(PublicAccessType.BlobContainer, null);

    var blobName = $"{Guid.NewGuid():N}{Path.GetExtension(request.File.FileName)}";
    var blobClient = blobContainer.GetBlobClient(blobName);
    
    await blobClient.UploadAsync(request.File.OpenReadStream());

    return blobClient.Uri.AbsoluteUri;
  }

  public async Task<IEnumerable<string>> SaveAsync(SaveManyBlobRequest saveManyBlobRequest)
  {
    var blobUriList = new List<string>();

    foreach (var blob in saveManyBlobRequest.Blobs)
    {
       blobUriList.Add(await SaveAsync(new SaveBlobRequest
      {
        File = blob,
        Container = saveManyBlobRequest.Container
      }));
    }

    return blobUriList;
  }
}
