using VideoLibrary;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace YoutubeMp3Downloader.Services;

public class YoutubeToMp3Service : IYoutubeToMp3Service
{

    public async void DownloadMp3ByUrl(string url)
    {
        try
        {

            var path = "C:/YoutubeMp3Downloader";
            if (!Directory.Exists(path))
            {
                InstallFfmpeg();
                Directory.CreateDirectory(path);
            }
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
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(inputFileName, mp3FileName);
                Console.WriteLine("Converting to mp3...");
                conversion.OnProgress += async (sender, args) =>
                {
                    await Console.Out.WriteAsync($"\r[{args.Duration}/{args.TotalLength}][{args.Percent}%]");
                };
                await conversion.SetAudioBitrate(320000).SetOutputFormat(Format.mp3).Start();
                Console.WriteLine();
                Console.WriteLine("Saving Audio file");

            }).Wait();
            Console.WriteLine("Deleting Video file");
            File.Delete(inputFileName);
            Console.WriteLine($"[DONE!] => {mp3FileName}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            if (ex.Message.ToLower().Contains("ffmpeg"))
            {
                Console.WriteLine("It looks like FFMPEG is not installed in your system... We are installing it for you");
                InstallFfmpeg();
            }
        }
    }

    private void InstallFfmpeg()
    {
        Task.Run(async () => { await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official); }).Wait();
    }
}
