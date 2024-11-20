using Charger.Domain.Models;
using Charger.Enums;
using System.Collections.Generic;

namespace Charger.Interfaces
{
    public interface IBatteryService
    {
        IDictionary<BatteryType, BatteryData> Batteries { get; set; }
        IChargingConfig ChargingConfig { get; set; }
        double GetBatteryVoltage(BatteryType batteryType);
        void SwitchBatteryRelay(BatteryType batteryType, bool state);
        bool BatteryIsAvailable(BatteryType batteryType);
        void SwitchAllBatteryToNull();
    }
}
