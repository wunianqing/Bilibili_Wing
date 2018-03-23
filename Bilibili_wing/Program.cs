using System;
using System.Net;

namespace Bilibili_wing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Bilibili_wing.BiliHttpHelper.GetDownloadLink(10112).Result);
        }
    }
}
