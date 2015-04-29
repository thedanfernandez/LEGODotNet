using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LegoBot.Phone.Resources;
using System.Diagnostics;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Lego.Ev3.Phone;
using LegoBot.Shared;


namespace LegoBot.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        LegoWrapper _lego = new LegoWrapper(); 

        public MainPage()
        {
            InitializeComponent();       
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _lego.BrickChanged += _lego_BrickChanged;
            await _lego.Connect(); 
        }

        void _lego_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            txtDistance.Text = e.Ports[InputPort.Two].SIValue.ToString();
            txtTouch.Text = e.Ports[InputPort.Four].SIValue.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Name;
            DriveCommand cmd = (DriveCommand)Enum.Parse(typeof(DriveCommand), name);
            await _lego.SendCommand(cmd); 

        }


    }
}