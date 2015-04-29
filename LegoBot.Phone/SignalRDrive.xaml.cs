using LegoBot.Shared;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Phone.Controls;
using System.Windows;

namespace LegoBot.Phone
{
    public partial class SignalRDrive : PhoneApplicationPage
    {
        //SignalR Hub proxy
        private IHubProxy _hubProxy;
        LegoWrapper _lego = new LegoWrapper(); 

        public SignalRDrive()
        {
            InitializeComponent();
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            await _lego.Connect();

            //Sets up connection and events from SignalR server
            HubConnection connection = new HubConnection(App.LegoRobotUrl);
            _hubProxy = connection.CreateHubProxy("SignalRDriveHub");
            _hubProxy.On<DriveCommand>("broadcastDriveCommand", CommandReceived);
            
            await connection.Start();
            txtCommand.Visibility = System.Windows.Visibility.Visible;
        }

        private void CommandReceived(DriveCommand command)
        {
            Dispatcher.BeginInvoke(async () =>
            {
                txtCommand.Text = command.ToString();
                await _lego.SendCommand(command);
            });
        }



    }
}