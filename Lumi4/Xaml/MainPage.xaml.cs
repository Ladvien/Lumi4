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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lumi4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string serverUrl = "http://192.168.1.103/";
        // LumiHttpPeripheral esper = new LumiHttpPeripheral(serverUrl);

        public MainPage()
        {
            this.InitializeComponent();

            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

            // Height
            //ReceivedTextBoxScrollPresenter.Height = ReceivedTextBoxScrollPresenter.
            //UpdateLayout();
            //var textBoxHeight = bounds.Height - MainStack.Height;
            //ReceivedTextBlock.Height = textBoxHeight;
            // ReceivedScrollView.MaxHeight = textBoxHeight;


            //esper.PostString("Hey you!");
            //esper.SetPollingDelay(2000);
            //esper.GetDeviceName();
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };


        }

        private async void Get_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            //string sendMessage = txTextBox.Text;
            //esperPostString(serverUrl, sendMessage);
            //esper.End();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            //LumiHttpPeripheral esper = new LumiHttpPeripheral(ProgressBar);
            //var discoveredIPs = await esper.SearchForESPER(98, 130);

            //foreach (Uri ip in discoveredIPs)
            //{
            //    IPComboBox.Items.Add(ip.Host);
            //}
            //IPComboBox.SelectedIndex = 0;
        }
    }
}
