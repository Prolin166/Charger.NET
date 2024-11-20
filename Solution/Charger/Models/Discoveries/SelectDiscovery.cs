using System.Collections.Generic;

namespace Charger.Models.Discoveries
{
    public class SelectDiscovery : Discovery
    {
        public string command_topic { get; set; }
        public List<string> options { get; set; }
        public static SelectDiscovery GetHomeassistantDiscovery(string name, string basetopic, string uniqueId, List<string> options)
        {
            basetopic = basetopic.ToLower();
            return new SelectDiscovery
            {
                name = name,
                command_topic = $"{basetopic}/set",
                state_topic = $"{basetopic}/state",
                unique_id = uniqueId,
                availability = new Availability($"{basetopic}/availability"),
                retain = true,
                options = options,
                device = new Device()
            };
        }
    }
}
