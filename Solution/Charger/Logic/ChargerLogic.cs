using Charger.Enums;
using Charger.Extensions;
using Charger.Interfaces;
using Charger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Charger.Logic
{
    public class ChargerLogic : IChargerLogic
    {
        private readonly IBatteryService _batteryService;
        private readonly IManagementService _managementService;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        private StatusType _chargingStatus;
        private bool _doorStatus;
        private double _chargingLimit;
        private TimeSpan _timeToCharge;
        private bool _processInRunMode;

        public event EventHandler<MeasurementResultAvailableEventArgs> MeasurementResultAvailable;
        public event PropertyChangedEventHandler ChargingProcessParameterChanged;

        public Dictionary<BatteryType, Stopwatch> BatteriesToCharge = new Dictionary<BatteryType, Stopwatch>();

        public StatusType ChargingStatus { get => _chargingStatus; set { _chargingStatus = value; OnPropertyChanged(); } }
        public double ChargingLimit { get => _chargingLimit; set { _chargingLimit = value; OnPropertyChanged(); } }
        public TimeSpan TimeToCharge { get => _timeToCharge; set { _timeToCharge = value; OnPropertyChanged(); } }
        public bool DoorStatus { get => _doorStatus; private set { _doorStatus = value; OnPropertyChanged(); } }
        public bool ProcessInRunMode { get => _processInRunMode; private set { _processInRunMode = value; OnPropertyChanged(); } }

        public ChargerLogic(IBatteryService batteryService, IManagementService managementService)
        {
            _batteryService = batteryService;
            _managementService = managementService;
        }

        public void StartProcess()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _processInRunMode = true;
            ChargingProcess();
        }

        public void StopProcess()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
            _batteryService.SwitchAllBatteryToNull();
            BatteriesToCharge.Clear();
            _processInRunMode = false;
        }

        private void ChargingProcess()
        {
            Task task = new Task(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    ReadAndSetDoorSensorStatus();
                    BatteriesToCharge.Clear();
                    _batteryService.SwitchAllBatteryToNull();
                    Console.WriteLine("Status = " + ChargingStatus);
                    switch (ChargingStatus)
                    {
                        case StatusType.Conservation:
                            StatusConservation();
                            break;
                        case StatusType.Charging:
                            StatusCharging();
                            break;
                        case StatusType.Observation:
                            StatusObservation();
                            break;
                        default:
                            break;
                    }
                }
            }, _cancellationToken);
            task.Start();
        }
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            ChargingProcessParameterChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void StatusCharging()
        {
            while (ChargingStatus == StatusType.Charging)
            {
                var data = new MeasurementResultAvailableEventArgs()
                {
                    ChargingStatus = ChargingStatus
                };

                MeasurementResultAvailable?.Invoke(this, data);
                foreach (var battery in _batteryService.Batteries.Keys)
                {
                    if (_batteryService.BatteryIsAvailable(battery))
                    {
                        Thread.Sleep(200);
                        _batteryService.SwitchBatteryRelay(battery, true);
                    }
                    ReadAndSetDoorSensorStatus();
                }
            }
        }

        private void StatusObservation()
        {
            while (ChargingStatus == StatusType.Observation)
            {
                foreach (var battery in _batteryService.Batteries.Keys)
                {
                    var value = _batteryService.GetBatteryVoltage(battery);
                    var data = new MeasurementResultAvailableEventArgs()
                    {
                        ChargingStatus = ChargingStatus,
                        BatteryName = battery,
                        Voltage = new VoltageSensor
                        {
                            voltage = value
                        }
                    };

                    MeasurementResultAvailable?.Invoke(this, data);
                    DoorStatus = _managementService.GetDoorSensorStatus();
                }
            }
        }

        private void StatusConservation()
        {
            while (ChargingStatus == StatusType.Conservation)
            {
                foreach (var battery in _batteryService.Batteries.Keys)
                {
                    double value = 0;
                    if (BatteriesToCharge.ContainsKey(battery))
                    {
                        if (BatteriesToCharge[battery].Elapsed >= _timeToCharge)
                        {
                            BatteriesToCharge.Remove(battery);
                            _batteryService.SwitchBatteryRelay(battery, false);
                        }
                    }

                    Thread.Sleep(200);

                    if (!BatteriesToCharge.ContainsKey(battery))
                    {
                        value = _batteryService.GetBatteryVoltage(battery);
                        var data = new MeasurementResultAvailableEventArgs()
                        {
                            ChargingStatus = ChargingStatus,
                            BatteryName = battery,
                            Voltage = new VoltageSensor
                            {
                                voltage = value
                            }
                        };

                        MeasurementResultAvailable?.Invoke(this, data);

                        Thread.Sleep(200);

                        if (value < _chargingLimit)
                        {
                            if (!BatteriesToCharge.ContainsKey(battery) && _batteryService.BatteryIsAvailable(battery))
                            {
                                BatteriesToCharge.Add(battery, Stopwatch.StartNew());
                                _batteryService.SwitchBatteryRelay(battery, true);
                            }
                        }
                    }
                    ReadAndSetDoorSensorStatus();
                }
            }
        }

        private bool ReadAndSetDoorSensorStatus()
        {
            DoorStatus = _managementService.GetDoorSensorStatus();
            ChargingProcessParameterChanged?.Invoke(this, new PropertyChangedEventArgs("ALL"));
            if (DoorStatus == false)
                ChargingStatus = StatusType.Observation;

            return DoorStatus;
        }

    }
}
