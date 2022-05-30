using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YoutubeMp3Downloader.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace YoutubeMp3Downloader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainViewModel>();

            return services.BuildServiceProvider();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }


        public MainViewModel MainVM => Services.GetService<MainViewModel>();
    }
}
