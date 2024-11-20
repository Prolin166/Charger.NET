namespace Charger.Models
{
    public class Availability
    {
        public Availability(string topic)
        {
            this.topic = topic;
        }

        public string topic { get; set; }
    }
}
