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
            //accounts.Add(new Account("memoriaee", "description", "lastplace", false, "cookie"));
            //accounts.Add(new Account("ilvro", "chess cool", "lastplace", false, "cookie"));
            //accounts.Add(new Account("envysor", "description", "lastplace", false, "cookie"));

            accountsDataGrid.ItemsSource = accounts;

        }
        public void startUp()
        {
            //new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
            this.Title = "Neuron";

        }
    }
}
