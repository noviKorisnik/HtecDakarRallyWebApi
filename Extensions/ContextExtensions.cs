using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class ContextExtensions
    {
        private async static Task<Race> getRaceWithVehicles(this DrDbContext context, int? raceId = null)
        {
            Race race = await
                context
                .Races
                .Include("Vehicles.VehicleEvents")
                .Where(r => raceId.HasValue ? (r.Id == raceId.Value) : (r.Status == RaceStatusEnum.Running))
                .SingleOrDefaultAsync();
            if (race == null)
            {
                throw new DrException(string.Format("Race not found."));
            }
            return race;
        }
        public async static Task<Race> GetUpdatedRace(this DrDbContext context, int? raceId = null, VehicleTypeEnum? vehicleType = null)
        {
            Race race = await context.getRaceWithVehicles(raceId);
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

        public static async Task<SearchResult> FindVehicles(this DrDbContext context, SearchParams search)
        {
            IQueryable<Vehicle> query = context.Vehicles.Include("Race").Where
            (
                vehicle => true
                    && (!search.Id.HasValue || vehicle.Id == search.Id.Value)
                    && (!search.RaceId.HasValue || vehicle.RaceId == search.RaceId.Value)
                    && (string.IsNullOrWhiteSpace(search.Team) || vehicle.Team.Contains(search.Team, StringComparison.InvariantCultureIgnoreCase))
                    && (string.IsNullOrWhiteSpace(search.Team) || vehicle.Team.Contains(search.Team, StringComparison.InvariantCultureIgnoreCase))
                    && (!search.FromYear.HasValue || vehicle.Race.Year >= search.FromYear.Value)
                    && (!search.ToYear.HasValue || vehicle.Race.Year <= search.ToYear.Value)
                    && (!search.ManufacturedFrom.HasValue || vehicle.Manufactured >= search.ManufacturedFrom.Value)
                    && (!search.ManufacturedTo.HasValue || vehicle.Manufactured <= search.ManufacturedTo.Value)
                    && (!search.FromDistance.HasValue || vehicle.Distance >= search.FromDistance.Value)
                    && (!search.ToDistance.HasValue || vehicle.Distance <= search.ToDistance.Value)
                    && (search.Class == null || search.Class.Count == 0 || search.Class.Contains(vehicle.Class))
                    && (search.Type == null || search.Type.Count == 0 || search.Type.Contains(vehicle.Type))
                    && (search.Status == null || search.Status.Count == 0 || search.Status.Contains(vehicle.Status))
            )
            .SortOrder(search.SortOrder);
            
            return new SearchResult(search, await query.ToListAsync());
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
        public static void CheckNoRunningRace(this DrDbContext context)
        {
            if (context.Races.Any(race => race.Status == RaceStatusEnum.Running))
            {
                throw new DrException(string.Format("There is already running race."));
            }
        }
    }
}