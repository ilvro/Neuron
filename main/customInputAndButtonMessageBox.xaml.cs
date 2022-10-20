using Neuron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Neuron_V2.main
{
    /// <summary>
    /// Interaction logic for customInputAndButtonMessageBox.xaml
    /// </summary>
    public partial class customInputAndButtonMessageBox : Window
    {
        public customInputAndButtonMessageBox()
        {
            InitializeComponent();
        }

        private void playBtn2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string settingsPath = NeuronF.currentPath() + @"\main\settings\";
            string fileName = settingsPath + this.Title + "_accountData.json";
            new Account { Username = this.Title, Description = "", lastPlace = NeuronF.getJsonProperty(fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(fileName, "Cookie"), lastMethod = "None" }.openRoblox();

            string json = File.ReadAllText(fileName);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj[0]["lastPlace"] = gameIDLine.Text;
            jsonObj[0]["lastMethod"] = "openApp";
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, output);
            this.Close();
        }
        private void btnJoin_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (gameIDLine.Text.Length > 0)
                {
                    string settingsPath = NeuronF.currentPath() + @"\main\settings\";
                    string fileName = settingsPath + this.Title + "_accountData.json";
                    new Account { Username = this.Title, Description = "", lastPlace = NeuronF.getJsonProperty(fileName, "lastPlace"), relaunchWhenClosed = NeuronF.getJsonProperty(fileName, "relaunchWhenClosed"), Cookie = NeuronF.getJsonProperty(fileName, "Cookie") }.joinServer(Convert.ToInt64(gameIDLine.Text));

                    // edit "lastplace" property, its used for relaunching
                    string json = File.ReadAllText(fileName);
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    jsonObj[0]["lastPlace"] = gameIDLine.Text;
                    jsonObj[0]["lastMethod"] = "joinServer";
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(fileName, output);
                    this.Close();
                }
                else
                {
                    //throw new Exception();
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid server ID.");
            }
        }

        private void gameIDLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            var changes = e.Changes.Last();
            if (gameIDLine.Text.Length > 0)
            {
                IDText.Opacity = 0;
            }
            else
            {
                IDText.Opacity = 0.3;
            }
        }
    }
}
