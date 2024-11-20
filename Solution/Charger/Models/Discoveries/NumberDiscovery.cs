using Charger.Enums;

namespace Charger.Models.Discoveries
{
    public class NumberDiscovery : Discovery
    {
        public string command_topic { get; set; }
        public string unit_of_measurement { get; set; }
        public float min { get; set; }
        public float max { get; set; }
        public float step { get; set; }
        public string mode { get; set; }

        public static NumberDiscovery GetHomeassistantDiscovery(string name, string basetopic, UnitType unitType, string uniqueId, float min, float max, float step, DisplayType mode)
        {
            basetopic = basetopic.ToLower();
            return new NumberDiscovery
            {
                name = name,
                command_topic = $"{basetopic}/set",
                state_topic = $"{basetopic}/state",
                unit_of_measurement = unitType.ToString(),
                unique_id = uniqueId,
                min = min,
                max = max,
                step = step,
                mode = mode.ToString().ToLower(),
                availability = new Availability($"{basetopic}/availability"),
                retain = true,
                device = new Device()
            };
        }
    }
}
