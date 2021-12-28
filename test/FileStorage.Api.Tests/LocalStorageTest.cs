using System.IO;
using System.Threading.Tasks;

using FileStorage.Api.Data.Transfer;
using FileStorage.Api.Services;

using Xunit;

namespace FileStorage.Api.Tests;

public class LocalStorageTest
{
    private readonly LocalStorage _storage = new();
 
    [Fact]
    public async Task FilesSaved()
    {
        var request = new UploadRequest("abc.bin", 
            File: new byte[] { 1, 2, 3 }, 
            Json: new byte[] { 4, 5, 6 });

        var responce = await _storage.UploadAsync(request);

        Assert.True(File.Exists(responce.Link));

        Assert.Equal(request.File, File.ReadAllBytes(responce.Link));

        Assert.Equal(request.Json, File.ReadAllBytes(responce.Link + ".json"));
    }
}