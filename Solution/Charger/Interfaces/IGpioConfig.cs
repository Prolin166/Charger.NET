using Charger.Models;

namespace Charger.Interfaces
{
    public interface IGpioConfig
    {
        ControlData DoorSensor { get; set; }
        ControlData LedGreen { get; set; }
        ControlData LedYellow { get; set; }
        ControlData LedBlue { get; set; }
        void Initialize();
    }
}