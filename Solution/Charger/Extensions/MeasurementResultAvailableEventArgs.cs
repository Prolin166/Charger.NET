using Charger.Enums;
using Charger.Models;
using System;

namespace Charger.Extensions
{
    public class MeasurementResultAvailableEventArgs : EventArgs
    {
        public StatusType ChargingStatus { get; set; }
        public BatteryType BatteryName { get; set; }
        public VoltageSensor Voltage { get; set; }
    }
}
