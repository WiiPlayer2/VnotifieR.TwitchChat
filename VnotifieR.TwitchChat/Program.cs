using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace VnotifieR.TwitchChat
{
    class Program
    {
        private static Ini ini;

        static void Main(string[] args)
        {
            ini = new Ini("./config.ini");

            var creds = new ConnectionCredentials(ini.GetValue("username"), ini.GetValue("token"));
            var client = new TwitchClient();
            client.Initialize(creds, ini.GetValue("username"));

            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnLeftChannel += Client_OnLeftChannel;
            client.OnMessageReceived += Client_OnMessageReceived;

            client.Connect();
            Thread.Sleep(-1);
        }

        private static void Client_OnLeftChannel(object sender, TwitchLib.Client.Events.OnLeftChannelArgs e)
        {
            SendMessage("SYSTEM", "left channel");
        }

        private static void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            SendMessage(e.ChatMessage.Username, e.ChatMessage.Message);
        }

        private static void Client_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            SendMessage("SYSTEM", "joined channel");
        }

        private static void SendMessage(string title, string content)
        {
            var request = WebRequest.CreateHttp(ini.GetValue("url"));
            request.Method = "POST";
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonConvert.SerializeObject(new Notification
                {
                    Title = title,
                    Content = content,
                }));
                writer.Flush();
            }
            try
            {
                request.GetResponse();
            }
            catch { }
        }
    }
}
