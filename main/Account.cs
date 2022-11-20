//using Newtonsoft.Json.Linq;
using Neuron_V2.main;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neuron
{
    public class Account
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public string lastPlace { get; set; }
        public string relaunchWhenClosed { get; set; }
        public string Cookie { get; set; }
        public string lastMethod { get; set; }
        private static readonly HttpClient client = new HttpClient();



        public string validateCookie()
        {
            return "";
        }
        public string getCSRF()
        {

            try
            {
                var client = new RestClient("https://auth.roblox.com/v1/authentication-ticket");
                Uri url = new Uri("https://auth.roblox.com/v1/authentication-ticket");

                var TokenRequest = new RestRequest("", Method.Post);
                client.AddCookie(".ROBLOSECURITY", Cookie, "", url.Host);
                var response = client.Execute(TokenRequest);

                foreach (var h in response.Headers)
                {
                    string Token = NeuronF.getBetween(h.ToString(), "x-csrf-token, Value = ", ",");
                    if (Token.Length > 1)
                    {
                        return Token;
                    }
                }
            }
            catch
            {
                throw new Exception("connection error");
            }
            return "connection error";
            
        }

        public static string getUsername(string Cookie)
        {
            var client = new RestClient("https://users.roblox.com/v1/users/authenticated");
            Uri url = new Uri("https://users.roblox.com/v1/users/authenticated");

            var TokenRequest = new RestRequest("", Method.Get);
            client.AddCookie(".ROBLOSECURITY", Cookie, "", url.Host);
            var response = client.Execute(TokenRequest);
            return NeuronF.getBetween(response.Content, "name\":\"", "\"");
        }

        public string getAuthTicket()
        {
            var client = new RestClient("https://auth.roblox.com/v1/authentication-ticket/");
            Uri url = new Uri("https://auth.roblox.com/v1/authentication-ticket/");

            var TicketRequest = new RestRequest("", Method.Post);

            client.AddCookie(".ROBLOSECURITY", Cookie, "", url.Host);
            TicketRequest.AddHeader("X-CSRF-Token", getCSRF());
            TicketRequest.AddHeader("Referer", "https://www.roblox.com/");
            var response = client.Execute(TicketRequest);
            foreach (var h in response.Headers)
            {
                string Ticket = NeuronF.getBetween(h.ToString(), "rbx-authentication-ticket, Value = ", ",");
                if (Ticket.Length > 1)
                {
                    return Ticket;
                }
            }
            throw new Exception("Failed to get authentication ticket.");
        }

        public void openRoblox()
        {
            Random Random = new Random();
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int LaunchTime = 1;
            int browserTrackerID = Random.Next(100000000, 999999999);
            long timestamp = (long)t.TotalSeconds;
            Process.Start($"roblox-player:1+launchmode:app+gameinfo:{getAuthTicket()}+launchtime:{LaunchTime}+browsertrackerid:{browserTrackerID}+robloxLocale:en_us+gameLocale:en_us");
        }

        public void joinServer(long placeID)
        {
            string settingsPath = NeuronF.currentPath() + @"\main\settings\";
            try
            {
                foreach (string a in Directory.GetFiles(settingsPath))
                {
                    break;
                }
            }
            catch
            {
                settingsPath = settingsPath.Replace("\\Bin\\Debug", "");
            }
            string dataPath = settingsPath + "connData.json";
            bool switching = Convert.ToBoolean(NeuronF.getJsonProperty(dataPath, "isSwitching"));
            if (switching)
            {
                while (switching)
                {
                    // wait until its done changing vpns
                }
            }

            Random Random = new Random();
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            long browserTrackerID = Random.Next(100000000, 999999999);
            long timestamp = (long)t.TotalSeconds;

            //Process.Start($"roblox-player:1+launchmode:play+gameinfo:{getAuthTicket()}+launchtime:{timestamp * 1000}+placelauncherurl:https%3A%2F%2Fassetgame.roblox.com%2Fgame%2FPlaceLauncher.ashx%3Frequest%3DRequestGame%26browserTrackerId%3D{browserTrackerID}%26placeId%3D{placeID}%26isPlayTogetherGame%3Dfalse+browsertrackerid:+{browserTrackerID}+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp");
            // will start the process in python because the method above isnt compatible with synapse for some reason
            //MessageBox.Show($"python {NeuronF.currentPath() + @"\main\joinServer.py"} {getAuthTicket()} {browserTrackerID} {placeID}");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WorkingDirectory = NeuronF.currentPath() + @"\main\";
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C python joinServer.py {getAuthTicket()} {browserTrackerID} {placeID}";
            process.StartInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
        }


        


        public class AccountData
        {
            public string Username { get; set; }
            public string Description { get; set; }
            public string lastPlace { get; set; }
            public bool relaunchWhenClosed { get; set; }
            public string Cookie { get; set; }
            public string lastMethod { get; set; }
        }
    }
}