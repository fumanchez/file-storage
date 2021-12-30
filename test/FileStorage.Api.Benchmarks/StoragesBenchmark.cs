using System;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using FileStorage.Api.Data.Transfer;
using FileStorage.Api.Services;

namespace FileStorage.Api.Benchmarks;

public class StoragesBenchmark
{
    private readonly UploadRequest[] _requests = GenerateRequests(5);

    private readonly LocalStorage _localStorage = new();
    private readonly SqliteStorage _sqliteStorage = new();

    [Benchmark] public Task Local() => SendAsync(_requests, _localStorage);

    [Benchmark] public Task Sqlite() => SendAsync(_requests, _sqliteStorage);

    private static Task SendAsync(UploadRequest[] requests, IStorage storage)
    {
        var tasks = new Task[requests.Length];
        for (var i = 0; i < requests.Length; ++i)
            tasks[i] = storage.UploadAsync(requests[i]);
        return Task.WhenAll(tasks);
    }

    private static UploadRequest[] GenerateRequests(int count, int maxFileSize = 10000 * 1024)
    {
        var requests = new UploadRequest[count];
        for (var i = 0; i < count; ++i)
        {
            var name = Guid.NewGuid().ToString();
            var size = maxFileSize * (i + 1) / count;
            var file = new byte[size];
            var json = Encoding.UTF8.GetBytes($"{{ \"name\": \"{name}\", \"size\": {size} }}");

            new Random().NextBytes(file);

            requests[i] = new UploadRequest(name, file, json);
        }

        return requests;
    }
}