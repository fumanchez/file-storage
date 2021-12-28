using Flurl.Http;

using static System.Console;

var file = File.ReadAllBytes(@"Assets/wallpaper.jpg");
var json = File.ReadAllBytes(@"Assets/wallpaper.json");

var baseUrl = args.Length > 0 ? 
    args[0] :
    "http://localhost:5015";

var responce = await $"{baseUrl}/api/test/upload"
    .PostJsonAsync(new
    {
        Name = "wallpaper.jpg",
        File = file,
        Json = json,
    });

WriteLine(responce.ResponseMessage);
WriteLine(await responce.GetStringAsync());
