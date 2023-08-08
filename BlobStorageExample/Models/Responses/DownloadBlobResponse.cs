using System.IO;

namespace BlobStorageExample.Models.Responses;

public class DownloadBlobResponse
{
  public DownloadBlobResponse(string name, Stream content, string contentType) =>
        (Name, Content, ContentType) = (name, content, contentType);

  public string Name { get; }

  public Stream Content { get; }

  public string ContentType { get; }
}
