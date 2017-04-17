using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4.UI;
using System;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lumi4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string serverUrl = "http://192.168.1.103/";
        static Uri serverUri = new Uri(serverUrl);
        WebServerCentralManager centralManager = new WebServerCentralManager(serverUri);
        WebServerPeripheral Peripheral;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new Lumi4App.ViewModels.MainPageViewModel(new Lumi4App.Models.Lumi4Model());
            
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if(Peripheral != null)
            {
                Peripheral.AddDataToSendBuffer(DataConversion.StringToListByteArray(SendTextBox.Text).ToArray());
                SendTextBox.Text = "";
            } else
            {
                // TODO: Add "Not Connected" to system message.
            }
        }

        private void IPComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = IPComboBox.SelectedItem.ToString();
            if (name != "" && name != null)
            {
                var httpPeripheral = centralManager.GetDiscoveredPeripheralByName(name);
                SelectedIp.Text = httpPeripheral.PeripheralInfo.IP.ToString();
            }
        }


    }
}
