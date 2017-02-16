﻿using System;
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
using Lumi4;
using System.Diagnostics;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.CentralManager;

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



            //HttpPeripheral httpPeripheral = (HttpPeripheral)PeripheralFactory.CreateNewPeripheral("http");

            //Debug.WriteLine(httpPeripheral);

            //httpPeripheral.DeviceStateChange += HttpPeripheral_DeviceStateChange;
            //httpPeripheral.ReceivedData += HttpPeripheral_ReceivedData;
            //httpPeripheral.SentData += HttpPeripheral_SentData;
            //httpPeripheral.Test();

            WifiCentralManager centralManager = new WifiCentralManager();

            centralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            centralManager.Start();
            

        }

        private void CentralManager_DeviceStateChange(object source, DeviceStateChangeEventArgs args)
        {
            Debug.WriteLine(args.DeviceState.State);
        }

        private void HttpPeripheral_ReceivedData(object source, ReceivedDataEventArgs args)
        { 
            DataConversion converter = new DataConversion();
            var str = converter.ByteArrayToAsciiString(args.ReceivedData);
            Debug.WriteLine(str);
        }

        private void HttpPeripheral_SentData(object source, SentDataEventArgs args)
        {
            DataConversion converter = new DataConversion();
            var str = converter.ByteArrayToAsciiString(args.SentData);
            Debug.WriteLine(str);
        }

        private void HttpPeripheral_DeviceStateChange(object source, DeviceStateChangedEventArgs args)
        {
            Debug.WriteLine(args.PeripheralInfo.Name);
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
