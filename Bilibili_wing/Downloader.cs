using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Bilibili_wing
{
    public class Downloader
    {
        private string _url = "";
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private WebClient _client = new WebClient();

        public bool IsRunning
        {
            get
            {
                if (_client == null)
                    return false;
                return _client.IsBusy;
            }
        }

        private bool _requestStop = false;

        public Downloader()
        {
            BiliHttpHelper.AddHeader(_client);
        }

        public Downloader(string url)
        {
            BiliHttpHelper.AddHeader(_client);
            Url = url;
        }

        public void Start(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            System.Console.WriteLine("try to open " + Url);
            _client.DownloadFileAsync(new Uri(Url), filePath);
        }

        public void Stop()
        {
            _client.CancelAsync();
        }
    }
}