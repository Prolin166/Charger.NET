using Charger.Domain.Models;
using Charger.Enums;
using Charger.Interfaces;
using Charger.Models;
using System.Collections.Generic;

namespace Charger.Services
{
    public class BatteryService : IBatteryService
    {
        private readonly IHardwareManager _hardwareManager;

        public IDictionary<BatteryType, BatteryData> Batteries { get; set; }
        public IChargingConfig ChargingConfig { get; set; }

        public BatteryService(IHardwareManager hardwareManager)
        {
            _hardwareManager = hardwareManager;
        }
        public double GetBatteryVoltage(BatteryType batteryType)
        {
            Batteries.TryGetValue(batteryType, out BatteryData batteryConfig);

            var value = _hardwareManager.MeasureBatteryVoltage(batteryConfig.MeasureAddress, batteryConfig.MeasureResistance);
            if (value > ChargingConfig.ActiveLimitVoltage)
            {
                batteryConfig.IsAvailable = true;
            }
            else
            {
                batteryConfig.IsAvailable = false;
                value = ChargingConfig.InactiveLimitVoltage;
            }
            return value;
        }

        public void SwitchBatteryRelay(BatteryType batteryType, bool state)
        {
            Batteries.TryGetValue(batteryType, out BatteryData batteryConfig);
            _hardwareManager.SwitchRelay(batteryConfig.Switch, state);
        }

        public bool BatteryIsAvailable(BatteryType batteryType)
        {
            Batteries.TryGetValue(batteryType, out BatteryData batteryConfig);
            return batteryConfig.IsAvailable;
        }

        public void SwitchAllBatteryToNull()
        {
            foreach (var battery in Batteries.Keys)
            {
                SwitchBatteryRelay(battery, false);
            }
        }
    }
}
