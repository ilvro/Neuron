using Neuron;
using System;
using System.Collections.Generic;
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
            new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
            // new Account("Cildum", "_|WARNING:-DO-NOT-SHARE-THIS.--Sharing-this-will-allow-someone-to-log-in-as-you-and-to-steal-your-ROBUX-and-items.|_B79F08218181778EB89381241A8131DC6F6C2A7436660836FC9E399A0C601CF7AB3486430EBD3CFBEC0DA471DDBBBC7FA4F267CFA26341371DB7B4DFE422FF308897D108104B0C4C491517B8F87142C9484233D2EE48EC6BC9AB28328DBD9F0A2EA839378842CAB102CE8A4A854F86C4DC5731E875BC7E59720F929B9A1C2B9B15F23DFEF5A719822E84E23990AA4CCABF07071020066C9BB1BB8800451C90689984764AB8BCA5769829B862B7F2936E0BF25B8DFAB49AC0B301F432C692B2F36DFF71BAF32DE6D2B3FEE0738AE45837F7F93DA78D73F3EB577737C23212C1DF888BA2453CEDABBD469440AA816BE6EC3960260691DB642B75F8E3731A5913216781C5AEF2618C4DEF910731B2E52D4486AFAF435FCB7C0EC58D86B720576A8AAF250B0DD8CB27E2E614B176536BF545F2E502C976CDB18398985D51ECA3FEE54E646E7AA8204979ADF6AC8A3937FA3E45C76C6C0F4720511718E5905AB14378406D5E58").openRoblox();


        }
        private void startUp()
        {
            //new Mutex(true, "ROBLOX_singletonMutex"); // thx alt manager
        }
    }
}
