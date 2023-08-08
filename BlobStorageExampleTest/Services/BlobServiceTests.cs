using BlobStorageExample.Services;
using FluentAssertions;

namespace BlobStorageExampleTest.Services;

public class BlobServiceTests : BaseStorageServiceTests
{
  private readonly IBlobService _service;

  public BlobServiceTests() : base()
  {
    _service = new BlobService(_mockAzureClientFactory.Object);
  }

  protected override void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
      }
      _disposed = true;
    }
  }

  public class BlobServiceConstrutor : BlobServiceTests
  {
    [Fact]
    public void BlobService_Should_Instantiate()
    {
      // Assert.
      _service.Should().NotBeNull();
    }
  }

  public class GetListAsync : BlobServiceTests 
  { 
  }
}
