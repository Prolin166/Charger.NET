using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charger.Interfaces
{
    public interface IMqttConnection
    {
        string HomeassistantConfigTopicFirst { get; }
        string HomeassistantConfigTopicLast { get; }

        event EventHandler<MqttApplicationMessageReceivedEventArgs> ApplicationMessageReceivedEventHandler;

        void InitMqttConnection(string username, string password, string brokerIpAddress);
        Task PublishMessage(string topic, string value);
        Task SubscribeMultipleTopics(List<MqttTopicFilterBuilder> mqttTopicFilters);
    }
}