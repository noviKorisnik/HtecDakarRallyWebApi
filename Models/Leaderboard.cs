using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class Leaderboard
    {
        private Race _race;
        public Leaderboard(Race race)
        {
            _race = race;
        }
        public DateTime? DateTime { get { return _race.DateTime; } }
        public TimeSpan Time { get { return _race.Time; } }
        public RaceStatusEnum Status { get { return _race.Status; } }
        public VehicleTypeEnum? LeaderboardType { get { return _race.LeaderboardType; } }
        public int Year { get { return _race.Year; } }
        public double Distance { get { return _race.Distance; } }
        public int Multiplier { get { return _race.Multiplier; } }
        public IEnumerable<LeaderboardItem> RaceOrder
        {
            get
            {
                List<LeaderboardItem> items =
                    _race
                    .Vehicles
                    .Where(vehicle => !LeaderboardType.HasValue || vehicle.Type == LeaderboardType.Value)
                    .OrderBy(vehicle => vehicle.FinishTime.HasValue ? 0 : 1)
                    .ThenBy(vehicle => vehicle.FinishTime.GetValueOrDefault())
                    .ThenByDescending(vehicle => vehicle.Distance)
                    .Select(vehicle => new LeaderboardItem(vehicle))
                    .ToList();

                int position = 0;
                items.ForEach(item => item.Position = ++position);

                return items;
            }
        }
    }
}