using Charger.Enums;
using Charger.Extensions;
using System;
using System.ComponentModel;

namespace Charger.Interfaces
{
    public interface IChargerLogic
    {
        StatusType ChargingStatus { get; set; }
        double ChargingLimit { get; set; }
        TimeSpan TimeToCharge { get; set; }
        bool DoorStatus { get; }
        bool ProcessInRunMode { get; }

        event EventHandler<MeasurementResultAvailableEventArgs> MeasurementResultAvailable;

        public event PropertyChangedEventHandler ChargingProcessParameterChanged;
        void StartProcess();
        void StopProcess();
    }
}