using Lego.Ev3.Core;
using Lego.Ev3.Phone;
using LegoBot.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LegoBot.Phone
{

    public class LegoWrapper
    {
        public Brick LegoBrick;
        public const uint QuarterSecond = 250;
        public const uint OneSecond = 1000;
        public const int HalfPower = 50; 

        public event EventHandler<BrickChangedEventArgs> BrickChanged; 

        public async Task Connect()
        {
            LegoBrick = new Brick(new BluetoothCommunication(), true);
            LegoBrick.BrickChanged += LegoBrick_BrickChanged;
            await LegoBrick.ConnectAsync();
            await LegoBrick.DirectCommand.PlayToneAsync(100, 40000, 300);

        }

        private void LegoBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            if (BrickChanged != null)
            {
                BrickChanged(this, e); 
            }
        }

        public async Task SendCommand(DriveCommand command)
        {
            if (LegoBrick == null)
            {
                return;
            }

            switch (command)
            {
                case DriveCommand.Forward:
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, HalfPower, OneSecond, false);
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, HalfPower, OneSecond, false);

                    break;
                case DriveCommand.Back:
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, HalfPower * -1, OneSecond, false);
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, HalfPower * -1, OneSecond, false);

                    break;
                case DriveCommand.Left:
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, HalfPower * -1, QuarterSecond, false);
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, HalfPower, QuarterSecond, false);

                    break;
                case DriveCommand.Right:
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, HalfPower * -1, QuarterSecond, false);
                    LegoBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, HalfPower, QuarterSecond, false);

                    break;
                default:
                    break;
            }
            await LegoBrick.BatchCommand.SendCommandAsync(); 
        }
    }
}
