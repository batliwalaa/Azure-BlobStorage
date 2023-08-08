using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorageExample.Infrastructure;

public static class BlobContainerClientExtension
{
  public static void AddAzureStorageBlobServiceClients(
    this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAzureClients(builder => 
      builder
        .AddBlobServiceClient(configuration.GetConnectionString(Constants.AzureBlobStorage))
        .WithName(Constants.AzureBlobStorage));
  }
}
