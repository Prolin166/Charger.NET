using System.Collections.Generic;

namespace Charger.Models
{
    public class Device
    {
        public List<string> identifiers { get; set; } = new List<string> { "charger", "voltage1", "voltage2", "voltage3" };
        public string name { get; set; } = "Batterie-Ladegerät";
        public string manufacturer { get; set; } = "Prolin";
        public string model { get; set; } = "12V";
        public string serial_number { get; set; } = "0001";
        public string hw_version { get; set; } = "1.0.0.0";
        public string sw_version { get; set; } = "1.0.0.0";
        public string configuration_url { get; set; } = "";
    }
}
