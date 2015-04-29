using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using Lego.Ev3.Core;
using Lego.Ev3.Phone;
using LegoBot.Shared;

namespace LegoBot.Phone
{
    public partial class VoteDrive : PhoneApplicationPage
    {           
        //SignalR Hub proxy
        private IHubProxy _hubProxy;
        private LegoWrapper _lego = new LegoWrapper();
        private int _distance; 

        public VoteDrive()
        {
            InitializeComponent();
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {          
            //Sets up connection and events from SignalR server
            HubConnection connection = new HubConnection(App.LegoRobotUrl);
            _hubProxy = connection.CreateHubProxy("VoteDriveHub");
            _hubProxy.On<DriveCommand>("commandToRun", RunCommand);
            
            await connection.Start();
            _lego.BrickChanged += _lego_BrickChanged;
            await _lego.Connect();

        }

        private void RunCommand(DriveCommand command)
        {
            Dispatcher.BeginInvoke(async () =>
            {
                txtCommand.Text = command.ToString();
                await _lego.SendCommand(command);
            });
        }
        
        async void _lego_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            try
            {
                //Only send if >= 2 centimeter change
                if (Math.Abs(_distance - (int)e.Ports[InputPort.Two].SIValue) >= 2 )
                {
                    _distance = (int)e.Ports[InputPort.Two].SIValue;

                    //Send the new distance to the SignalRHub
                    await _hubProxy.Invoke("SendUpdatedSensorData", _distance);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception:" + ex);
            }

        }
    }
}