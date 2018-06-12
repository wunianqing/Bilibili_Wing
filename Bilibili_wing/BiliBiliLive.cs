using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Bilibili_wing
{
    public class BiliBiliLive
    {
        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set
            {
                if (value <= 0) return;
                _roomId = value;
                UpdataDownloadLink();
            }
        }

        private string _downloadLink;
        public string DownloadLink
        {
            get { return _downloadLink; }
        }

        private bool _isAvailed = false;
        public bool IsAvailed
        {
            get { return _isAvailed; }
        }

        private Downloader _download;

        public bool IsRunning
        {
            get
            {
                if (_download == null)
                    return false;
                return _download.IsRunning;
            }
        }

        public BiliBiliLive() { }
        public BiliBiliLive(int roomtId)
        {
            RoomId = roomtId;
        }


        private async void UpdataDownloadLink()
        {
            _isAvailed = false;
            _downloadLink = await BiliHttpHelper.GetDownloadLink(_roomId);
            _isAvailed = true;
        }

        public async void Start()
        {
            if (_download != null) return;
            _download = new Downloader(DownloadLink);
            var directoryPath = $"{Directory.GetCurrentDirectory()}\\{RoomId}";
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            try
            {
                await _download.Start($"{directoryPath}\\{DateTime.Now.ToString("yyMMdd")}.flv");
            }
            catch
            {
                Console.WriteLine($"Live {RoomId} finished.");
            }
        }

        public void Stop()
        {
            _download?.Stop();
            _download = null;
        }

        public override string ToString()
        {
            return $"Room {_roomId} has download {_download.DownloadSize}";
        }
    }
}