using FileStorage.Api.Data.Transfer;

namespace FileStorage.Api.Services;

public interface IStorage
{
    public Task<UploadResponce> UploadAsync(UploadRequest request);
}