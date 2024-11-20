using System.Device.Gpio;

namespace Charger.Interfaces
{
    public interface IGpioProvider
    {
        GpioController LcdController { get; set; }
        GpioController GpioController { get; set; }

        void InitGpio(bool initGpio);
        void ShowTextOnDisplay(int line, int column, string text);
        void ClearDisplay();
        void SwitchGpioPin(int pinNumber, bool status);
        bool ReadGpioStatus(int pinNumber);
    }
}
