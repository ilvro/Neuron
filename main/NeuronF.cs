using Neuron;
using Newtonsoft.Json;
using RestSharp;
using SlavaGu.ConsoleAppLauncher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
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

        public static string checkWhitelist(string key, string discordID, string callType) // not used anymore since its free now
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


        public class ProcessData
        {
            public string Username { get; set; }
            public string PID { get; set; }
        }

        public class ConnData
        {
            public string webhookLink { get; set; }
            public bool isSwitching { get; set; }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system.");
        }

        public static string getJsonProperty(string path, string property)
        {
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
            if (File.Exists(neuronPath + path).ToString() == "True" || File.Exists(path).ToString() == "True") // check if theyre already logged in
            {
                try
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
                catch
                {
                    using (StreamReader r = new StreamReader(@path))
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
            }
            return toReturn;
        }

        public static void writeToJson(string path, string content)
        {

        }

        public static void addAccount(string Cookie)
        {
            string name = Account.getUsername(Cookie);
            if (File.Exists(NeuronF.currentPath() + @"\main\settings\" + name + "_accountData.json").ToString() != "True") // ...
            {
                List<Account.AccountData> data = new List<Account.AccountData>();
                data.Add(new Account.AccountData()
                {
                    Username = name,
                    Description = "",
                    lastPlace = "",
                    relaunchWhenClosed = false,
                    Cookie = Cookie,
                    lastMethod = ""

                });

                string json = System.Text.Json.JsonSerializer.Serialize(data);
                File.WriteAllText(NeuronF.currentPath() + @"\main\settings\" + name + "_accountData.json", json);
            }
            else
            {
                MessageBox.Show("This account is already added.");
            }


        }

        public static bool getOpeningState()
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
            bool reopening = false;
            foreach (string fileName in Directory.GetFiles(settingsPath))
            {
                if (fileName.Contains(".reopening="))
                {
                    reopening = bool.Parse(getBetween(fileName, "=", ".txt"));
                    break;
                }
            }
            return reopening;
        }

        public static bool getSwitchState()
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
            bool switchState = false;
            foreach (string fileName in Directory.GetFiles(settingsPath))
            {
                if (fileName.Contains(".switch="))
                {
                    switchState = bool.Parse(getBetween(fileName, "=", ".txt"));
                    break;
                }
            }
            return switchState;
        }


        public static void setOpeningState(bool reopening)
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
            foreach (string fileName in Directory.GetFiles(settingsPath))
            {
                if (fileName.Contains(".reopening="))
                {
                    File.Delete(fileName);
                    using (StreamWriter sw = File.AppendText(settingsPath + ".reopening=" + reopening.ToString().ToLower() + ".txt"))
                    { // creates the file
                        sw.WriteLine("");
                        sw.Dispose();
                        sw.Close();
                    }
                }
            }
        }

    public class RobloxFunctions
        {
            public bool isRobloxActive()
            {
                bool found = false;
                Process[] processCollection = Process.GetProcesses();
                foreach (Process p in processCollection)
                {
                    if (p.ProcessName == "RobloxPlayerBeta")
                    {
                        found = true;
                        break;
                    }
                }
                return found;
            }


            public List<Process> getActiveInstances()
            {
                List<Process> processList = new List<Process>();
                Process[] processCollection = Process.GetProcesses();
                foreach (Process p in processCollection)
                {
                    if (p.ProcessName == "RobloxPlayerBeta" && (int)p.MainWindowHandle != 0)
                    {
                        processList.Add(p);
                    }
                }
                return processList;
            }

            public List<Process> getActiveManagedInstances()
            {
                List<Process> processList = new List<Process>();
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
                var lines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");
                foreach (var item in lines)
                {
                    string processId = item.Substring(item.LastIndexOf(":") + 1);
                    if (processId.Length > 0)
                    {
                        try
                        {
                            Process process = System.Diagnostics.Process.GetProcessById(Int32.Parse(processId));
                            if ((int)process.MainWindowHandle != 0)
                            {
                                processList.Add(process);
                            }
                        }
                        catch
                        {
                            // gotta do this because it says "Process with an id of ____ is not running" while waiting for it to reopen
                        }
                    }


                }
                processList.Add(System.Diagnostics.Process.GetProcessById(0));
                return processList;
            }

            public List<Process> getActiveUnmanagedInstances()
            {
                List<Process> processList = new List<Process>();
                Process[] processCollection = Process.GetProcesses();
                foreach (Process p in processCollection)
                {
                    if (p.ProcessName == "RobloxPlayerBeta" && (int)p.MainWindowHandle != 0 && isInstanceManaged(p) == false)
                    {
                        processList.Add(p);
                    }
                }
                return processList;
            }



            public Process getLastActiveInstance()
            {
                return getActiveInstances().OrderBy(p => p.StartTime).Last();
            }


            public Process getLastActiveManagedInstance()
            {
                return getActiveManagedInstances().OrderBy(p => p.StartTime).Last();
            }


            public Process getLastUnmanagedActiveInstance()
            {
                //return getActiveUnmanagedInstances().OrderBy(p => p.StartTime).Last();        doesnt work for some reason
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
                var tempLines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");
                Process[] ProcessCollection = Process.GetProcesses();
                List<Process> processList = new List<Process>();
                foreach (var item in ProcessCollection)
                {
                    if (item.ProcessName == "RobloxPlayerBeta.exe" && tempLines.Contains(item.Id.ToString()) && (int)item.MainWindowHandle != 0)
                    {
                        processList.Add(item);
                    }
                }
                processList.Add(System.Diagnostics.Process.GetProcessById(0));
                return processList.OrderBy(p => p.StartTime).Last();
            }


            public bool isInstanceActive(Process p)
            {
                return (int)p.MainWindowHandle != 0;
            }

            public bool isInstanceManaged(Process p)
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
                var tempLines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");

                bool found = false;
                foreach (var item in tempLines)
                {
                    if (item.Contains(p.Id.ToString()))
                    {
                        found = true;
                    }
                }
                return found;
            }


            public void killInactiveInstances()
            {
                Process[] processCollection = Process.GetProcesses();
                foreach (Process p in processCollection)
                {
                    if (p.ProcessName == "RobloxPlayerBeta" && (int)p.MainWindowHandle == 0)
                    {
                        p.Kill();
                    }
                }
            }

            public void onInstanceExited(object sender, EventArgs e, string name)
            {
                //MessageBox.Show("called oninstanceexited on " + name);
                while (getOpeningState() == true)
                {
                    // wait
                }
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
                string toRemove = "";
                var lines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");
                foreach (var line in lines)
                {
                    if (line.Contains(name))
                    {
                        toRemove = line;
                    }
                }
                File.WriteAllLines(settingsPath + ".accountsBeingManaged.txt", File.ReadLines(settingsPath + ".accountsBeingManaged.txt").Where(l => l != toRemove).ToList()); // removes from managed list
                string accountPath = settingsPath + name + "_accountData.json";
                string joinMethod = getJsonProperty(accountPath, "lastMethod");
                if (getJsonProperty(accountPath, "relaunchWhenClosed") == "True")
                {
                    if (joinMethod == "joinServer")
                    {
                        setOpeningState(true);
                        new Account { Username = getJsonProperty(accountPath, "Username"), Description = getJsonProperty(accountPath, "Description"), lastPlace = getJsonProperty(accountPath, "lastPlace"), relaunchWhenClosed = getJsonProperty(accountPath, "relaunchWhenClosed"), Cookie = getJsonProperty(accountPath, "Cookie") }.joinServer((long)Convert.ToInt64(getJsonProperty(accountPath, "lastPlace")));
                        while (getActiveUnmanagedInstances().Count == 0)
                        {
                            //
                        }
                        System.Threading.Thread.Sleep(1100);
                        //MessageBox.Show("there are " + getActiveUnmanagedInstances().Count + " unmanaged instances");

                        // get last launched account
                        string[] files = Directory.GetFiles(settingsPath);
                        foreach (var item in files)
                        {
                            if (item.Contains(".lastLaunched="))
                            {

                                using (StreamWriter sw = File.AppendText(settingsPath + ".accountsBeingManaged.txt"))
                                {
                                    sw.WriteLine(name + ":" + getLastActiveInstance().Id.ToString());
                                    sw.Close();
                                }
                                //MessageBox.Show("launched account " + name + " on pid " + RobloxF.getLastActiveInstance().Id.ToString());
                                int isManaging = getActiveManagedInstances().Count - 1;
                                Process currentProcess = getLastActiveInstance();
                                currentProcess.EnableRaisingEvents = true;
                                //MessageBox.Show(currentProcess.StartTime.ToString() + " | IsInstanceManaged=" + isInstanceManaged(currentProcess).ToString() + " (" + currentProcess.Id + ")" + " | ManagedInstances=" + isManaging);


                                //currentProcess.Exited += new EventHandler(RobloxF.onInstanceExited);
                                NeuronF.setOpeningState(false);
                                currentProcess.Exited += (s, ea) => onInstanceExited(sender, e, name);
                                setOpeningState(false);
                            }
                        }

                    }
                }

            }

            public void disconnectVPN()
            {
                //
            }

            public void checkVPN()
            {
                //string oldIP = GetLocalIPAddress();
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
                bool isSwitching = Convert.ToBoolean(NeuronF.getJsonProperty(dataPath, "isSwitching"));
                bool switched = false;
                int count = 0;
                while (isSwitching)
                {
                    System.Threading.Thread.Sleep(5000);
                }
                var robloxLogs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\logs";
                
                if (Directory.Exists(robloxLogs))
                {
                    foreach (var item in Directory.GetFiles(robloxLogs))
                    {
                        if (switched == false)
                        {
                            using (var stream = new FileStream(
                               item,
                               FileMode.Open,
                               FileAccess.Read,
                               FileShare.ReadWrite))
                            {
                            using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    bool contains = false;
                                    bool switching = Convert.ToBoolean(getJsonProperty(dataPath, "isSwitching"));
                                    if (streamReader.ReadToEnd().Contains("Sending disconnect with reason: 268").ToString() == "True" && switching == false)
                                    {
                                        //var tempLines = File.ReadLines(settingsPath + "roblox-logs-ignore.txt");

                                        System.IO.FileStream fs = new System.IO.FileStream(settingsPath + "roblox-logs-ignore.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                                        System.IO.StreamReader sr = new System.IO.StreamReader(fs);

                                        List<String> lst = new List<string>();

                                        while (!sr.EndOfStream) {
                                            if (sr.ReadLine().Contains(item.ToString()))
                                            {
                                                contains = true;
                                            }
                                        }
                                        if (contains == false)
                                        {
                                            System.Diagnostics.Process processt = new System.Diagnostics.Process();
                                            System.Diagnostics.ProcessStartInfo startInfot = new System.Diagnostics.ProcessStartInfo();
                                            startInfot.WorkingDirectory = "C:\\Program Files\\NordVPN";
                                            startInfot.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                            startInfot.FileName = "cmd.exe";
                                            startInfot.Arguments = "/C nordvpn -d";
                                            processt.StartInfo = startInfot;
                                            processt.Start();

                                            //MessageBox.Show("found unexpected");
                                            //MessageBox.Show(item.ToString());
                                            string[] possible_paths =
                                            {
                                                    "C:\\Program Files\\NordVPN",
                                                    "C:\\NordVPN",
                                                    "D:\\NordVPN",

                                                    "C:\\Program Files\\Private Internet Access",
                                                    "C:\\Program Files (x86)\\Private Internet Access",
                                                    "C:\\Private Internet Access",
                                                    "D:\\Private Internet Access",

                                                    "C:\\Program Files\\OpenVPN\\bin"
                                            };

                                            foreach (var path in possible_paths)
                                            {
                                                if (Directory.Exists(path) || File.Exists(path))
                                                {
                                                    if (path.Contains("NordVPN"))
                                                    {
                                                        string oldIP = GetLocalIPAddress();

                                                        List<NeuronF.ConnData> data = new List<NeuronF.ConnData>(); // change isSwitching to true
                                                        data.Add(new NeuronF.ConnData()
                                                        {
                                                            webhookLink = getJsonProperty(dataPath, "webhookLink"),
                                                            isSwitching = true

                                                        });

                                                        string json = System.Text.Json.JsonSerializer.Serialize(data);
                                                        File.Delete(dataPath);
                                                        File.WriteAllText(dataPath, json);
                                                        //

                                                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                                                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                                        startInfo.WorkingDirectory = "C:\\Program Files\\NordVPN";
                                                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                                        startInfo.FileName = "cmd.exe";
                                                        startInfo.Arguments = "/C nordvpn -c";
                                                        process.StartInfo = startInfo;
                                                        process.Start();

                                                        // wait until its done changing vpns
                                                        while (oldIP == GetLocalIPAddress())
                                                        {
                                                            if (count >= 10 || oldIP != GetLocalIPAddress())
                                                            {
                                                                //MessageBox.Show("broke loop");
                                                                break;
                                                            }
                                                            count++;
                                                            //MessageBox.Show(count.ToString());
                                                            System.Threading.Thread.Sleep(6000);
                                                        }
                                                        System.Threading.Thread.Sleep(2000);
                                                        // broke loop, means the old ip is not the same as the current one (changed)     or it failed to connect
                                                        if (oldIP == GetLocalIPAddress())
                                                        {
                                                            //same ip, failed to connect
                                                            //MessageBox.Show("same ip, failed to connect (" + oldIP + " || " + GetLocalIPAddress() + ")");
                                                            string json2 = File.ReadAllText(dataPath);
                                                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json2);
                                                            jsonObj[0]["isSwitching"] = false;
                                                            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                                                            File.WriteAllText(dataPath, output);
                                                        }
                                                        else
                                                        {

                                                            if (Convert.ToBoolean(getJsonProperty(dataPath, "isSwitching")))
                                                            {
                                                                //MessageBox.Show("writing " + item.ToString() + " to path " + settingsPath + "roblox-logs-ignore.txt (File.Exists = " + File.Exists(settingsPath + "roblox-logs-ignore.txt") + ")");
                                                                // put the filename in roblox-logs-ignore so it doesnt check the same file twice
                                                                using (StreamWriter sw = File.AppendText(settingsPath + "roblox-logs-ignore.txt")) // creates the file
                                                                {
                                                                    sw.WriteLine(item.ToString());
                                                                    sw.Dispose();
                                                                    sw.Close();
                                                                }

                                                                // send webhook
                                                                string webhookLink = getJsonProperty(dataPath, "webhookLink");

                                                                // change isSwitching to false
                                                                if (File.Exists(dataPath))
                                                                {
                                                                    string json2 = File.ReadAllText(dataPath);
                                                                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json2);
                                                                    jsonObj[0]["isSwitching"] = false;
                                                                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                                                                    File.WriteAllText(dataPath, output);
                                                                }
                                                            }

                                                        }
                                                        break;
                                                    }
                                                    else if (path.Contains("Private Internet Access"))
                                                    {

                                                        // change isSwitching to true
                                                        if (File.Exists(dataPath))
                                                        {
                                                            string json2 = File.ReadAllText(dataPath);
                                                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json2);
                                                            jsonObj[0]["isSwitching"] = true;
                                                            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                                                            File.WriteAllText(dataPath, output);
                                                        }


                                                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                                                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                                        startInfo.WorkingDirectory = path;
                                                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                                        startInfo.FileName = "cmd.exe";
                                                        startInfo.Arguments = "/C \""+path+"\" disconnect";
                                                        process.StartInfo = startInfo;
                                                        process.Start();

                                                        System.Diagnostics.Process process2 = new System.Diagnostics.Process();
                                                        System.Diagnostics.ProcessStartInfo startInfo2 = new System.Diagnostics.ProcessStartInfo();
                                                        startInfo.WorkingDirectory = path;
                                                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                                        startInfo.FileName = "cmd.exe";
                                                        startInfo.Arguments = "/C \"" + path + "\" connect";
                                                        process2.StartInfo = startInfo2;
                                                        process2.Start();
                                                    }
                                                }
                                            }
                                            switched = true;
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
                Thread thread = new Thread(checkVPN);
                thread.Start();
            }
        }
    }
}