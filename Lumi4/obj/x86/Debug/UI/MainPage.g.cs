﻿#pragma checksum "C:\Users\Ladvien\Documents\Visual Studio 2015\Projects\Lumi4\Lumi4\UI\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6B03A900A85338A8484850091B77E8BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lumi4
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.ProgressBar = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                }
                break;
            case 2:
                {
                    this.ReceivedTextGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 129 "..\..\..\UI\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.Send_Button_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.ReceivedTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.Search = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 50 "..\..\..\UI\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Search).Click += this.Search_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.IPComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 7:
                {
                    this.DeviceNameLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.txIndicator = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 9:
                {
                    this.rxIndicator = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

