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
        private bool _requestStop = false;
        private string _status;
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

        public string Status
        {
            get { return _status; }
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
        public async void Start(string filePath)
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
            _status = string.Format("{0}    downloaded {1} of {2} bytes. {3} % complete...", 
                            (string)e.UserState, 
                            e.BytesReceived, 
                            e.TotalBytesToReceive,
                            e.ProgressPercentage);
        }

        public void Stop()
        {
            _requestStop = false;
        }
        #endregion
    }
}