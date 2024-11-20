using Charger.Interfaces;
using ModbusTCP;
using System;

namespace Charger.Hardware.Connection
{
    public class ModbusProvider : IModbusProvider
    {
        private const byte UNIT_IDENTIFIER = 0x01;
        private Master _modbusMaster;

        public string IpAddress { get; set; }
        public ushort TcpPort { get; set; }
        public bool IsConnected { get => _modbusMaster.connected; }
        public ushort Timeout
        {
            get { return _modbusMaster.timeout; }
            set { _modbusMaster.timeout = value; }
        }

        public void InitConnection(string ipAddress, ushort tcpPort)
        {
            IpAddress = ipAddress;
            TcpPort = tcpPort;

            _modbusMaster = new Master(ipAddress, tcpPort);
            _modbusMaster.timeout = 2000;
            _modbusMaster.OnException += _modbusMaster_OnException;
        }

        private void _modbusMaster_OnException(ushort id, byte unit, byte function, byte exception)
        {
            Console.WriteLine("Fehler");
        }

        public void OpenConnection()
        {
            _modbusMaster.connect(IpAddress, TcpPort);
        }

        public void CloseConnection()
        {
            _modbusMaster.disconnect();
        }

        public byte[] ReadDigitalOutput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes)
        {
            byte[] data = new byte[numDataBytes];
            _modbusMaster.ReadCoils(guid, UNIT_IDENTIFIER, startAddress, numInputs, ref data);
            return data;
        }

        public byte[] ReadDigitalInput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes)
        {
            byte[] data = new byte[numDataBytes];
            _modbusMaster.ReadDiscreteInputs(guid, UNIT_IDENTIFIER, startAddress, numInputs, ref data);
            return data;
        }

        public byte[] ReadAnalogOutputs(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes)
        {
            byte[] data = new byte[numDataBytes];
            _modbusMaster.ReadHoldingRegister(guid, UNIT_IDENTIFIER, startAddress, numInputs, ref data);
            return data;
        }

        public byte[] ReadAnalogInput(ushort guid, ushort startAddress, ushort numInputs, int numDataBytes)
        {
            byte[] data = new byte[numDataBytes];
            _modbusMaster.ReadInputRegister(guid, UNIT_IDENTIFIER, startAddress, numInputs, ref data);
            return data;
        }

        public byte[] WriteSingleDigitalOutput(ushort guid, ushort startAddress, bool value, int numResultBytes)
        {
            byte[] result = new byte[numResultBytes];
            _modbusMaster.WriteSingleCoils(guid, UNIT_IDENTIFIER, startAddress, value, ref result);
            return result;
        }
        public byte[] WriteMultipleDigitalOutputs(ushort guid, ushort startAddress, bool value, int numResultBytes)
        {
            byte[] result = new byte[numResultBytes];
            _modbusMaster.WriteSingleCoils(guid, UNIT_IDENTIFIER, startAddress, value, ref result);
            return result;
        }

        public byte[] WriteSingleAnalogOutputRegister(ushort guid, ushort startAddress, byte[] values, int numResultBytes)
        {
            byte[] result = new byte[numResultBytes];
            _modbusMaster.WriteSingleRegister(guid, UNIT_IDENTIFIER, startAddress, values, ref result);
            return result;
        }
        public byte[] WriteMultipleAnalogOutputRegisters(ushort guid, ushort startAddress, byte[] values, int numResultBytes)
        {
            byte[] result = new byte[numResultBytes];
            _modbusMaster.WriteMultipleRegister(guid, UNIT_IDENTIFIER, startAddress, values, ref result);
            return result;
        }
    }
}
