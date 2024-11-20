using Charger.Enums;
using Charger.Interfaces;
using Charger.Models;
using System.Collections.Generic;

namespace Charger.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IHardwareManager _hardwareManager;

        public IDictionary<ControlType, ControlData> Controls { get; set; }

        public ManagementService(IHardwareManager hardwareManager)
        {
            _hardwareManager = hardwareManager;
        }

        public void SetDisplayText(int line, int column, string text)
        {
            _hardwareManager.SetDisplayText(line, column, text);
        }

        public void ClearDisplay()
        {
            _hardwareManager.ClearDisplay();
        }

        public void SetLedStatus(ControlType control, bool isOn)
        {
            Controls.TryGetValue(control, out ControlData controlConfig);
            _hardwareManager.SwitchLed(isOn, controlConfig.ControlAddress);
        }

        public bool GetDoorSensorStatus()
        {
            Controls.TryGetValue(ControlType.DoorSensor, out ControlData controlConfig);
            return _hardwareManager.ReadSensorStatus(controlConfig.ControlAddress);
        }
    }
}
