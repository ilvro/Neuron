﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Neuron;
using Neuron_V2.main;
using SlavaGu.ConsoleAppLauncher;
using RestSharp;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;

namespace Neuron_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            startUp();
            this.Title = "Neuron";


            // because the wl is down
            var mainWindow = new mainUserUI();
            mainWindow.ShowDialog(); // this pauses this window's code
            Application.Current.Shutdown(); // stops everything when the main ui is closed (mainUserUI)
           
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void startUp()
        {

            /*
            if (Directory.Exists(NeuronF.currentPath() + @"\main").ToString() != "True") // ...
            {
                //MessageBox.Show("Couldn't find folder 'main'. This could be due to a download error.\nTry downloading again.");
                MessageBox.Show("wrong directory");
                Application.Current.Shutdown();
            }*/

            // check if roblox is open; if it is, prompt to close it. this is because multi roblox wont work if its already open
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
            if (processes.Length > 0)
            {
                MessageBoxResult dlgResult = MessageBox.Show("Close ROBLOX? If you press no, you won't be able to use multiple accounts.", "Neuron", MessageBoxButton.YesNo);
                if (dlgResult.ToString() == "Yes")
                {
                    try
                    {
                        processes[0].Kill();
                    }
                    catch
                    {
                        
                    }
                }
            }


        }
        private void userTextBorder_GotFocus(object sender, RoutedEventArgs e)
        {
            //
        }

        private void keyTextAndBorder_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var changes = e.Changes.Last();
            if (keyTextAndBorder.Text.Length > 0)
            {
                keyText.Opacity = 0;
            }
            else
            {
                keyText.Opacity = 0.3;
            }
        }

        private void IDTextAndBorder_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var changes = e.Changes.Last();
            if (IDTextAndBorder.Text.Length > 0)
            {
                IDText.Opacity = 0;
            }
            else
            {
                IDText.Opacity = 0.3;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) // checks whitelist
        {
            if (NeuronF.checkWhitelist(keyTextAndBorder.Text, IDTextAndBorder.Text, "login") == "qptoe5?")
            {


                List<NeuronF.AuthData> data = new List<NeuronF.AuthData>();
                data.Add(new NeuronF.AuthData()
                {
                    discordID = IDTextAndBorder.Text,
                    Key = keyTextAndBorder.Text,

                });

                string json = System.Text.Json.JsonSerializer.Serialize(data);
                File.WriteAllText(NeuronF.currentPath() + @"\main\settings\login.json", json);

                var mainNeuron = new mainUserUI();
                mainNeuron.ShowDialog(); // this pauses this window's code
                Application.Current.Shutdown(); // stops everything when the main ui is closed (mainUserUI)
                //string a = "#119ce0";
            }
        }
    }

}
