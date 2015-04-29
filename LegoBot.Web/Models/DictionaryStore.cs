using LegoBot.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace LegoBot.Web.Models
{
    public  class DictionaryStore : LegoBot.Web.Models.IDictionaryStore
    {
        private readonly ConcurrentDictionary<string, DriveCommand> PlayerVote = new ConcurrentDictionary<string, DriveCommand>();
        private LegoState _state = new LegoState(); 

        public void AddOrUpdateVote(string id, DriveCommand command)
        {
            PlayerVote.AddOrUpdate(id, command, (key, oldValue) => command);      
        }

        public void AddOrUpdateSensorData(int distance)
        {
            _state.SensorDistance = distance; 
        }


        public void Reset()
        {
            PlayerVote.Clear();

            foreach (var item in _state.AllVotes)
            {
                item.Count = 0;
            }

        }


        public LegoState GetState() 
        {
            foreach (var item in _state.AllVotes)
            {
                item.Count = PlayerVote.Values.Count(cmd => cmd == item.Command);
            }

            return _state; 
        }

        public DriveCommand GetMostPopularCommand()
        {
            LegoState state = GetState();
            var cmd = (from s in state.AllVotes
                        orderby s.Count descending
                        select s.Command).FirstOrDefault();
            Trace.TraceInformation("Most Popular = " + cmd.ToString()); 
            
            return cmd; 
        
        }
        public bool HasCommand()
        {
            if (PlayerVote.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}