using Charger.Enums;

namespace Charger.Models.Discoveries
{
    public class SensorDiscovery : Discovery
    {
        public string device_class { get; set; }
        public string unit_of_measurement { get; set; }
        public string value_template { get; set; }

        public static SensorDiscovery GetHomeassistantDiscovery(string name, string basetopic, DeviceClassType deviceClassType, BatteryType battery, UnitType unitType, string valuetemplate)
        {
            basetopic = basetopic.ToLower();
            return new SensorDiscovery
            {
                name = name,
                device_class = deviceClassType.ToString().ToLower(),
                state_topic = $"{basetopic}/state",
                unit_of_measurement = unitType.ToString(),
                value_template = valuetemplate.ToLower(),
                unique_id = deviceClassType.ToString().ToLower() + (int)battery,
                availability = new Availability($"{basetopic}/availability"),
                retain = true,
                device = new Device()
            };
        }
    }
}
