using Charger.Enums;
using Charger.Extensions;
using Charger.Interfaces;
using Charger.Models.Discoveries;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Charger.MQTT
{
    public class BatteryConnection : IConnection
    {
        private readonly IChargerLogic _chargingLogic;
        private readonly IMqttConnection _mqttConnectionService;
        private readonly IMqttConfig _mqttConfig;
        private SensorDiscovery _batteryOneDiscovery;
        private SensorDiscovery _batteryTwoDiscovery;
        private SensorDiscovery _batteryThreeDiscovery;
        private SensorDiscovery _batteryFourDiscovery;

        public BatteryConnection(IChargerLogic chargingLogic, IMqttConnection mqttConnectionService, IMqttConfig mqttConfig)
        {
            _chargingLogic = chargingLogic;
            _mqttConnectionService = mqttConnectionService;
            _mqttConfig = mqttConfig;
        }

        public void Init()
        {
            _chargingLogic.MeasurementResultAvailable += ChargingLogic_MeasurementResultAvailable;
        }

        private void ChargingLogic_MeasurementResultAvailable(object sender, MeasurementResultAvailableEventArgs e)
        {
            _mqttConnectionService.PublishMessage($"sensor/{e.BatteryName.ToString().ToLower()}/state", JsonConvert.SerializeObject(e.Voltage)).Wait();
        }

        public async Task PublishConfigurationMessage()
        {
            _batteryOneDiscovery = SensorDiscovery.GetHomeassistantDiscovery("Batterie 1 Spannung", _mqttConfig.TopicBatteryOne, DeviceClassType.Voltage, BatteryType.BatteryOne, UnitType.V, "{{ value_json.voltage }}");
            _batteryTwoDiscovery = SensorDiscovery.GetHomeassistantDiscovery("Batterie 2 Spannung", _mqttConfig.TopicBatteryTwo, DeviceClassType.Voltage, BatteryType.BatteryTwo, UnitType.V, "{{ value_json.voltage }}");
            _batteryThreeDiscovery = SensorDiscovery.GetHomeassistantDiscovery("Batterie 3 Spannung", _mqttConfig.TopicBatteryThree, DeviceClassType.Voltage, BatteryType.BatteryThree, UnitType.V, "{{ value_json.voltage }}");
            _batteryFourDiscovery = SensorDiscovery.GetHomeassistantDiscovery("Batterie 4 Spannung", _mqttConfig.TopicBatteryFour, DeviceClassType.Voltage, BatteryType.BatteryFour, UnitType.V, "{{ value_json.voltage }}"); ;

            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicBatteryOne + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_batteryOneDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicBatteryTwo + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_batteryTwoDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicBatteryThree + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_batteryThreeDiscovery));
            await _mqttConnectionService.PublishMessage(_mqttConnectionService.HomeassistantConfigTopicFirst + _mqttConfig.TopicBatteryFour + _mqttConnectionService.HomeassistantConfigTopicLast, JsonConvert.SerializeObject(_batteryFourDiscovery));

            await _mqttConnectionService.PublishMessage(_batteryOneDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_batteryOneDiscovery.state_topic, "0");

            await _mqttConnectionService.PublishMessage(_batteryTwoDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_batteryTwoDiscovery.state_topic, "0");

            await _mqttConnectionService.PublishMessage(_batteryThreeDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_batteryThreeDiscovery.state_topic, "0");

            await _mqttConnectionService.PublishMessage(_batteryFourDiscovery.availability.topic, "online");
            await _mqttConnectionService.PublishMessage(_batteryFourDiscovery.state_topic, "0");
        }
    }
}
