using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YoutubeMp3Downloader.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using System.IO;
using YoutubeMp3Downloader.Services;

namespace YoutubeMp3Downloader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {

            var appCenterSecret = ConfigurationManager.AppSettings["AppCenterSecret"];
            Console.WriteLine($"AppCenter Secret: {appCenterSecret}");
            AppCenter.Start(appCenterSecret, typeof(Analytics), typeof(Crashes));

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }


        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IYoutubeToMp3Service,YoutubeToMp3Service>();
        }


        public new static App Current => (App)Application.Current;


        public MainViewModel MainVM => ServiceProvider.GetService<MainViewModel>();
    }
}
