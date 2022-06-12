using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace YoutubeMp3Downloader.UWP
{
    public class AppSettings : IAppSettings
    {
        public AppSettings()
        {

            var resourceLoader = ResourceLoader.GetForCurrentView("appsettings");
            AppVersion = resourceLoader.GetString(nameof(AppVersion));
            AppCenterSecret = resourceLoader.GetString(nameof(AppCenterSecret));
        }
        public string AppVersion { get; }
        public string AppCenterSecret { get; }
    }
}
