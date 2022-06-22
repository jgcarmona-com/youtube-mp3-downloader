using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Reflection;
using VideoLibrary;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace YoutubeMp3Downloader.Services;

public class YoutubeToMp3Service : IYoutubeToMp3Service
{
    private const string path = "C:/YoutubeMp3Downloader";
    private readonly string applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";

    public void DownloadMp3ByUrl(string url)
    {
        try
        {
            EnsureOutputFolder();
            Console.Write($"Downloading");
            var folder = path + "/";
            var youtube = YouTube.Default;
            Console.Write(".");
            var vid = youtube.GetVideo(url);
            Console.Write(".");
            var inputFileName = folder + vid.FullName;
            File.WriteAllBytes(folder + vid.FullName, vid.GetBytes());
            Console.WriteLine(".");

            var mp3FileName = folder + vid.FullName.Replace(".mp4", "") + ".mp3";
            if (File.Exists(mp3FileName))
            {
                Console.WriteLine("The file already exists.");
                File.Delete(inputFileName);
                return;
            }
            Task.Run(async () =>
            {
                EnsureFfmpeg().Wait();
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(inputFileName, mp3FileName);
                Console.Write("Converting to mp3...");
                conversion.OnProgress += (sender, args) =>
                {
                    Console.Write($"\r[{args.Duration}/{args.TotalLength}][{args.Percent}%]");
                };
                await conversion.SetAudioBitrate(320000).SetOutputFormat(Format.mp3).Start();
                Console.WriteLine();
                Console.WriteLine("Saving Audio file");

            }).Wait();
            Console.WriteLine("Deleting Video file");
            File.Delete(inputFileName);
            Console.WriteLine($":) DOWNLOADED >>>>\r\n\t {mp3FileName}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Crashes.TrackError(ex);
        }
    }

    private async Task EnsureFfmpeg()
    {
        try
        {
            if (!File.Exists(applicationPath + "/" + "ffmpeg.exe"))
            {
                Analytics.TrackEvent("DOWNLOADING FFMPEG LIBRARY");
                Console.WriteLine("DOWNLOADING FFMPEG LIBRARY");
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full);
                //await SaveFileInAppFolder("https://jgcarmona.blob.core.windows.net/youtube-mp3-downloader/ffmpeg.exe", "ffmpeg.exe");
                //await SaveFileInAppFolder("https://jgcarmona.blob.core.windows.net/youtube-mp3-downloader/ffprobe.exe", "ffprobe.exe");
                Console.WriteLine("DONE FFMPEG LIBRARY");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("IF IT IS A PERMISSIONS ERROR, PLEASE TUN THE APP AS AN ADMISITRATOR ONCE");
            Crashes.TrackError(ex);
            throw;
        }
    }

    private void EnsureOutputFolder()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private async Task SaveFileInAppFolder(string url, string fileName)
    {

        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var fileInfo = new FileInfo(applicationPath + "/" + fileName);
            using var fileStream = fileInfo.OpenWrite();
            await stream.CopyToAsync(fileStream);
        }
        else
        {
            throw new Exception("File not found");
        }
    }
}
