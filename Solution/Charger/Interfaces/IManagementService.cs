using Charger.Enums;
using Charger.Models;
using System.Collections.Generic;

namespace Charger.Interfaces
{
    public interface IManagementService
    {
        IDictionary<ControlType, ControlData> Controls { get; set; }
        void SetLedStatus(ControlType led, bool isOn);
        void SetDisplayText(int line, int column, string text);
        void ClearDisplay();
        bool GetDoorSensorStatus();
    }
}
