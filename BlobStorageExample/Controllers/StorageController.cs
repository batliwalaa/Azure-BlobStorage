using BlobStorageExample.Models.Requests;
using BlobStorageExample.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BlobStorageExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
  private readonly IContainerService _containerService;
  private readonly IBlobService _blobService;

  public StorageController(
    IContainerService containerService,
    IBlobService blobService) => (_containerService, _blobService) = (containerService, blobService);

  [HttpDelete("container/{container}")]
  [SwaggerOperation(Tags = new[] { "Container" })]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public async Task<IActionResult> Delete([FromQuery] string container)
  {
    return Ok(await _containerService.DeleteAsync(container));
  }

  [HttpGet("container/all")]
  [SwaggerOperation(Tags = new[] { "Container" })]
  [Produces(MediaTypeNames.Application.Json)]
  public async Task<IActionResult> Get()
  {
    return Ok(await _containerService.GetAsync());
  }

  [HttpGet("blob")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public async Task<IActionResult> Get([FromQuery] GetBlobRequest getBlobRequest) =>
    Ok(await _blobService.GetAsync(getBlobRequest));

  [HttpGet("blob/download")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  [ProducesResponseType(typeof(File), 200)]
  public async Task<IActionResult> Download([FromQuery] DownloadBlobRequest downloadBlobRequest)
  {
    var downloadBlobResponse = await _blobService.DownloadAsync(downloadBlobRequest);

    return File(downloadBlobResponse.Content, downloadBlobResponse.ContentType, downloadBlobResponse.Name);
  }

  [HttpGet("blob/list")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public async Task<IActionResult> List([FromQuery] string container) =>
    Ok(await _blobService.GetListAsync(container));

  [HttpPost("blob/savemany")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  public async Task<IActionResult> SaveMany([FromForm] SaveManyBlobRequest saveManyBlobRequest) =>
    Ok(await _blobService.SaveAsync(saveManyBlobRequest));

  [HttpPost("blob/save")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  public async Task<IActionResult> Save([FromForm] SaveBlobRequest request)
  {
    return Ok(await _blobService.SaveAsync(request));
  }

  [HttpDelete("blob")]
  [SwaggerOperation(Tags = new[] { "Blob" })]
  public async Task<IActionResult> Delete([FromQuery] DeleteBlobRequest deleteBlobRequest) =>
    Ok(await _blobService.DeleteAsync(deleteBlobRequest));
}
