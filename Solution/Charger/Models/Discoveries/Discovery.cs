namespace Charger.Models.Discoveries
{
    public abstract class Discovery
    {
        public string name { get; set; }
        public string state_topic { get; set; }
        public string unique_id { get; set; }
        public Availability availability { get; set; }
        public bool retain { get; set; }
        public Device device { get; set; }
    }
}
