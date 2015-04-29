using System;
namespace LegoBot.Web.Models
{
    interface IDictionaryStore
    {
        void AddOrUpdateSensorData(int distance);
        void AddOrUpdateVote(string id, LegoBot.Shared.DriveCommand command);
        LegoBot.Shared.DriveCommand GetMostPopularCommand();
        LegoBot.Shared.LegoState GetState();
        bool HasCommand();
        void Reset();
    }
}
