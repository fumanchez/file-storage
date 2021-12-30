using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileStorage.Api.Data.Transfer;
using FileStorage.Api.Services;

using Xunit;

namespace FileStorage.Api.Tests;

public class SqliteStorageTest
{
    private readonly UploadRequest[] _requests = GenerateRequests(100);

    private readonly SqliteStorage _storage = new();

    [Fact]
    public async Task FileSaved()
    {
        var request = _requests[0];
        var responce = await SendAsync(request);

        Assert.NotNull(responce);
        Assert.NotNull(responce.Link);

        var returnedRequest = await _storage.GetBackAsync(responce.Link);
        
        Assert.NotNull(returnedRequest.File);
        Assert.NotNull(returnedRequest.Json);

        Assert.Equal(request.File, returnedRequest.File);
        Assert.Equal(request.Json, returnedRequest.Json);
    }

    [Fact]
    public async Task FilesBunchSaved()
    {
        var tasks = from request in _requests 
                    select _storage.UploadAsync(request);

        await Task.WhenAll(tasks);

        var responces = _requests
            .Zip(tasks.Select(task => task.Result))
            .ToDictionary(pair => pair.First, pair => pair.Second);

        foreach (var request in _requests)
        {
            var responce = responces[request];
            Assert.NotNull(responce);
            Assert.NotNull(responce);

            var returnedRequest = await _storage.GetBackAsync(responce.Link);

            Assert.NotNull(returnedRequest.File);
            Assert.NotNull(returnedRequest.Json);

            Assert.Equal(request.File, returnedRequest.File);
            Assert.Equal(request.Json, returnedRequest.Json);
        }
    }

    private Task<UploadResponce>[] SendAsync(UploadRequest[] requests)
    {
        var tasks = new Task<UploadResponce>[requests.Length];
        for (var i = 0; i < requests.Length; ++i)
            tasks[i] = _storage.UploadAsync(requests[i]);
        return tasks;
    }

    private Task<UploadResponce> SendAsync(UploadRequest request)
    {
        return _storage.UploadAsync(request);
    }

    private static UploadRequest[] GenerateRequests(int count, int maxFileSize = 1000 * 1024)
    {
        var random = new Random();
        var requests = new UploadRequest[count];
        for (var i = 0; i < requests.Length; ++i)
        {
            var name = Guid.NewGuid().ToString();
            var size = maxFileSize * i / count;
            var file = new byte[size];
            var json = Encoding.UTF8.GetBytes($"{{ \"name\": \"{name}\", \"size\": {size} }}");

            random.NextBytes(file);

            requests[i] = new UploadRequest(name, file, json);
        }

        return requests;
    }
}