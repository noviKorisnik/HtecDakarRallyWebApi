using HtecDakarRallyWebApi.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class VehicleStat
    {
        private Vehicle _vehicle;
        public VehicleStat(Vehicle vehicle)
        {
            _vehicle = vehicle;
        }
        public int Id { get { return _vehicle.Id; } }
        public int RaceYear { get { return _vehicle.Race.Year; } }
        public string Team { get { return _vehicle.Team; } }
        public string Model { get { return _vehicle.Model; } }
        public int Manufactured { get { return _vehicle.Manufactured; } }
        public VehicleClassEnum Class{ get { return _vehicle.Class; } }
        public VehicleTypeEnum Type { get { return _vehicle.Type; } }
        public int MaxSpeed{ get { return _vehicle.MaxSpeed; } }
        public int RepairTime{ get { return _vehicle.RepairTime; } }
        public int LightMalfunction{ get { return _vehicle.LightMalfunction; } }
        public int HeavyMalfunction{ get { return _vehicle.HeavyMalfunction; } }
        public VehicleStatusEnum Status { get { return _vehicle.Status; } }
        public double Distance { get { return _vehicle.Distance; } }
        public TimeSpan Time { get { return _vehicle.Time; } }
        public TimeSpan? FinishTime{ get { return _vehicle.FinishTime; } }
        public int Repairments
        {
            get
            {
                return _vehicle.VehicleEvents.Count(e => e.Status == VehicleStatusEnum.InRepairment && e.Time < Time);
            }
        }
    }
}