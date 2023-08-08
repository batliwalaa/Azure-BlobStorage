using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobStorageExample.Services;

public interface IContainerService
{
  Task<IEnumerable<string>> GetAsync();
  Task<bool> DeleteAsync(string container);
}
