using FileStorage.Api.Data.Transfer;

using Microsoft.Data.Sqlite;

namespace FileStorage.Api.Services;

/// <summary>
/// Uses an SQLite database file.
/// </summary>
public partial class SqliteStorage : IStorage
{
    private const string ConnectionString = "Data Source=store.db;Cache=Shared";

    public SqliteStorage()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        new CreateFilesTableQuery(connection).Execute();
    }

    public Task<UploadRequest> GetBackAsync(string link)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var id = long.Parse(link);
        var (name, content, json) = new SelectFileQuery(connection).ExecuteWith(id);

        var request = new UploadRequest(name, content.ToArray(), json.ToArray());
        return Task.FromResult(request);
    }

    public async Task<UploadResponce> UploadAsync(UploadRequest request)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var id = await new InsertFileQuery(connection).ExecuteWith(request.Name, request.File, request.Json);
        var responce = new UploadResponce(id.ToString(), true);
        return responce;
    }
}