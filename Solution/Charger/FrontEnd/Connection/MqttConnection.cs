using Charger.Interfaces;

using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Charger.FrontEnd.Connection
{
    public class MqttConnection : IMqttConnection
    {
        private MqttClientOptions _mqttClientOptions;
        private MqttFactory _mqttFactory;
        private IMqttClient _mqttClient;

        public event EventHandler<MqttApplicationMessageReceivedEventArgs> ApplicationMessageReceivedEventHandler;

        public string HomeassistantConfigTopicFirst { get => "homeassistant/"; }

        public string HomeassistantConfigTopicLast { get => "/config"; }

        public async Task PublishMessage(string topic, string value)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(value)
                .Build();

            await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        }

        public async Task SubscribeMultipleTopics(List<MqttTopicFilterBuilder> mqttTopicFilters)
        {
            _mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
            var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder();

            foreach (var mqttTopicFilter in mqttTopicFilters)
            {
                mqttSubscribeOptions.WithTopicFilter(mqttTopicFilter);
            }
            await _mqttClient.SubscribeAsync(mqttSubscribeOptions.Build(), CancellationToken.None);
        }

        public void InitMqttConnection(string username, string password, string brokerIpAddress)
        {
            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithCredentials(username, password)
                .WithTcpServer(brokerIpAddress)
                .WithWillRetain(true)
                .Build();

            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
        }

        private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            ApplicationMessageReceivedEventHandler?.Invoke(this, arg);
            return Task.CompletedTask;
        }
    }
}