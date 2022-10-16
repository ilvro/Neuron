﻿using Neuron;
using Newtonsoft.Json;
using RestSharp;
using SlavaGu.ConsoleAppLauncher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

// this is the set of general functions -these could be on the mainwindow file but its more organized this way
namespace Neuron_V2.main
{
    class NeuronF
    {
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string randomString(int length)
        {

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string currentDirectory()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        public static string currentPath()
        {
            string neuronPath = currentDirectory();
            for (int i = 0; i < neuronPath.Length; i++)
            {
                if (neuronPath[i].ToString() == "2" && neuronPath[i - 1].ToString() == "V" && neuronPath[i - 8].ToString() == "N") // get actual directory instead of bin/debug. looks ugly, i know
                {
                    neuronPath = neuronPath.Substring(0, i + 1);
                    break;
                }
            }
            return neuronPath;
        }

        public static string checkWhitelist(string key, string discordID, string callType)
        {
            // callType can be "regular" or "login"
            Random random = new Random();
            string hwid = ConsoleApp.Run("cmd", "/c wmic csproduct get uuid").Output.Trim().Replace(" ", "").Replace("UUID", "").Remove(0, 4);
            string hwidHash = NeuronF.sha256(hwid);
            string keyHash = NeuronF.sha256(key);
            string computerNameHash = NeuronF.sha256(System.Environment.MachineName);

            // salt
            int n = random.Next(30, 44);
            int n2 = random.Next(8, 16);
            int corindex = random.Next(8, 16);
            string cor = NeuronF.randomString(n);
            string corHash = NeuronF.sha256(cor);

            // server request. php handles the rest
            var client = new RestClient($"https://primedubs.xyz/servers/neuron_server_new.php?i={corindex}&l={key.Length}&key={NeuronF.randomString(random.Next(16, 22))}&n={n}&cor={cor}&x={keyHash}&hwid={hwidHash}&name={computerNameHash}&u2={n2}&i={corindex}&did={discordID}");
            var authRequest = new RestRequest("", Method.Get);

            var response = client.Execute(authRequest);
            string expectedResponse;
            expectedResponse = corHash.Substring(corindex, n - n2) + hwidHash.Substring(key.Length);
            expectedResponse = expectedResponse.Substring(0, expectedResponse.Length - corindex * 3);

            if (response.Content == expectedResponse)
            {
                return "qptoe5?";
            }
            return "";
        }

        public class AuthData
        {
            public string discordID { get; set; }
            public string Key { get; set; }
        }

        public class UIData
        {
            public string mainTheme { get; set; }
        }

        public static string getJsonProperty(string path, string property) {
            // File.Exists(path + @"\main\login.json
            string neuronPath = currentDirectory();
            for (int i = 0; i < neuronPath.Length; i++)
            {
                if (neuronPath[i].ToString() == "2" && neuronPath[i - 1].ToString() == "V" && neuronPath[i - 8].ToString() == "N") // get actual directory instead of bin/debug. looks ugly, i know
                {
                    neuronPath = neuronPath.Substring(0, i + 1);
                    break;
                }
            }
            string toReturn = "false";
            if (File.Exists(neuronPath + path).ToString() == "True") // check if theyre already logged in
            {
                using (StreamReader r = new StreamReader(neuronPath + @path))
                {
                    string json = r.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    foreach (var item in array)
                    {
                        toReturn = item[property];
                        break;
                    }
                }
            }
            return toReturn;
        }

        public static void writeToJson(string path, string content)
        {
        }

        public static void addAccount(string Cookie)
        {
            string name = Account.getUsername(Cookie);
            if (File.Exists(NeuronF.currentPath() + @"\main\settings\"+name+"_accountData.json").ToString() != "True") // ...
            {
                List<Account.AccountData> data = new List<Account.AccountData>();
                data.Add(new Account.AccountData()
                {
                    Username = name,
                    Description = "",
                    lastPlace = "",
                    relaunchWhenClosed = false,
                    Cookie = Cookie

                });

                string json = System.Text.Json.JsonSerializer.Serialize(data);
                File.WriteAllText(NeuronF.currentPath() + @"\main\settings\" + name + "_accountData.json", json);
            }
            else
            {
                MessageBox.Show("This account is already added.");
            }

            //MessageBox.Show(Application.StartupPath);

            //MessageBox.Show("added account" + name);
        }

    }
}
