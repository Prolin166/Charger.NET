using Charger.Models;

namespace Charger.Domain.Models
{
    public class BatteryData
    {
        public string BatteryName { get; set; }
        public bool IsAvailable { get; set; }
        public double MeasureResistance { get; set; }
        public WagoPort MeasureAddress { get; set; }
        public WagoPort Switch { get; set; }
    }
}
