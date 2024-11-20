using Charger.Interfaces;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using System;
using System.Device.Gpio;
using System.Device.I2c;

namespace Charger.Hardware.Connection
{
    public class GpioProvider : IGpioProvider
    {
        private const int BUS_ID = 1;
        private const int DEVICE_ADDRESS = 0x27;

        private I2cDevice _i2cDevice;
        private Pcf8574 _gpioDriver;
        private Lcd2004 _lcdDisplay;
        private bool _initGpio;

        public GpioController LcdController { get; set; }
        public GpioController GpioController { get; set; }

        public void InitGpio(bool initGpio)
        {
            _initGpio = initGpio;
            if (_initGpio)
            {
                _i2cDevice = I2cDevice.Create(new I2cConnectionSettings(BUS_ID, DEVICE_ADDRESS));
                _gpioDriver = new Pcf8574(_i2cDevice);
                LcdController = new GpioController(PinNumberingScheme.Logical, _gpioDriver);
                _lcdDisplay = CreateLcdPanel();
                _lcdDisplay.BlinkingCursorVisible = false;
                _lcdDisplay.UnderlineCursorVisible = false;
                GpioController = new GpioController(PinNumberingScheme.Board);
            }
        }

        public void ShowTextOnDisplay(int line, int column, string text)
        {
            if (_initGpio)
            {
                _lcdDisplay.SetCursorPosition(column, line);
                _lcdDisplay.Write(text);
                Console.WriteLine($"Line = {line}; Column = {column}; Text = {text}");
            }
        }

        public void ClearDisplay()
        {
            _lcdDisplay.Clear();
        }

        public void SwitchGpioPin(int pinNumber, bool status)
        {
            if (_initGpio)
            {
                var pinStatus = (status) ? PinValue.High : PinValue.Low;
                GpioController.OpenPin(pinNumber, PinMode.Output);
                GpioController.Write(pinNumber, pinStatus);
                GpioController.ClosePin(pinNumber);
                Console.WriteLine($"PinNumber = {pinNumber} = {pinStatus}");
            }
        }

        public bool ReadGpioStatus(int pinNumber)
        {
            bool gpioHigh = false;
            if (_initGpio)
            {
                GpioController.OpenPin(pinNumber, PinMode.InputPullDown);
                var pinStatus = GpioController.Read(pinNumber);
                gpioHigh = pinStatus == PinValue.High;
                GpioController.ClosePin(pinNumber);
                Console.WriteLine($"PinNumber = {pinNumber} = {pinStatus}");
            }

            return gpioHigh;
        }

        private Lcd2004 CreateLcdPanel()
        {
            return new Lcd2004(registerSelectPin: 0,
            enablePin: 2,
            dataPins: new int[] { 4, 5, 6, 7 },
            backlightPin: 3,
            backlightBrightness: 0.1f,
            readWritePin: 1,
            controller: LcdController);
        }
    }
}
