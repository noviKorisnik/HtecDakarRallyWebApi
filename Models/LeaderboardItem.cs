using HtecDakarRallyWebApi.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class LeaderboardItem
    {
        private Vehicle _vehicle;
        public LeaderboardItem(Vehicle vehicle)
        {
            _vehicle = vehicle;
        }
        public int Position { get; set; }
        public string Team { get { return _vehicle.Team; } }
        public string Model { get { return _vehicle.Model; } }
        public int Manufactured { get { return _vehicle.Manufactured; } }
        public VehicleClassEnum Class { get { return _vehicle.Class; } }
        public VehicleTypeEnum Type { get { return _vehicle.Type; } }
        public VehicleStatusEnum Status { get { return _vehicle.Status; } }
        public double Distance { get { return _vehicle.Distance; } }
        public TimeSpan Time { get { return _vehicle.Time; } }
        public TimeSpan? FinishTime { get { return _vehicle.FinishTime; } }
    }
}