using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class DbModelExtensions
    {

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

        public static double AvgSpeed(this Vehicle vehicle)
        {
            return (1 - DrConstants.Random.NextDouble() * DrConstants.Random.NextDouble()) * vehicle.MaxSpeed;
        }
    }
}