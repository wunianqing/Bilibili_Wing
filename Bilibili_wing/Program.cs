using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using System.Threading;

namespace Bilibili_wing
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.WriteLine("Please enter command");
                var cmd = Console.ReadLine();
                var cmds = cmd.Split(' ').Select(c => c.Trim().ToLower()).ToArray();

                var app = new CommandLineApplication();
                app.HelpOption();
                var startLive = app.Option("-s|--start <RoomID>", "The RoomID", CommandOptionType.SingleValue);
                var statusLive = app.Option("-st|--status", "List all downloading lives", CommandOptionType.NoValue);
                var stopLive = app.Option("-p|--stop <RoomID>", "Stop the specified live", CommandOptionType.SingleValue);
                var exitApp = app.Option("-e|--exit","exit application",CommandOptionType.NoValue);
                app.OnExecute(() =>
                {
                    if (startLive.HasValue())
                    {
                        int roomId;
                        if (int.TryParse(startLive.Value(), out roomId))
                        {
                            var live = new BiliBiliLive(roomId);
                            System.Console.WriteLine("start live");
                            StartLive(live);
                            _lives.Add(live);
                        }
                        else
                        {
                            Console.WriteLine("RoomID has to be a int");
                        }
                    }
                    else if (statusLive.HasValue())
                    {
                        foreach (var live in _lives)
                        {
                            System.Console.WriteLine(live);
                        }
                    }
                    else if (stopLive.HasValue())
                    {
                        int roomId;
                        if (int.TryParse(stopLive.Value(), out roomId))
                        {
                            var live = _lives.FirstOrDefault(l => l.RoomId == roomId);
                            if (live == null)
                            {
                                Console.WriteLine("No such live is downloading, please check and try again");
                                return;
                            }
                            live.Stop();
                        }
                        else
                        {
                            Console.WriteLine("RoomID has to be a int");
                        }
                    }
                    else if(exitApp.HasValue())
                    {
                        Environment.Exit(0);
                    }
                });
                app.Execute(cmds);
            }
        }

        static List<BiliBiliLive> _lives = new List<BiliBiliLive>();

        static void StartLive(BiliBiliLive live)
        {
            while (!live.IsAvailed)
                Thread.Sleep(500);
            live.Start();
        }
    }
}
