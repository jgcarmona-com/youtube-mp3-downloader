using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeMp3Downloader.Services;

namespace YoutubeMp3Downloader.WPF.ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private YoutubeToMp3Service _service;
        private StringWriterExt _writer;

        public MainViewModel()
        {
            Url = "";
            ConsoleOutput = "";
            _service = new YoutubeToMp3Service();
            DownloadMp3Command = new RelayCommand(DownloadMp3);

            _writer = new StringWriterExt(true); // true = AutoFlush
            _writer.Flushed += new StringWriterExt.FlushedEventHandler(WriterFlushed);
            TextWriter stdout = Console.Out;
            Console.SetOut(_writer);
        }

        public string Url { get; set; }
        public string ConsoleOutput { get; set; }

        public IRelayCommand DownloadMp3Command { get; }

        private async void DownloadMp3()
        {
            await Task.Run(() =>
            {
                _service.DownloadMp3ByUrl(Url);
            });
            Url = "";
            OnPropertyChanged("Url");
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
