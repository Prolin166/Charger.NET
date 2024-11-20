using Charger.Enums;
using Charger.Interfaces;
using Charger.Models.Discoveries;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Charger.MQTT
{
    public class ManagementConnection : IConnection
    {
        private readonly IChargerLogic _chargingLogic;
        private readonly IMqttConnection _mqttConnectionService;
        private readonly IMqttConfig _mqttConfig;
        private SelectDiscovery _chargingStateDiscovery;
        private NumberDiscovery _chargingLimitDiscovery;
        private NumberDiscovery _chargingTimeDiscovery;
        private BinarySensorDiscovery _doorSensorStatusDiscovery;
        private BinarySensorDiscovery _processStatusDiscovery;

        public ManagementConnection(IChargerLogic chargingLogic, IMqttConnection mqttConnectionService, IMqttConfig mqttConfig)
        {
            _chargingLogic = chargingLogic;
            _mqttConnectionService = mqttConnectionService;
            _mqttConfig = mqttConfig;
        }

        public void Init()
        {
            _chargingLogic.ChargingProcessParameterChanged += GetChargingProcessParameter;
            _mqttConnectionService.ApplicationMessageReceivedEventHandler += MqttConnectionService_ApplicationMessageReceivedEventHandler;
            InitChargingLogic();
        }

        private void MqttConnectionService_ApplicationMessageReceivedEventHandler(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            var payload = Encoding.Default.GetString(e.ApplicationMessage.PayloadSegment);

            if (e.ApplicationMessage.Topic == _chargingStateDiscovery.command_topic)
                _chargingLogic.ChargingStatus = (StatusType)Enum.Parse(typeof(StatusType), payload);

            if (e.ApplicationMessage.Topic == _chargingLimitDiscovery.command_topic)
                _chargingLogic.ChargingLimit = double.Parse(payload, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            if (e.ApplicationMessage.Topic == _chargingTimeDiscovery.command_topic)
                _chargingLogic.TimeToCharge = TimeSpan.FromSeconds(int.Parse(payload));
        }

        private void GetChargingProcessParameter(object sender, PropertyChangedEventArgs e)
        {
            _mqttConnectionService.PublishMessage(_chargingStateDiscovery.state_topic, _chargingLogic.ChargingStatus.ToString());
            _mqttConnectionService.PublishMessage(_chargingTimeDiscovery.state_topic, _chargingLogic.TimeToCharge.TotalSeconds.ToString());
            _mqttConnectionService.PublishMessage(_chargingLimitDiscovery.state_topic, _chargingLogic.ChargingLimit.ToString().Replace(',', '.'));
            _mqttConnectionService.PublishMessage(_doorSensorStatusDiscovery.state_topic, _chargingLogic.DoorStatus == false ? "ON" : "OFF");
            _mqttConnectionService.PublishMessage(_processStatusDiscovery.state_topic, _chargingLogic.ProcessInRunMode == true ? "ON" : "OFF");
        }

        public async Task PublishConfigurationMessage()
        {
            _chargingStateDiscovery = SelectDiscovery.GetHomeassistantDiscovery("Ladestatus", _mqttConfig.TopicChargingState, _mqttConfig.TopicChargingState + "01", new List<string> { "Conservation", "Observation", "Charging" });
            _chargingLimitDiscovery = NumberDiscovery.GetHomeassistantDiscovery("Ladelimit", _mqttConfig.TopicChargingLimit, UnitType.V, _mqttConfig.TopicChargingLimit + "01", 8, 16, (float)0.1, DisplayType.Box);
            _chargingTimeDiscovery = NumberDiscovery.GetHomeassistantDiscovery("Ladezeit", _mqttConfig.TopicChargingTime, UnitType.s, _mqttConfig.TopicChargingTime + "01", 1, 600, 1, DisplayType.Box);
            _doorSensorStatusDiscovery = BinarySensorDiscovery.GetHomeassistantDiscovery("Türstatus", _mqttConfig.TopicDoorSensorState, DeviceClassType.Door);
            _processStatusDiscovery = BinarySensorDiscovery.GetHomeassistantDiscovery("Ladeprozess", _mqttConfig.TopicProcessState, DeviceClassType.Battery_Charging);

            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicChargingState + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_chargingStateDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicChargingLimit + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_chargingLimitDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicChargingTime + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_chargingTimeDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicDoorSensorState + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_doorSensorStatusDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicProcessState + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_processStatusDiscovery));


            await _mqttConnectionService.PublishMessage(_chargingStateDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_chargingStateDiscovery.state_topic, "Observation");

            await _mqttConnectionService.PublishMessage(_chargingLimitDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_chargingLimitDiscovery.state_topic, "8,5");

            await _mqttConnectionService.PublishMessage(_chargingTimeDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_chargingTimeDiscovery.state_topic, "35");

            await _mqttConnectionService.PublishMessage(_doorSensorStatusDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_doorSensorStatusDiscovery.state_topic, "OFF");

            await _mqttConnectionService.PublishMessage(_processStatusDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_processStatusDiscovery.state_topic, "OFF");
        }

        private void InitChargingLogic()
        {
            _chargingLogic.ChargingLimit = 8.8;
            _chargingLogic.ChargingStatus = StatusType.Observation;
            _chargingLogic.TimeToCharge = new TimeSpan(0, 0, 0, 30);
        }
    }
}
