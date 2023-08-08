using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorageExample.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace BlobStorageExampleTest.Services;

public class ContainerServiceTests : BaseStorageServiceTests
{
  private readonly IContainerService _service;

  public ContainerServiceTests() : base()
  {
    _service = new ContainerService(_mockAzureClientFactory.Object);
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

  public class ContainerServiceConstrutor : ContainerServiceTests
  {
    [Fact]
    public void ContainerService_Should_Instantiate()
    {
      // Assert.
      _service.Should().NotBeNull();
    }
  }

  public class DeleteAsync : ContainerServiceTests
  {
    [Fact]
    public async Task BlobContainerClient_DeleteIfExistsAsync_Should_MarkContainerForDeletion()
    {
      // Arrange.
      Mock<BlobContainerClient> mockBlobContainerClient = new();
      mockBlobContainerClient.Setup(x => x.DeleteIfExistsAsync(
        It.IsAny<BlobRequestConditions>(),
        It.IsAny<CancellationToken>())
      ).ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
        .Returns(mockBlobContainerClient.Object);

      // Act.
      bool result = await _service.DeleteAsync("testcontainer");

      // Assert.
      using (new AssertionScope())
      {
        Assert.True(result);
        _mockBlobServiceClient.Verify(x => x.GetBlobContainerClient(It.IsAny<string>()), Times.Once());
        mockBlobContainerClient.Verify(x =>
          x.DeleteIfExistsAsync(
            It.IsAny<BlobRequestConditions>(),
            It.IsAny<CancellationToken>()
          ), Times.Once());
      }      
    }

    [Fact]
    public async Task BlobContainerClient_DeleteIfExistsAsync_Should_Not_MarkContainerForDeletion()
    {
      // Arrange.
      Mock<BlobContainerClient> mockBlobContainerClient = new();
      mockBlobContainerClient.Setup(x => x.DeleteIfExistsAsync(
        It.IsAny<BlobRequestConditions>(),
        It.IsAny<CancellationToken>())
      ).ReturnsAsync(Response.FromValue(false, new Mock<Response>().Object));
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
        .Returns(mockBlobContainerClient.Object);

      // Act.
      bool result = await _service.DeleteAsync("testcontainer");

      // Assert.
      using (new AssertionScope())
      {
        Assert.False(result);
        _mockBlobServiceClient.Verify(x => x.GetBlobContainerClient(It.IsAny<string>()), Times.Once());
        mockBlobContainerClient.Verify(x =>
          x.DeleteIfExistsAsync(
            It.IsAny<BlobRequestConditions>(),
            It.IsAny<CancellationToken>()
          ), Times.Once());
      }
    }

    [Fact]
    public async Task BlobContainerClient_DeleteIfExistsAsync_Throws_RequestFailedException()
    {
      // Arrange.
      Mock<BlobContainerClient> mockBlobContainerClient = new();
      mockBlobContainerClient.Setup(x => x.DeleteIfExistsAsync(
        It.IsAny<BlobRequestConditions>(),
        It.IsAny<CancellationToken>())
      ).ThrowsAsync(new RequestFailedException("Request failed exception"));
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
        .Returns(mockBlobContainerClient.Object);

      // Act.
      Func<Task> action = () => _service.DeleteAsync("testcontainer");

      // Assert.
      using (new AssertionScope())
      {
        await action.Should().ThrowAsync<RequestFailedException>();
        _mockBlobServiceClient.Verify(x => x.GetBlobContainerClient(It.IsAny<string>()), Times.Once());
        mockBlobContainerClient.Verify(x =>
          x.DeleteIfExistsAsync(
            It.IsAny<BlobRequestConditions>(),
            It.IsAny<CancellationToken>()
          ), Times.Once());
      }
    }
  }

  public class GetAsync : ContainerServiceTests
  {
    [Fact]
    public async Task BlobServiceClient_GetBlobContainersAsync_Should_Return_BlobContainerItems()
    {
      // Arrange.
      AsyncPageable<BlobContainerItem> asyncPageableOfTypeBlobContainerItems = GetBlobContainerItems();
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainersAsync(
          It.IsAny<BlobContainerTraits>(),
          It.IsAny<BlobContainerStates>(),
          It.IsAny<string>(),
          It.IsAny<CancellationToken>())
        )
        .Returns(asyncPageableOfTypeBlobContainerItems);

      // Act.
      IEnumerable<string> result = await _service.GetAsync();

      // Assert.
      using (new AssertionScope())
      {
        result.Should().NotBeEmpty();
        result.Single().Should().Be("test blob");
        _mockBlobServiceClient.Verify(x => x.GetBlobContainersAsync(
          It.IsAny<BlobContainerTraits>(),
          It.IsAny<BlobContainerStates>(),
          It.IsAny<string>(),
          It.IsAny<CancellationToken>()), Times.Once());
      }
    }

    [Fact]
    public async Task BlobServiceClient_GetBlobContainersAsync_When_NoContainers_Should_Return_Empty()
    {
      // Arrange.
      AsyncPageable<BlobContainerItem> asyncPageableOfTypeBlobContainerItems = GetBlobContainerItems(true);
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainersAsync(
          It.IsAny<BlobContainerTraits>(),
          It.IsAny<BlobContainerStates>(),
          It.IsAny<string>(),
          It.IsAny<CancellationToken>())
        )
        .Returns(asyncPageableOfTypeBlobContainerItems);

      // Act.
      IEnumerable<string> result = await _service.GetAsync();

      // Assert.
      using (new AssertionScope())
      {
        result.Should().BeEmpty();
        _mockBlobServiceClient.Verify(x => x.GetBlobContainersAsync(
          It.IsAny<BlobContainerTraits>(),
          It.IsAny<BlobContainerStates>(),
          It.IsAny<string>(),
          It.IsAny<CancellationToken>()), Times.Once());
      }
    }

    [Fact]
    public async Task BlobServiceClient_GetBlobContainersAsync_Throws_RequestFailedException()
    {
      // Arrange.
      _mockBlobServiceClient
        .Setup(x => x.GetBlobContainersAsync(
          It.IsAny<BlobContainerTraits>(),
          It.IsAny<BlobContainerStates>(),
          It.IsAny<string>(),
          It.IsAny<CancellationToken>())
        ).Throws(new RequestFailedException("Request failed exception"));

      // Act.
      Func<Task> action = () => _service.GetAsync();

      // Assert.
      using (new AssertionScope())
      {
        await action.Should().ThrowAsync<RequestFailedException>();
        _mockBlobServiceClient
          .Verify(x => x.GetBlobContainersAsync(
            It.IsAny<BlobContainerTraits>(),
            It.IsAny<BlobContainerStates>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
          Times.Once());
      }
    }

    private static AsyncPageable<BlobContainerItem> GetBlobContainerItems(bool empty = false) 
    {
      List<BlobContainerItem> blobContainerItems = new ();

      if (!empty)
      {
        blobContainerItems.Add(
          BlobsModelFactory.BlobContainerItem("test blob",
          BlobsModelFactory.BlobContainerProperties(DateTimeOffset.Now, ETag.All)));
      }
      
      Page<BlobContainerItem> page = 
        Page<BlobContainerItem>.FromValues(blobContainerItems, null, Mock.Of<Response>());

      return AsyncPageable<BlobContainerItem>.FromPages(new [] { page });
    }
  }
}