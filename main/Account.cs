//using Newtonsoft.Json.Linq;
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

namespace Neuron
{
    class Account
    {
        public string Username;
        public string Cookie;
        private static readonly HttpClient client = new HttpClient();

        public Account(string auser, string acookie)
        {
            Username = auser;
            Cookie = acookie;


        }

        public string validateCookie()
        {
            return "";
        }
        public string getCSRF()
        {

            var client = new RestClient("https://auth.roblox.com/v1/authentication-ticket");
            Uri url = new Uri("https://auth.roblox.com/v1/authentication-ticket");

            var TokenRequest = new RestRequest("", Method.Post);
            client.AddCookie(".ROBLOSECURITY", Cookie, "", url.Host);
            var response = client.Execute(TokenRequest);

            foreach (var h in response.Headers)
            {
                string Token = getBetween(h.ToString(), "x-csrf-token, Value = ", ",");
                if (Token.Length > 1)
                {
                    return Token;
                }
            }
            throw new Exception("Failed to get X-CSRF-Token.");
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
                string Ticket = getBetween(h.ToString(), "rbx-authentication-ticket, Value = ", ",");
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

        public string joinServer(long placeID)
        {
            Random Random = new Random();
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int browserTrackerID = Random.Next(100000000, 999999999);
            long timestamp = (long)t.TotalSeconds;

            Process.Start($"roblox-player:1+launchmode:play+gameinfo:{getAuthTicket()}+launchtime:{timestamp * 1000}+placelauncherurl:https%3A%2F%2Fassetgame.roblox.com%2Fgame%2FPlaceLauncher.ashx%3Frequest%3DRequestGame%26browserTrackerId%3D{browserTrackerID}%26placeId%3D{placeID}%26isPlayTogetherGame%3Dfalse+browsertrackerid:+{browserTrackerID}+robloxLocale:en_us+gameLocale:en_us");
            return "";
        }

        public string addAccount()
        {
            return "";
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public class AccountData
        {
            public string robloxID { get; set; }
            public string discordID { get; set; }
            public string ROBLOSECURITY { get; set; }
            public string lastPlace { get; set; }
            public string Key { get; set; }
        }
    }
}