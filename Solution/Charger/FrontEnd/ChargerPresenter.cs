using Charger.Enums;
using Charger.Extensions;
using Charger.Interfaces;
using System.ComponentModel;

namespace Charger.FrontEnd
{
    public class ChargerPresenter
    {
        private readonly IChargerLogic _chargerLogic;
        private readonly IManagementService _managementService;

        public ChargerPresenter(IChargerLogic chargerLogic, IManagementService managementService)
        {
            _chargerLogic = chargerLogic;
            _managementService = managementService;
        }

        public void Init()
        {
            _chargerLogic.ChargingProcessParameterChanged += GetChargingProcessParameter;
            _chargerLogic.MeasurementResultAvailable += GetMeasurementResult;
        }

        private void GetMeasurementResult(object sender, MeasurementResultAvailableEventArgs e)
        {
            if(e.ChargingStatus == StatusType.Charging)
            {
                _managementService.ClearDisplay();
                _managementService.SetDisplayText(1, 0, $"       Ladung       ");
            }
            else
            {
                switch (e.BatteryName)
                {
                    case BatteryType.BatteryOne:
                        _managementService.SetDisplayText(0, 0, $"Batterie 1: {e.Voltage.voltage}V   ");
                        break;
                    case BatteryType.BatteryTwo:
                        _managementService.SetDisplayText(1, 0, $"Batterie 2: {e.Voltage.voltage}V   ");
                        break;
                    case BatteryType.BatteryThree:
                        _managementService.SetDisplayText(2, 0, $"Batterie 3: {e.Voltage.voltage}V   ");
                        break;
                    case BatteryType.BatteryFour:
                        _managementService.SetDisplayText(3, 0, $"Batterie 4: {e.Voltage.voltage}V   ");
                        break;
                    default:
                        break;
                }
            }
        }

        private void GetChargingProcessParameter(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ChargingStatus":
                    switch (_chargerLogic.ChargingStatus)
                    {
                        case StatusType.Conservation:
                            _managementService.SetLedStatus(ControlType.LedBlue, false);
                            _managementService.SetLedStatus(ControlType.LedYellow, false);
                            _managementService.SetLedStatus(ControlType.LedGreen, true);
                            break;
                        case StatusType.Charging:
                            _managementService.SetLedStatus(ControlType.LedBlue, false);
                            _managementService.SetLedStatus(ControlType.LedYellow, true);
                            _managementService.SetLedStatus(ControlType.LedGreen, false);
                            break;
                        case StatusType.Observation:
                            _managementService.SetLedStatus(ControlType.LedBlue, true);
                            _managementService.SetLedStatus(ControlType.LedYellow, false);
                            _managementService.SetLedStatus(ControlType.LedGreen, false);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
