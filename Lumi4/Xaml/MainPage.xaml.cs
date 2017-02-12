using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4.CentralManager;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lumi4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string serverUrl = "http://192.168.1.103/";

        public MainPage()
        {
            this.InitializeComponent();

            // UI Hacking.
            // TODO: Reconsider below.
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };

            HttpPeripheral httpPeripheral = (HttpPeripheral)PeripheralFactory.CreateNewPeripheral("wifi");

            Debug.WriteLine(httpPeripheral);

        }

        private async void Get_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
