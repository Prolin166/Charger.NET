namespace Charger.Interfaces
{
    public interface IModbusProvider
    {
        string IpAddress { get; }
        ushort TcpPort { get; }
        bool IsConnected { get; }
        ushort Timeout { get; set; }

        void InitConnection(string ipAddress, ushort tcpPort);
        void CloseConnection();
        void OpenConnection();
        byte[] ReadAnalogInput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes);
        byte[] ReadAnalogOutputs(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes);
        byte[] ReadDigitalInput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes);
        byte[] ReadDigitalOutput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes);
        byte[] WriteMultipleAnalogOutputRegisters(ushort guid, ushort startAddress, byte[] values, int numResultBytes);
        byte[] WriteMultipleDigitalOutputs(ushort guid, ushort startAddress, bool value, int numResultBytes);
        byte[] WriteSingleAnalogOutputRegister(ushort guid, ushort startAddress, byte[] values, int numResultBytes);
        byte[] WriteSingleDigitalOutput(ushort guid, ushort startAddress, bool value, int numResultBytes);
    }
}