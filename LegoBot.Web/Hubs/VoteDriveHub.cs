   using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using LegoBot.Shared;
using System.Threading;
using System.Collections.Concurrent;
using LegoBot.Web.Models;

namespace LegoBot.Web.Hubs
{
    public class VoteDriveHub : Hub
    {

       private static DictionaryStore storage = new DictionaryStore();
       private static Timer _votingTimer;
       private static int stateCounter = 0;
       private static int _startTime = 1000;
       private static int _interval = 500;
       private static bool _initialized;
       private static readonly object _lockObj = new object();
        
        public VoteDriveHub()
        {
            //Requirement to ensure only one timer gets built as constructor gets called repeatedly
            lock (_lockObj)
            {
                if (_initialized)
                {
                    return;                
                }
                _initialized = true;
                _votingTimer = new Timer(TimerElapsed, null, _startTime, _interval);    
            }
        }
        private void TimerElapsed(object state)
        {
            //Trace.TraceInformation(DateTime.Now.ToLongTimeString() + ": SignalR Timer Elapsed ");
            stateCounter++;

            //Send vote total every 1/2 second
            Clients.All.sendState(storage.GetState());

            //Send command every 3 seconds
            if (stateCounter >= 6)
            {
                //Trace.TraceInformation("Sending to LEGO!");
                stateCounter = 0;
                if (storage.HasCommand())
                {
                    DriveCommand cmd = storage.GetMostPopularCommand();
                    Debug.WriteLine("Popular: " + cmd.ToString());
                    Clients.All.commandToRun(cmd);
                    storage.Reset();
                }
            }
            
        }

        public void SendUpdatedSensorData(int distance)
        {
            //Trace.TraceInformation("SendUpdateSensorData, Distance: " + distance.ToString());
            storage.AddOrUpdateSensorData(distance);
        }

        public void SendDriveCommand(DriveCommand command)
        {
            //Trace.TraceInformation("SendDriveCommand: " + command.ToString());
            storage.AddOrUpdateVote(Context.ConnectionId, command);
        }

    }
}