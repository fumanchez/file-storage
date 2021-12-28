namespace FileStorage.Api.Data.Transfer;

public record UploadRequest(string Name, byte[] File, byte[] Json);