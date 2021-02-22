using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class ModelExtensions
    {
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
            {
                if (value != VehicleTypeEnum.None)
                {
                    dictionary.Add(value, race.Vehicles.Count(vehicle => vehicle.Type == value));
                };
            });
            return dictionary;
        }

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

        public static void SetStatus(this Race race, RaceStatusEnum? status = null)
        {
            if (status.HasValue)
            {
                race.Status = status.Value;
            }
            else
            {
                race.Status =
                    race.Vehicles.All(vehicle => vehicle.Status == VehicleStatusEnum.Pending)
                        ? RaceStatusEnum.Pending
                        : race.Vehicles.All(vehicle => vehicle.Status.IsFinishStatus())
                            ? RaceStatusEnum.Finished
                            : RaceStatusEnum.Running;
            }
        }
        public static void SetStatus(this Vehicle vehicle, VehicleStatusEnum? status = null)
        {
            if (status.HasValue)
            {
                vehicle.Status = status.Value;
            }
            else
            {
                try
                {
                    vehicle.Status =
                        vehicle.VehicleEvents
                            .Where(e => e.Time <= vehicle.Time)
                            .OrderByDescending(e => e.Time)
                            .FirstOrDefault()
                            .Status;
                }
                catch
                {
                    vehicle.Status = VehicleStatusEnum.Running;
                }
            }
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
            double avgSpeed = vehicle.avgSpeed();
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
        private static double avgSpeed(this Vehicle vehicle)
        {
            return (1 - DrConstants.Random.NextDouble() * DrConstants.Random.NextDouble()) * vehicle.MaxSpeed;
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

        public static IQueryable<Vehicle> SortOrder(this IQueryable<Vehicle> query, ICollection<SortOrderEnum> orderBy)
        {
            try
            {
                orderBy.Where(o => o != SortOrderEnum.None).ToList().ForEach(sort => query = query.SortBy(sort));
            }
            catch { }
            return query;
        }
        public static IQueryable<Vehicle> SortBy(this IQueryable<Vehicle> query, SortOrderEnum orderBy)
        {
            IOrderedQueryable<Vehicle> ordered = query as IOrderedQueryable<Vehicle>;
            switch (orderBy)
            {
                case SortOrderEnum.Id: try { return ordered.ThenBy(v => v.Id); } catch { return query.OrderBy(v => v.Id); }
                case SortOrderEnum.IdDesc: try { return ordered.ThenByDescending(v => v.Id); } catch { return query.OrderByDescending(v => v.Id); }
                case SortOrderEnum.Year: try { return ordered.ThenBy(v => v.Race.Year); } catch { return query.OrderBy(v => v.Race.Year); }
                case SortOrderEnum.YearDesc: try { return ordered.ThenByDescending(v => v.Race.Year); } catch { return query.OrderByDescending(v => v.Race.Year); }
                case SortOrderEnum.Team: try { return ordered.ThenBy(v => v.Team); } catch { return query.OrderBy(v => v.Team); }
                case SortOrderEnum.TeamDesc: try { return ordered.ThenByDescending(v => v.Team); } catch { return query.OrderByDescending(v => v.Team); }
                case SortOrderEnum.Model: try { return ordered.ThenBy(v => v.Model); } catch { return query.OrderBy(v => v.Model); }
                case SortOrderEnum.ModelDesc: try { return ordered.ThenByDescending(v => v.Model); } catch { return query.OrderByDescending(v => v.Model); }
                case SortOrderEnum.Manufactured: try { return ordered.ThenBy(v => v.Manufactured); } catch { return query.OrderBy(v => v.Manufactured); }
                case SortOrderEnum.ManufacturedDesc: try { return ordered.ThenByDescending(v => v.Manufactured); } catch { return query.OrderByDescending(v => v.Manufactured); }
                case SortOrderEnum.Class: try { return ordered.ThenBy(v => v.Class); } catch { return query.OrderBy(v => v.Class); }
                case SortOrderEnum.ClassDesc: try { return ordered.ThenByDescending(v => v.Class); } catch { return query.OrderByDescending(v => v.Class); }
                case SortOrderEnum.Type: try { return ordered.ThenBy(v => v.Type); } catch { return query.OrderBy(v => v.Type); }
                case SortOrderEnum.TypeDesc: try { return ordered.ThenByDescending(v => v.Type); } catch { return query.OrderByDescending(v => v.Type); }
                case SortOrderEnum.Status: try { return ordered.ThenBy(v => v.Status); } catch { return query.OrderBy(v => v.Status); }
                case SortOrderEnum.StatusDesc: try { return ordered.ThenByDescending(v => v.Status); } catch { return query.OrderByDescending(v => v.Status); }
                case SortOrderEnum.Distance: try { return ordered.ThenBy(v => v.Distance); } catch { return query.OrderBy(v => v.Distance); }
                case SortOrderEnum.DistanceDesc: try { return ordered.ThenByDescending(v => v.Distance); } catch { return query.OrderByDescending(v => v.Distance); }
                case SortOrderEnum.FinishTime: try { return ordered.ThenBy(v => v.FinishTime); } catch { return query.OrderBy(v => v.FinishTime); }
                case SortOrderEnum.FinishTimeDesc: try { return ordered.ThenByDescending(v => v.FinishTime); } catch { return query.OrderByDescending(v => v.FinishTime); }
                default: return query;
            };
        }
    }
}