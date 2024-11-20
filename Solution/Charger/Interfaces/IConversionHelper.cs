namespace Charger.Interfaces
{
    public interface IConversionHelper
    {
        double ConvertBytesToVoltage(byte[] data);
        byte[] ConvertVoltageToBytes(double voltage);
        double CalculateOriginalVoltage(double voltage, double measureResistance);
    }
}