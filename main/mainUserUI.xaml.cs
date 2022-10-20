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


            //accountsDataGrid.ItemsSource = accounts;

            string settingsPath = NeuronF.currentPath() + @"\main\settings\";
            foreach (string fileName in Directory.GetFiles(settingsPath))
            {
                if (fileName.Contains("_accountData")) {
                    if (NeuronF.getJsonProperty(fileName, "Username").Length > 0)
                    {
                        // theres probably a much better way to do this
                        accounts.Add(new Account { Username = NeuronF.getJsonProperty(fileName, "Username"), Description = NeuronF.getJsonProperty(fileName, "Description"), lastPlace = NeuronF.getJsonProperty(fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(fileName, "Cookie") });
                    }
                }
            }

            accountsDataGrid.ItemsSource = accounts;

        }

        public void refreshGrid()
        {
            ObservableCollection<Account> accounts = new ObservableCollection<Account>();
            string settingsPath = NeuronF.currentPath() + @"\main\settings\";

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

                    DataGridRow row = (DataGridRow)vis;
                    // ((Account)row.DataContext).openRoblox();
                    customInputAndButtonMessageBox messageBox = new customInputAndButtonMessageBox();
                    messageBox.Title = ((Account)row.DataContext).Username;
                    messageBox.ShowDialog();
                }
            }
        }


        private void deleteBtn_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
                if (vis is DataGridRow)
                {
                    DataGridRow row = (DataGridRow)vis;
                    string username = ((Account)row.DataContext).Username;

                    MessageBoxResult dlgResult = MessageBox.Show("Are you sure you want to delete this account? ("+username+")", "Neuron", MessageBoxButton.YesNo);
                    if (dlgResult.ToString() == "Yes")
                    {
                        string settingsPath = NeuronF.currentPath() + @"\main\settings\";
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
    }
}
