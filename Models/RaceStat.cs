using HtecDakarRallyWebApi.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class RaceStat
    {
        private Race _race;
        public RaceStat(Race race)
        {
            _race = race;
        }
        public int Id { get { return _race.Id; } }
        public int Year { get { return _race.Year; } }
        public double Distance { get { return _race.Distance; } }
        public RaceStatusEnum Status { get { return _race.Status; } }
        public DateTime? DateTime { get { return _race.DateTime; } }
        public TimeSpan Time { get { return _race.Time; } }
        public int Multiplier { get { return _race.Multiplier; } }
        public IDictionary<VehicleStatusEnum, int> VehiclesByStatus
        {
            get
            {
                return _race.getVehiclesByStatus();
            }
        }
        public IDictionary<VehicleTypeEnum, int> VehiclesByType
        {
            get
            {
                return _race.getVehiclesByType();
            }
        }
    }
}