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

        private bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
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

        public async void Start(string filePath)
        {
            _isRunning = true;
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            System.Console.WriteLine("try to open " + Url);
            await _client.DownloadFileTaskAsync(new Uri(Url),filePath);
            _isRunning = false;
        }

        public void Stop()
        {
            _requestStop = false;
        }
    }
}