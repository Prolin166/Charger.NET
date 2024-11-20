using Charger.Domain.Models;
using Charger.Interfaces;
using Charger.Models;

namespace Charger.Configuration
{
    public class WagoConfig : IWagoConfig
    {
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public WagoPort ChargingSwitch { get; set; }
        public BatteryData BatteryOne { get; set; }
        public BatteryData BatteryTwo { get; set; }
        public BatteryData BatteryThree { get; set; }
        public BatteryData BatteryFour { get; set; }

        public void Initialize()
        {
            IpAddress = "198.168.X.X";
            Port = 502;

            ChargingSwitch = new WagoPort()
            {
                StartAddress = 0x00,
                AddressLength = 0x00
            };

            BatteryOne = new BatteryData()
            {
                Switch = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                },

                MeasureAddress = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                }
            };

            BatteryTwo = new BatteryData()
            {
                Switch = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                },

                MeasureAddress = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                }
            };

            BatteryThree = new BatteryData()
            {
                Switch = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                },

                MeasureAddress = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                }
            };

            BatteryFour = new BatteryData()
            {
                Switch = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                },

                MeasureAddress = new WagoPort()
                {
                    StartAddress = 0x00,
                    AddressLength = 0x00
                }
            };
        }
    }
}
