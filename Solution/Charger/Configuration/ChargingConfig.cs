using Charger.Interfaces;

namespace Charger.Configuration
{
    public class ChargingConfig : IChargingConfig
    {
        public double InactiveLimitVoltage { get; set; }
        public double ActiveLimitVoltage { get; set; }

        public void Initialize()
        {
            InactiveLimitVoltage = 0;
            ActiveLimitVoltage = 8;
        }
    }
}
