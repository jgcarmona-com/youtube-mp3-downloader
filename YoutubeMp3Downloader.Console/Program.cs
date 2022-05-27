using YoutubeMp3Downloader.Services;

Console.WriteLine("@---::::::::::::::::::::::::::::::---@\n");
Console.WriteLine("#   Youtube Mp3 Console Downloader   #\r");
Console.WriteLine("@---::::::::::::::::::::::::::::::---@\n");

var quit = false;

var service = new YoutubeToMp3Service();

while (!quit)
{
    Console.WriteLine("Enter a youtube URL or 'quit' to exit:");
    var input = Console.ReadLine();
    if (input == "quit")
    {
        quit = true;
    }
    else if (!string.IsNullOrEmpty(input))
    {
        service.DownloadMp3ByUrl(input);
    }
}

