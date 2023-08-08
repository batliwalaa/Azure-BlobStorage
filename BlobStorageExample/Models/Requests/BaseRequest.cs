namespace BlobStorageExample.Models.Requests;

public abstract class BaseRequest
{
  public string Blob { get; set; } = string.Empty;

  public string Container { get; set; }
}
