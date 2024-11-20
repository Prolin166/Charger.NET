using Charger.Interfaces;
using Charger.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Charger.Hardware
{
    public class HardwareManager : IHardwareManager
    {
        private IModbusProvider _modbusProvider;
        private IGpioProvider _gpioProvider;
        private IGuidHelper _guidHelper;
        private IConversionHelper _conversionHelper;
        private ILogger<HardwareManager> _logger;

        public HardwareManager(IModbusProvider modbusProvider,
                                IGpioProvider gpioProvider,
                                IGuidHelper guidHelper,
                                IConversionHelper conversionHelper,
                                ILogger<HardwareManager> logger)
        {
            _modbusProvider = modbusProvider;
            _gpioProvider = gpioProvider;
            _guidHelper = guidHelper;
            _conversionHelper = conversionHelper;
            _logger = logger;
        }

        public double MeasureBatteryVoltage(WagoPort address, double measureResistance)
        {
            try
            {
                int numDataBytes = 2;
                byte[] data = _modbusProvider.ReadAnalogInput(_guidHelper.CreateGuidAsUShort(), address.StartAddress, address.AddressLength, numDataBytes);
                Array.Reverse(data);
                double voltage = _conversionHelper.ConvertBytesToVoltage(data);
                voltage = _conversionHelper.CalculateOriginalVoltage(voltage, measureResistance);
                return voltage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public void SwitchRelay(WagoPort address, bool state)
        {
            try
            {
                int numRes = 2;
                byte[] val = _modbusProvider.WriteSingleDigitalOutput(_guidHelper.CreateGuidAsUShort(), address.StartAddress, state, numRes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public void SwitchLed(bool state, int ledAddress)
        {
            try
            {
                _gpioProvider.SwitchGpioPin(ledAddress, state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public void SetDisplayText(int line, int column, string text)
        {
            try
            {
                _gpioProvider.ShowTextOnDisplay(line, column, text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public bool ReadSensorStatus(int sensorAddress)
        {
            try
            {
                return _gpioProvider.ReadGpioStatus(sensorAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public void ClearDisplay()
        {
            _gpioProvider.ClearDisplay();
        }

        public void InitHardwareConnection(string ipaddress, ushort tcpPort, bool initGpio)
        {
            try
            {
                _modbusProvider.InitConnection(ipaddress, tcpPort);
                _gpioProvider.InitGpio(initGpio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
