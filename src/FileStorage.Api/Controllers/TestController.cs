using FileStorage.Api.Data.Transfer;
using FileStorage.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Api;

[ApiController] [Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly IStorage _storage;

    public TestController(IStorage storage)
    {
        _storage = storage;
    }

    [HttpPost]
    public async Task<ActionResult<UploadResponce>> Upload([FromBody] UploadRequest request)
    { 
        var responce = await _storage.UploadAsync(request);
        return responce;
    }
}
