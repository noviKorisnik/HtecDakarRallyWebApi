using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System;

namespace HtecDakarRallyWebApi.Models
{
    public class Race
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public virtual double Distance
        {
            get { return DrConstants.RaceDistance; }
        }
        public RaceStatusEnum Status { get; set; }
        public virtual ICollection<Vehicle> Vehicles{get;set;}
        public DateTime? DateTime { get; set; }
        public TimeSpan Time { get; set; }
        public int Multiplier { get; set; }

        public virtual VehicleTypeEnum? LeaderboardType { get; set; }
    }
}