namespace Charger.Interfaces
{
    public interface IChargingConfig
    {
        double InactiveLimitVoltage { get; set; }
        double ActiveLimitVoltage { get; set; }
    }
}
