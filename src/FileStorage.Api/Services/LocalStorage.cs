using FileStorage.Api.Data.Transfer;

namespace FileStorage.Api.Services;

public class LocalStorage : IStorage
{
    private static readonly DirectoryInfo TargetDirectory = new DirectoryInfo("./store");

    static LocalStorage()
    {
        if (!TargetDirectory.Exists)
        {
            TargetDirectory.Create();
        }
    }

    public async Task<UploadResponce> UploadAsync(UploadRequest request)
    {
        var filePath = Path.Combine(TargetDirectory.FullName, request.Name);
        var jsonPath = Path.Combine(TargetDirectory.FullName, request.Name + ".json");

        var tasks = new[]
        {
            File.WriteAllBytesAsync(filePath, request.File),
            File.WriteAllBytesAsync(jsonPath, request.Json)
        };

        await Task.WhenAll(tasks);

        return new UploadResponce(filePath);
    }
}