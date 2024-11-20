namespace Charger.Interfaces
{
    public interface IMqttConfig
    {
        string MqttBrokerAddress { get; set; }
        string MqttBrokerUsername { get; set; }
        string MqttBrokerPassword { get; set; }
        string TopicChargingLimit { get; set; }
        string TopicChargingTime { get; set; }
        string TopicChargingState { get; set; }
        string TopicDoorSensorState { get; set; }
        string TopicProcessState { get; set; }
        string TopicBatteryOne { get; set; }
        string TopicBatteryTwo { get; set; }
        string TopicBatteryThree { get; set; }
        string TopicBatteryFour { get; set; }
    }
}
