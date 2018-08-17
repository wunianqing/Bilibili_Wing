using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Bilibili_wing
{
    public class BiliHttpHelper
    {
        public static async Task<string> GetDownloadLink(int roomId)
        {
            var normalUrl = $"https://api.live.bilibili.com/room/v1/Room/room_init?id={roomId}";
            var client = new HttpClient();
            AddHeader(client);
            HttpResponseMessage response = await client.GetAsync(normalUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var trueId = -1;
            try
            {
                trueId = int.Parse(JObject.Parse(responseBody)["data"]["room_id"].ToString());
            }
            catch
            {
                throw new ArgumentException("Can not get the true roomId. Maybe you type wrong roomId.");
            }

            var trueUrl = $"https://api.live.bilibili.com/api/playurl?cid={trueId}&otype=json&quality=0&platform=web";
            response = await client.GetAsync(trueUrl);
            string downloadLinkJson = await response.Content.ReadAsStringAsync();
            string downloadLink = "";
            try
            {
                downloadLink = JObject.Parse(downloadLinkJson)["durl"][0]["url"].ToString();
            }
            catch
            {
                throw new ArgumentException("Can not get the video download link. Please try again!");
            }
            return downloadLink;
        }

        public static void AddHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Accept", "text/html");
            client.DefaultRequestHeaders.Add("User-Agent", "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.124 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
        }

        public static void AddHeader(WebClient client)
        {
            client.Headers.Add("Accept", "text/html");
            client.Headers.Add("User-Agent", "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.124 Safari/537.36");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
        }
    }
}