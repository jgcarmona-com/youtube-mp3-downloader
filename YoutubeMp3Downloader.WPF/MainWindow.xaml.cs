using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubeMp3Downloader.Services;

namespace YoutubeMp3Downloader.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringWriterExt _sw;
        private YoutubeToMp3Service _service;

        public string ConsoleOutput { get; set; }

        public string Url { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _service = new YoutubeToMp3Service();
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            var writer = new StringWriterExt(true); // true = AutoFlush
            writer.Flushed += new StringWriterExt.FlushedEventHandler(writer_Flushed);

            TextWriter stdout = Console.Out;
            try
            {
                Console.SetOut(writer);
                _service.DownloadMp3ByUrl(Url);
            }
            finally
            {
                Console.SetOut(stdout);
            }
        }

        void writer_Flushed(object sender, EventArgs args)
        {
            ConsoleOutput += sender.ToString();
         }
    }
}
