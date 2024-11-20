using Charger.Domain.Models;
using Charger.Models;

namespace Charger.Interfaces
{
    public interface IWagoConfig
    {
        string IpAddress { get; set; }
        ushort Port { get; set; }
        WagoPort ChargingSwitch { get; set; }
        BatteryData BatteryOne { get; set; }
        BatteryData BatteryTwo { get; set; }
        BatteryData BatteryThree { get; set; }
        BatteryData BatteryFour { get; set; }
        void Initialize();
    }
}