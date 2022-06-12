using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeMp3Downloader.UWP
{
    public interface IAppSettings
    {
        string AppVersion { get; }
        string AppCenterSecret { get; }
    }
}
