using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Neuron_V2.main;

namespace Neuron
{
    public partial class Browser : Form
    {
        public delegate void SafeCallDelegate();
        public Browser()
        {
            InitializeComponent();
        }

        public ChromiumWebBrowser InitializeBrowser(string url, bool newAccount) // "http://www.roblox.com/login"
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
            try
            {
                Controls.Add(browser);
            }
            catch
            {
                Cef.GetGlobalCookieManager().DeleteCookies();
                browser.Load("http://www.roblox.com/login");
                CloseForm();
            }
            this.StartPosition = FormStartPosition.CenterScreen;
            browser.AddressChanged += Browser_AddressChangedAsync; // send event when user logins
            return browser;
    }



        private void Browser_Load(object sender, EventArgs e)
        {

        }

        private async void Browser_AddressChangedAsync(object sender, AddressChangedEventArgs e) // get roblox data. used to get .ROBLOSECURITY and username after the user adds an account; theyre needed to start roblox
        {
            // ashamed to say i ripped this whole part out of alt manager
            string url = e.Address;
            if (url.Contains("home"))
            {
                //MessageBox.Show("logged in");

                var cookieManager = Cef.GetGlobalCookieManager();
                await cookieManager.VisitAllCookiesAsync().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        List<Cookie> cookies = t.Result;

                        Cookie ROBLOSecurity = cookies.Find(x => x.Name == ".ROBLOSECURITY");

                        if (ROBLOSecurity != null)
                        {
                            NeuronF.addAccount(ROBLOSecurity.Value);
                            InitializeBrowser("https://www.roblox.com/login", false);
                        }

                    }
                });
            }


        }
        private void CloseForm() // sorry ic3
        {
            if (this.InvokeRequired)
            {
                var close = new SafeCallDelegate(CloseForm);
                this.Invoke(close, new object[] { });
            }
            else
                Close();
        }
        //
    }
}
