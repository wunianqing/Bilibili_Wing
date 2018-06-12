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
        #region Fields
        private string _url = "";
        private WebClient _client = new WebClient();
        private bool _isRunning = false;
        private string _downloadSize;
        #endregion

        #region Properties
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public string DownloadSize
        {
            get { return _downloadSize; }
        }
        #endregion

        #region Constructor
        public Downloader()
        {
            BiliHttpHelper.AddHeader(_client);
        }

        public Downloader(string url)
        {
            BiliHttpHelper.AddHeader(_client);
            Url = url;
        }
        #endregion

        #region Methods
        public async Task Start(string filePath)
        {
            _isRunning = true;
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            System.Console.WriteLine("try to open " + Url);
            _client.DownloadProgressChanged += DownloadProgressCallback;

            await _client.DownloadFileTaskAsync(new Uri(Url), filePath);
            _isRunning = false;
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            _downloadSize = string.Format("{0:N0} MB", e.BytesReceived / 1024 / 1024);
        }

        public void Stop()
        {
            _client.CancelAsync();
        }
        #endregion
    }
}