using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class DrExtensions
    {
        public static TimeSpan CurrentTimeSpan(this Race race, uint? newMultiplier = null)
        {
            DateTime now = DateTime.Now;
            TimeSpan current =
                race.DateTime.HasValue
                ? race.Multiplier * (now - race.DateTime.Value) + race.Time
                : new TimeSpan();
            if (newMultiplier.HasValue && newMultiplier.Value > 0)
            {
                race.Multiplier = (int)newMultiplier;
            }
            race.DateTime = now;
            race.Time = current;

            return current;
        }

        public static void UpdateToTime(this Vehicle vehicle, TimeSpan toTime)
        {
            if (vehicle.Status.IsFinishStatus())
            {
                return;
            }

            VehicleEvent lastEvent = vehicle.VehicleEvents.OrderBy(e => e.Time).LastOrDefault();
            if (lastEvent == null || lastEvent.Time < toTime)
            {
                vehicle.createEvents(lastEvent == null ? new TimeSpan() : lastEvent.Time, toTime);
            }

            lastEvent = vehicle.VehicleEvents.OrderBy(e => e.Time).LastOrDefault();
            if (lastEvent.Status == VehicleStatusEnum.Broken && lastEvent.Time < toTime)
            {
                toTime = lastEvent.Time;
            }

            TimeSpan runningTime = vehicle.getRunningTime(toTime);
            double avgSpeed = vehicle.AvgSpeed();
            double distance = avgSpeed * runningTime.TotalHours;

            if (distance > vehicle.Race.Distance - vehicle.Distance)
            {
                runningTime = new TimeSpan((long)((vehicle.Race.Distance - vehicle.Distance) / avgSpeed * TimeSpan.TicksPerHour));
                distance = vehicle.Race.Distance;
                toTime = vehicle.getToTime(runningTime);
                vehicle.VehicleEvents.Add(new VehicleEvent()
                {
                    VehicleId = vehicle.Id,
                    Status = VehicleStatusEnum.Finished,
                    Time = toTime,
                });

                vehicle.VehicleEvents.Where(e => e.Time > toTime).ToList().ForEach(e => vehicle.VehicleEvents.Remove(e));
            }
            else
            {
                distance += vehicle.Distance;
            }

            vehicle.Time = toTime;
            vehicle.Distance = distance;

            vehicle.SetStatus();
        }
        private static TimeSpan getToTime(this Vehicle vehicle, TimeSpan runningTime)
        {
            TimeSpan time = new TimeSpan();
            TimeSpan fromTime = vehicle.Time;
            TimeSpan toTime = fromTime;
            VehicleStatusEnum status = vehicle.Status;
            while (true)
            {
                VehicleEvent ev = vehicle.VehicleEvents.Where(e => e.Time > fromTime).OrderBy(e => e.Time).FirstOrDefault();
                if (status == VehicleStatusEnum.Running)
                {
                    if (ev.Time - fromTime < runningTime - time)
                    {
                        time += ev.Time - fromTime;
                    }
                    else
                    {
                        return (ev.Time + runningTime - time);
                    }
                }
                status = ev.Status;
                fromTime = ev.Time;
            }
        }
        private static TimeSpan getRunningTime(this Vehicle vehicle, TimeSpan toTime)
        {
            TimeSpan time = new TimeSpan();
            TimeSpan fromTime = vehicle.Time;
            VehicleStatusEnum status = vehicle.Status;
            while (fromTime < toTime)
            {
                VehicleEvent ev = vehicle.VehicleEvents.Where(e => e.Time > fromTime).OrderBy(e => e.Time).FirstOrDefault();
                if (status == VehicleStatusEnum.Running)
                {
                    time += (toTime < ev.Time ? toTime : ev.Time) - fromTime;
                }
                status = ev.Status;
                fromTime = ev.Time;
            }
            return time;
        }
        private static void createEvents(this Vehicle vehicle, TimeSpan fromTime, TimeSpan toTime, bool created = false)
        {
            if (fromTime > toTime && created)
            {
                return;
            };

            VehicleEvent candidate = vehicle.nextEventCandidate();

            TimeSpan fromHour = new TimeSpan((int)(fromTime.TotalHours) * TimeSpan.TicksPerHour);

            if (candidate != null)
            {
                candidate.Time += fromHour;

                if (candidate.Time > fromTime)
                {
                    vehicle.VehicleEvents.Add(candidate);

                    if (candidate.Status == VehicleStatusEnum.InRepairment)
                    {
                        VehicleEvent runner = new VehicleEvent()
                        {
                            VehicleId = vehicle.Id,
                            Status = VehicleStatusEnum.Running,
                            Time = candidate.Time + new TimeSpan(vehicle.RepairTime * TimeSpan.TicksPerHour),
                        };
                        vehicle.VehicleEvents.Add(runner);

                        vehicle.createEvents(runner.Time, toTime, true);
                    }
                }
            }
            else
            {
                vehicle.createEvents(fromHour + new TimeSpan(TimeSpan.TicksPerHour), toTime, created);
            }
        }
        private static VehicleEvent nextEventCandidate(this Vehicle vehicle, MalfunctionTypeEnum? malfunctionType = null)
        {
            if (malfunctionType.HasValue)
            {
                if (vehicle.Class.Malfunction(malfunctionType.Value) > DrConstants.Random.Next(100))
                {
                    return new VehicleEvent()
                    {
                        VehicleId = vehicle.Id,
                        Status = malfunctionType.Value.ToVehicleStatus(),
                        Time = new TimeSpan((long)(TimeSpan.TicksPerHour * DrConstants.Random.NextDouble())),
                    };
                }
                return null;
            }
            VehicleEvent light = vehicle.nextEventCandidate(MalfunctionTypeEnum.Light);
            VehicleEvent heavy = vehicle.nextEventCandidate(MalfunctionTypeEnum.Heavy);

            return
                light == null && heavy == null
                    ? null
                    : light == null
                        ? heavy
                        : heavy == null
                            ? light
                            : light.Time < heavy.Time
                                ? light
                                : heavy;
        }
        public static VehicleStatusEnum ToVehicleStatus(this MalfunctionTypeEnum malfunctionType)
        {
            return
                malfunctionType == MalfunctionTypeEnum.Light
                ? VehicleStatusEnum.InRepairment
                : VehicleStatusEnum.Broken;
        }

        public async static Task<Race> GetRaceWithVehicles(this DrDbContext context, int? raceId = null)
        {
            Race race = await
                context
                .Races
                .Include("Vehicles.VehicleEvents")
                .Where(r => raceId.HasValue ? r.Id == raceId.Value : r.Status == RaceStatusEnum.Running)
                .SingleOrDefaultAsync();
            if (race == null)
            {
                throw new DrException(string.Format("Race not found."));
            }
            return race;
        }
        public async static Task<Race> GetUpdatedRace(this DrDbContext context, int? raceId = 0, VehicleTypeEnum? vehicleType = null)
        {
            Race race = await context.GetRaceWithVehicles(raceId);
            if (race.Status == RaceStatusEnum.Running)
            {
                TimeSpan ts = race.CurrentTimeSpan();
                race.Vehicles
                    .Where(vehicle => !vehicleType.HasValue || vehicle.Type == vehicleType.Value)
                    .ToList().ForEach(vehicle => vehicle.UpdateToTime(ts));

                race.SetStatus();

                await context.SaveChangesAsync();
            }
            race.LeaderboardType = vehicleType;

            return race;
        }

        public static IDictionary<VehicleStatusEnum, int> getVehiclesByStatus(this Race race)
        {
            Dictionary<VehicleStatusEnum, int> dictionary = new Dictionary<VehicleStatusEnum, int>();
            Enum.GetValues<VehicleStatusEnum>().ToList().ForEach(value =>
                dictionary.Add(value, race.Vehicles.Count(vehicle => vehicle.Status == value))
            );
            return dictionary;
        }

        public static IDictionary<VehicleTypeEnum, int> getVehiclesByType(this Race race)
        {
            Dictionary<VehicleTypeEnum, int> dictionary = new Dictionary<VehicleTypeEnum, int>();
            Enum.GetValues<VehicleTypeEnum>().ToList().ForEach(value =>
                dictionary.Add(value, race.Vehicles.Count(vehicle => vehicle.Type == value))
            );
            return dictionary;
        }

        public static object DrMsgObject(this Exception exception, bool callback = false)
        {
            if (exception == null)
            {
                return null;
            }
            if (exception is DrException)
            {
                return new { Message = exception.Message, };
            }
            object result = exception.InnerException.DrMsgObject(true);
            if (result != null)
            {
                return result;
            }
            return callback ? null : new { Message = exception.Message, };
        }

    }
}