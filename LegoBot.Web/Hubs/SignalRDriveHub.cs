using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LegoBot.Shared;
using System.Diagnostics;

namespace LegoBot.Web.Hubs
{
    public class SignalRDriveHub : Hub
    {
        public void SendDriveCommand(DriveCommand command)
        {
            Trace.TraceInformation("SignalR received: " + command.ToString());
            Clients.All.broadcastDriveCommand(command);
            
        }
    }
}