using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BlobStorageExample.Models.Requests;

public class SaveManyBlobRequest : BaseRequest
{
  public IEnumerable<IFormFile> Blobs { get; set; }
}

