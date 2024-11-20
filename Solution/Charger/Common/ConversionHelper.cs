using Charger.Interfaces;
using System;

namespace Charger.Common
{
    public class ConversionHelper : IConversionHelper
    {
        const double COEFF_TO_SET_VOLTAGE = 0.0003051851;
        const int CLAMP_RESISTANCE = 120;
        public double ConvertBytesToVoltage(byte[] data)
        {
            short value = BitConverter.ToInt16(data, 0);
            return (double)value / 1000;
        }

        public byte[] ConvertVoltageToBytes(double voltage)
        {
            var adcValueAsBytes = (short)Math.Round(voltage / COEFF_TO_SET_VOLTAGE);
            return BitConverter.GetBytes(adcValueAsBytes);
        }

        public double CalculateOriginalVoltage(double voltage, double measureResistance)
        {
            return Math.Round(voltage / CLAMP_RESISTANCE * (CLAMP_RESISTANCE + measureResistance), 2);
        }
    }
}
