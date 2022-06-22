using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeMp3Downloader.Services;

namespace YoutubeMp3Downloader.WPF.ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private IYoutubeToMp3Service _service;
        private IConfiguration _configuration;
        private StringWriterExt _writer;

        public MainViewModel(IYoutubeToMp3Service youtubeToMp3Service)
        {
            _service = youtubeToMp3Service;

            Url = "";
            ConsoleOutput = "";
            Version = System.Configuration.ConfigurationManager.AppSettings["AppVersion"];

            DownloadMp3Command = new RelayCommand(DownloadMp3);

            _writer = new StringWriterExt(true); // true = AutoFlush
            _writer.Flushed += new StringWriterExt.FlushedEventHandler(WriterFlushed);
            TextWriter stdout = Console.Out;
            Console.SetOut(_writer);
            DownloadButtonEnabled = true;
        }

        public string Url { get; set; }
        public string ConsoleOutput { get; set; }
        public string Version { get; set; }

        public bool DownloadButtonEnabled { get; set; }

        public IRelayCommand DownloadMp3Command { get; }

        private async void DownloadMp3()
        {
            DownloadButtonEnabled = false;
            OnPropertyChanged("DownloadButtonEnabled");
            await Task.Run(() =>
            {
                Analytics.TrackEvent("Download Mp3 From URL", new Dictionary<string, string> { { "Url", Url } });
                _service.DownloadMp3ByUrl(Url);
            });
            Url = "";
            OnPropertyChanged("Url");
            DownloadButtonEnabled = true;
            OnPropertyChanged("DownloadButtonEnabled");
        }

        void WriterFlushed(object sender, EventArgs args)
        {
            ConsoleOutput = "";
            OnPropertyChanged("ConsoleOutput");
            ConsoleOutput = sender.ToString();
            OnPropertyChanged("ConsoleOutput");
        }
    }
}
