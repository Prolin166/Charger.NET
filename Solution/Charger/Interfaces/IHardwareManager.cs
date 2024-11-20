using Charger.Models;

namespace Charger.Interfaces
{
    public interface IHardwareManager
    {
        double MeasureBatteryVoltage(WagoPort address, double measureResisitance);
        void SwitchRelay(WagoPort address, bool state);
        void SwitchLed(bool state, int ledAddress);
        void SetDisplayText(int line, int column, string text);
        bool ReadSensorStatus(int sensorAddress);
        void ClearDisplay();
        void InitHardwareConnection(string ipaddress, ushort tcpPort, bool initGpio);
    }
}
