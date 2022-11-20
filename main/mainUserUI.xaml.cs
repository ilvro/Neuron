using Neuron;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using System.Data;

namespace Neuron_V2.main
{
    /// <summary>
    /// Interaction logic for mainUserUI.xaml
    /// </summary>
    public partial class mainUserUI : Window
    {
        public mainUserUI()
        {

            InitializeComponent();
            startUp();
            new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
        }

        public void startUp()
        {
            //new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
            this.Title = "Neuron";
            ObservableCollection<Account> accounts = new ObservableCollection<Account>();

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
                if (fileName.Contains("_accountData"))
                {
                    if (NeuronF.getJsonProperty(fileName, "Username").Length > 0)
                    {
                        // theres probably a much better way to do this
                        accounts.Add(new Account { Username = NeuronF.getJsonProperty(fileName, "Username"), Description = NeuronF.getJsonProperty(fileName, "Description"), lastPlace = NeuronF.getJsonProperty(fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(fileName, "Cookie") });
                    }
                    if (NeuronF.getJsonProperty(fileName, "relaunchWhenClosed").ToString().Length > 0) // reset
                    {

                        string json = File.ReadAllText(fileName);
                        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        jsonObj[0]["relaunchWhenClosed"] = false;
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(fileName, output);
                    }

                }

                if (fileName.Contains(".accountsBeingManaged"))
                {
                    File.Delete(fileName);
                    using (StreamWriter sw = File.AppendText(settingsPath + ".accountsBeingManaged.txt")) // creates the file
                    {
                        sw.WriteLine(":");
                        sw.Dispose();
                        sw.Close();
                    }
                }

                if (fileName.Contains(".lastLaunched="))
                {
                    File.Delete(fileName);
                }
                if (fileName.Contains(".reopening=true"))
                {
                    try
                    {
                        File.Move(fileName, ".reopening=false");
                    }
                    catch
                    {
                        //lol
                    }
                }
            }
            if (File.Exists(settingsPath + "roblox-logs-ignore") == false)
            {
                using (StreamWriter sw = File.AppendText(settingsPath + "roblox-logs-ignore.txt")) // creates the file
                {
                    sw.WriteLine("filename");
                    sw.Dispose();
                    sw.Close();
                }
            }
            else
            {
                File.Delete(settingsPath + "roblox-logs-ignore");
                using (StreamWriter sw = File.AppendText(settingsPath + "roblox-logs-ignore.txt")) // creates the file
                {
                    sw.WriteLine("filenametEst");
                    sw.Dispose();
                    sw.Close();
                }
            }
            if (File.Exists(settingsPath + ".reopening=false") == false)
            {
                using (StreamWriter sw = File.AppendText(settingsPath + ".reopening=false.txt")) // creates the file
                {
                    sw.WriteLine("");
                    sw.Dispose();
                    sw.Close();
                }
            }

            if (File.Exists(settingsPath + ".switch=false") == false && File.Exists(settingsPath + ".switch=true") == false)
            {
                using (StreamWriter sw = File.AppendText(settingsPath + ".switch=false.txt")) // creates the file
                {
                    sw.WriteLine("");
                    sw.Dispose();
                    sw.Close();
                }
            }
            if (File.Exists(settingsPath + "connData.json"))
            {
                string json = File.ReadAllText(settingsPath + "connData.json");
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj[0]["isSwitching"] = false;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(settingsPath + "connData.json", output);
            }
            if (File.Exists(settingsPath + "connData.json") == false)
            {
                List<NeuronF.ConnData> data = new List<NeuronF.ConnData>();
                data.Add(new NeuronF.ConnData()
                {
                    webhookLink = "",
                    isSwitching = false

                });

                string json = System.Text.Json.JsonSerializer.Serialize(data);
                File.WriteAllText(settingsPath + "connData.json", json);
            }

            string webhookLink = NeuronF.getJsonProperty(settingsPath + "connData.json", "webhookLink");
            if (webhookLink.Length > 10)
            {
                webhookTextAndBorder.Text = webhookLink;
            }


            var robloxLogs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\logs"; // clear roblox logs
            if (Directory.Exists(robloxLogs))
            {
                foreach (var item in Directory.GetFiles(robloxLogs))
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch // maybe the file is open?
                    {

                    }
                }
            }


            accountsDataGrid.ItemsSource = accounts;
            var RobloxF = new NeuronF.RobloxFunctions();
            Thread thread = new Thread(RobloxF.checkVPN);
            thread.Start();


        }

        public void refreshGrid()
        {
            ObservableCollection<Account> accounts = new ObservableCollection<Account>();
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
                if (fileName.Contains("_accountData"))
                {
                    if (NeuronF.getJsonProperty(fileName, "Username").Length > 0)
                    {
                        // theres probably a much better way to do this
                        accounts.Add(new Account { Username = NeuronF.getJsonProperty(fileName, "Username"), Description = NeuronF.getJsonProperty(fileName, "Description"), lastPlace = NeuronF.getJsonProperty(fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(fileName, "Cookie") });
                    }
                }
            }


            accountsDataGrid.ItemsSource = accounts;
        }


        private void addAccountBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Browser browser = new Browser();
            browser.InitializeBrowser("https://www.roblox.com/login", true);
            browser.ShowDialog();
            refreshGrid();
        }


        private void playBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
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
                    DataGridRow row = (DataGridRow)vis;
                    customInputAndButtonMessageBox messageBox = new customInputAndButtonMessageBox
                    {
                        Title = ((Account)row.DataContext).Username
                    };

                    // check if the account is already open

                    var tempLines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");
                    bool isOpen = false;
                    bool isOpening = false;
                    foreach (var item in tempLines)
                    {
                        if (item.Contains(messageBox.Title))
                        {
                            MessageBox.Show("This account is already open.");
                            isOpen = true;
                            break;
                        }
                    }

                    //  check if theres an account relaunching
                    if (NeuronF.getOpeningState())
                    {
                        MessageBox.Show("An account is already being opened.");
                        isOpening = true;
                    }
                    if (isOpen == false && isOpening == false)
                    {
                        // use last game id, if there is one
                        messageBox.gameIDLine.Text = NeuronF.getJsonProperty(settingsPath + ((Account)row.DataContext).Username + "_accountData.json", "lastPlace");
                        messageBox.ShowDialog();

                        NeuronF.setOpeningState(true);
                        var RobloxF = new NeuronF.RobloxFunctions();
                        while (RobloxF.getActiveUnmanagedInstances().Count == 0)
                        {
                            //
                        }
                        System.Threading.Thread.Sleep(1100);
                        //MessageBox.Show("there are " + RobloxF.getActiveUnmanagedInstances().Count + " unmanaged instances");

                        // get last launched account
                        string[] files = Directory.GetFiles(settingsPath);
                        foreach (var item in files)
                        {
                            if (item.Contains(".lastLaunched="))
                            {

                                string name = NeuronF.getBetween(item, "=", ".");
                                string toRemove = "";


                                var lines = File.ReadLines(settingsPath + ".accountsBeingManaged.txt");
                                foreach (var line in lines)
                                {
                                    if (line.Contains(name))
                                    {
                                        toRemove = line;
                                    }
                                }



                                File.WriteAllLines(settingsPath + ".accountsBeingManaged.txt", File.ReadLines(settingsPath + ".accountsBeingManaged.txt").Where(l => l != toRemove).ToList());

                                using (StreamWriter sw = File.AppendText(settingsPath + ".accountsBeingManaged.txt"))
                                {
                                    sw.WriteLine(name + ":" + RobloxF.getLastActiveInstance().Id.ToString());
                                    sw.Dispose();
                                    sw.Close();
                                }
                                //MessageBox.Show("launched account " + name + " on pid " + RobloxF.getLastActiveInstance().Id.ToString());
                                int isManaging = RobloxF.getActiveManagedInstances().Count - 1;
                                Process currentProcess = RobloxF.getLastActiveInstance();
                                currentProcess.EnableRaisingEvents = true;
                                currentProcess.Exited += (s, ea) => RobloxF.onInstanceExited(sender, e, name);
                                NeuronF.setOpeningState(false);


                            }
                        }

                        /*
                        List<Process> processList = getActiveInstances();
                        foreach (var process in processList)
                        {
                            process.EnableRaisingEvents = true;
                            process.Exited += new EventHandler(process_Exited);
                            Console.WriteLine(process.ProcessName);
                        }
                        */
                    }


                }
            }
        }



        private void deleteBtn_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    DataGridRow row = (DataGridRow)vis;
                    string username = ((Account)row.DataContext).Username;

                    MessageBoxResult dlgResult = MessageBox.Show("Are you sure you want to delete this account? (" + username + ")", "Neuron", MessageBoxButton.YesNo);
                    if (dlgResult.ToString() == "Yes")
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
                            if (fileName.Contains("_accountData") && fileName.Contains(username))
                            {
                                File.Delete(fileName);
                                refreshGrid();
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void relaunchToggle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(sender.ToString());
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    DataGridRow row = (DataGridRow)vis;
                    string username = ((Account)row.DataContext).Username;
                    bool toggled = Convert.ToBoolean(sender.ToString().Substring(sender.ToString().LastIndexOf(":") + 1));
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
                    string fileName = settingsPath + username + "_accountData.json";

                    string json = File.ReadAllText(fileName);
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    jsonObj[0]["relaunchWhenClosed"] = !toggled;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(fileName, output);
                }
            }
        }

        private void switchToggle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool toggled = Convert.ToBoolean(sender.ToString().Substring(sender.ToString().LastIndexOf(":") + 1));
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
                if (fileName.Contains(".switch="))
                {
                    File.Delete(fileName);
                    using (StreamWriter sw = File.AppendText(settingsPath + ".switch=" + (!toggled).ToString().ToLower() + ".txt"))
                    {
                        sw.WriteLine("");
                        sw.Dispose();
                        sw.Close();
                    }
                }
            }
            //
        }
        private void webhookTextAndBorder_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var changes = e.Changes.Last();
            if (IDText.Text.Length > 0)
            {
                IDText.Opacity = 0;
            }
            else
            {
                IDText.Opacity = 0.3;
            }
            string dataPath = NeuronF.currentPath() + @"\main\settings\connData.json";
            List<NeuronF.ConnData> data = new List<NeuronF.ConnData>();
            data.Add(new NeuronF.ConnData()
            {
                webhookLink = webhookTextAndBorder.Text,
                isSwitching = Convert.ToBoolean(NeuronF.getJsonProperty(dataPath, "isSwitching"))

            });

            string json = System.Text.Json.JsonSerializer.Serialize(data);
            File.Delete(dataPath);
            File.WriteAllText(dataPath, json);
        }
    }
}