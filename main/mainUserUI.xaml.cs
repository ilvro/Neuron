using Neuron;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            ObservableCollection<Account> accounts = new ObservableCollection<Account>();
            accounts.Add(new Account {Username="gui2", Description="the", lastPlace="lastplace", relaunchWhenClosed=false,Cookie="somecookie" });
            accounts.Add(new Account { Username = "memoriaee", Description = "description", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });
            accounts.Add(new Account { Username = "ilvro", Description = "chess cool", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });
            accounts.Add(new Account { Username = "envysor", Description = "description", lastPlace = "lastplace", relaunchWhenClosed = false, Cookie = "cookie" });



            accountsDataGrid.ItemsSource = accounts;
            

        }
        public void startUp()
        {
            //new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
            this.Title = "Neuron";

        }


        private void addAccountBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("add acccount pressed");
        }

        private void playBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("play button pressed");
        }

        private void deleteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("delete button pressed");
        }
    }
}
