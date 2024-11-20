using Charger.Enums;

namespace Charger.Models.Discoveries
{
    public class BinarySensorDiscovery : Discovery
    {
        public string device_class { get; set; }
        public string platform { get; set; }
        public string value_template { get; set; }

        public static BinarySensorDiscovery GetHomeassistantDiscovery(string name, string basetopic, DeviceClassType deviceClassType)
        {
            basetopic = basetopic.ToLower();
            return new BinarySensorDiscovery
            {
                name = name,
                device_class = deviceClassType.ToString().ToLower(),
                state_topic = $"{basetopic}/state",
                unique_id = deviceClassType.ToString().ToLower(),
                platform = "binary_sensor",
                availability = new Availability($"{basetopic}/availability"),
                retain = true,
                device = new Device(),
                value_template = "{{ value_json.state }}"
            };
        }
    }
}
