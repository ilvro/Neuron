using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace Neuron
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
        }

        public void InitializeBrowser(string url) // usual url is "http://www.roblox.com/login"
        {
            if (!Cef.IsInitialized) // cef might be initialized before, which causes errors
            {
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);
            }

            ChromiumWebBrowser browser = new ChromiumWebBrowser(url) // create chromium browser
            {
                Dock = DockStyle.Fill,
                Size = new Size(1024, 978),
                Location = new Point(0, 0)
            };
            this.Controls.Add(browser);
        }


        private void Browser_Load(object sender, EventArgs e)
        {
            //
        }
    }
}
