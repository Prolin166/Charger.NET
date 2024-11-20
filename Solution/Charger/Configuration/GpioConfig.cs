using Charger.Interfaces;
using Charger.Models;

namespace Charger.Configuration
{
    public class GpioConfig : IGpioConfig
    {
        public bool InitGpio { get; set; }
        public ControlData DoorSensor { get; set; }
        public ControlData LedGreen { get; set; }
        public ControlData LedYellow { get; set; }
        public ControlData LedBlue { get; set; }

        public void Initialize()
        {
            InitGpio = true;
            DoorSensor = new ControlData
            {
                ControlName = "DoorSensor",
                ControlAddress = 18
            };

            LedBlue = new ControlData
            {
                ControlName = "LedBlue",
                ControlAddress = 38
            };

            LedGreen = new ControlData
            {
                ControlName = "LedGreen",
                ControlAddress = 32
            };

            LedYellow = new ControlData
            {
                ControlName = "LedYellow",
                ControlAddress = 35
            };
        }
    }
}
