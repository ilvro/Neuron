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
            //accounts.Add(new Account { Username = "gui2", Description = "the", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "somecookie" });
            //accounts.Add(new Account { Username = "memoriaee", Description = "description", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });
            //accounts.Add(new Account { Username = "ilvro", Description = "chess cool", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });
            //accounts.Add(new Account { Username = "envysor", Description = "description", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });


            //accountsDataGrid.ItemsSource = accounts;

            string settingsPath = NeuronF.currentPath() + @"\main\settings\";
            foreach (string fileName in Directory.GetFiles(settingsPath))
            {
                if (fileName.Contains("_accountData")) {
                    if (NeuronF.getJsonProperty(settingsPath + fileName, "Username").Length > 0)
                    {
                        // theres probably a much better way to do this
                        accounts.Add(new Account { Username = NeuronF.getJsonProperty(settingsPath + fileName, "Username"), Description = NeuronF.getJsonProperty(settingsPath + fileName, "Description"), lastPlace = NeuronF.getJsonProperty(settingsPath + fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(settingsPath + fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(settingsPath + fileName, "Cookie") });
                    }
                }
            }

            accountsDataGrid.ItemsSource = accounts;

        }
        private void addAccountBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Browser browser = new Browser();
            browser.InitializeBrowser("https://www.roblox.com/login", true);
            browser.ShowDialog();
        }

        private void playBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("play button pressed");
        }

        private void deleteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("delete button pressed");
            MessageBox.Show(accountsDataGrid.CurrentCell.ToString());
        }
    }
}
