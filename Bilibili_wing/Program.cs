using System;
using System.Linq;
using System.Collections.Generic;

namespace Bilibili_wing
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                System.Console.WriteLine("Please enter command");
                var cmd = Console.ReadLine();
                var cmds = cmd.Split(' ').Select(c=>c.Trim().ToLower()).ToArray();
                if(cmds[0] == "start")
                {
                    System.Console.WriteLine($"Start live {cmds[1]}");
                    var live = new BiliBiliLive(int.Parse(cmds[1]));
                    StartLive(live);
                    _lives.Add(live);
                }
                else if(cmds[0] == "status")
                {
                    foreach(var live in _lives)
                    {
                        System.Console.WriteLine($"roomId={live.RoomId}\tdownloading={live.IsRunning}");
                    }
                }
                else if(cmds[0] == "stop")
                {
                    _lives.Where(l=>l.RoomId == int.Parse(cmds[1])).FirstOrDefault().Stop();
                }
            }
        }

        static List<BiliBiliLive> _lives = new List<BiliBiliLive>();

        static void StartLive(BiliBiliLive live)
        {
            while(!live.IsAvailed)
                ;
            live.Start();
        } 
    }
}
