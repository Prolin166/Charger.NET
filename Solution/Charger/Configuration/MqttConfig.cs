using Charger.Interfaces;

namespace Charger.Configuration
{
    public class MqttConfig : IMqttConfig
    {
        public string MqttBrokerAddress { get; set; }
        public string MqttBrokerUsername { get; set; }
        public string MqttBrokerPassword { get; set; }
        public string TopicChargingLimit { get; set; }
        public string TopicChargingTime { get; set; }
        public string TopicChargingState { get; set; }
        public string TopicDoorSensorState { get; set; }
        public string TopicProcessState { get; set; }
        public string TopicBatteryOne { get; set; }
        public string TopicBatteryTwo { get; set; }
        public string TopicBatteryThree { get; set; }
        public string TopicBatteryFour { get; set; }
    }
}
