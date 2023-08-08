using Microsoft.AspNetCore.Http;

namespace BlobStorageExample.Models.Requests;

public class SaveBlobRequest : BaseRequest
{
  public IFormFile File { get; set; }
}
