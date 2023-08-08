using Azure.Storage.Blobs;
using BlobStorageExample.Services;
using Microsoft.Extensions.Azure;
using Moq;

namespace BlobStorageExampleTest.Services;
public abstract class BaseStorageServiceTests : IDisposable
{
  protected bool _disposed;
  protected readonly Mock<IAzureClientFactory<BlobServiceClient>> _mockAzureClientFactory;
  protected readonly Mock<BlobServiceClient> _mockBlobServiceClient;

  public BaseStorageServiceTests()
  {
    _mockAzureClientFactory = new Mock<IAzureClientFactory<BlobServiceClient>>();
    _mockBlobServiceClient = new Mock<BlobServiceClient>();
    _mockAzureClientFactory.Setup(x =>
      x.CreateClient(It.IsAny<string>())).Returns(_mockBlobServiceClient.Object);
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected abstract void Dispose(bool disposing);
}
