using BlobStorageExample.Models.Requests;
using BlobStorageExample.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobStorageExample.Services;

public interface IBlobService
{
  Task<IEnumerable<string>> GetListAsync(string container);

  Task<string> GetAsync(GetBlobRequest request);

  Task<string> SaveAsync(SaveBlobRequest request);

  Task<IEnumerable<string>> SaveAsync(SaveManyBlobRequest saveBlobRequests);

  Task<bool> DeleteAsync(DeleteBlobRequest request);

  Task<DownloadBlobResponse> DownloadAsync(DownloadBlobRequest downloadBlobRequest);
}
