using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace YoutubeMp3Downloader.Services;

public class YoutubeToMp3Service
{

    public void DownloadMp3ByUrl(string url)
    {
        try
        {
            Console.WriteLine($"Download {url}");
            var path = "C:/YoutubeMp3Downloader";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var folder = path + "/";
            var youtube = YouTube.Default;
            Console.Write(".");
            var vid = youtube.GetVideo(url);
            Console.Write(".");
            File.WriteAllBytes(folder + vid.FullName, vid.GetBytes());
            Console.Write(".");

            var inputFile = new MediaFile { Filename = folder + vid.FullName };
            var mp3Filename = folder + vid.FullName.Replace(".mp4", "") + ".mp3";
            var outputFile = new MediaFile { Filename = mp3Filename };

            using (var engine = new Engine())
            {
                Console.Write(".");
                engine.GetMetadata(inputFile);
                Console.Write(".");
                engine.Convert(inputFile, outputFile);
                Console.Write(".");
            }
            File.Delete(folder + vid.FullName);
            Console.Write(".");
            Console.WriteLine($"\r\nDownloaded {mp3Filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
