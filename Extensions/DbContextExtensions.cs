using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class DbContextExtensions
    {

        public async static Task CheckRaceYear(this DrDbContext context, int year)
        {
            if (await context.Races.AnyAsync(race => race.Year == year))
            {
                throw new DrException(string.Format("Race year must be unique. There is already race for year {0}.", year));
            }
        }
        public async static Task<Race> GetRace(this DrDbContext context, int? raceId = null, bool throwException = false)
        {
            Race race =
                raceId.HasValue
                ? await context.Races.Where(r => r.Id == raceId).SingleOrDefaultAsync()
                : await context.Races.Where(r => r.Status == RaceStatusEnum.Running).SingleOrDefaultAsync();
            if (race == null && throwException)
            {
                throw new DrException(string.Format("Race not found."));
            }
            return race;
        }
        public async static Task<Race> CheckRacePending(this DrDbContext context, int raceId)
        {
            Race race = await context.GetRace(raceId, true);
            if (race.Status != RaceStatusEnum.Pending)
            {
                throw new DrException(string.Format("Race is not in pending state."));
            }
            return race;
        }

        public async static Task<Vehicle> GetUpdatedVehicle(this DrDbContext context, int vehicleId)
        {
            Vehicle vehicle = await context
                .Vehicles
                .Include("Race")
                .Include("VehicleEvents")
                .Where(v => v.Id == vehicleId)
                .FirstOrDefaultAsync();

            if (vehicle == null)
            {
                throw new DrException("Vehicle not found.");
            }

            if (vehicle.Race.Status == RaceStatusEnum.Running)
            {
                vehicle.UpdateToTime(vehicle.Race.CurrentTimeSpan());

                vehicle.Race.SetStatus();

                await context.SaveChangesAsync();
            }
            return vehicle;
        }
        public async static Task<Vehicle> GetVehicle(this DrDbContext context, int vehicleId, bool throwException = false)
        {
            Vehicle vehicle = await context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null && throwException)
            {
                throw new DrException(string.Format("Vehicle not found."));
            }
            return vehicle;
        }
        public static void CheckVehicle(this DrDbContext context, Vehicle vehicle)
        {
            Race race = context.Races.Find(vehicle.RaceId);
            if (race.Status != RaceStatusEnum.Pending)
            {
                throw new DrException("Can't update vehicle from non-pending race.");
            }
            if (context.Vehicles.Any(v => v.RaceId == vehicle.RaceId && v.Team == vehicle.Team && v.Id != vehicle.Id))
            {
                throw new DrException(string.Format("Team name should be unique within race."));
            }
            if (race.Year < vehicle.Manufactured)
            {
                throw new DrException(string.Format("Vehicle cannot be manufactured after race year."));
            }
        }
        public static void Check(this Vehicle vehicle)
        {
        }
        public static void CheckNoRunningRace(this DrDbContext context)
        {
            if (context.Races.Any(race => race.Status == RaceStatusEnum.Running))
            {
                throw new DrException(string.Format("There is already running race."));
            }
        }
    }
}