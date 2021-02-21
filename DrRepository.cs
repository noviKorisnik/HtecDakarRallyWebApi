using HtecDakarRallyWebApi.Models;
using System.Threading.Tasks;
using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi
{
    public class DrRepository
    {
        private readonly DrDbContext _context;

        public DrRepository(DrDbContext context)
        {
            _context = context;
        }

        //1. Create race
        //(parameters: year)
        public async Task<Race> CreateRace(Race race)
        {
            if (_context.Races.Any(r => r.Year == race.Year))
            {
                throw new DrException(string.Format("Race year must be unique. There is already race for year {0}.", race.Year));
            }

            _context.Races.Add(race);

            await _context.SaveChangesAsync();

            return race;
        }

        //2. Add vehicle to the race
        //available only prior to the race start
        //(parameters: vehicle)
        public async Task<Vehicle> AddVehicle(int raceId, Vehicle vehicle)
        {
            Race race = await _context.CheckRacePending(raceId);

            vehicle.RaceId = raceId;
            _context.Vehicles.Add(vehicle);

            _context.CheckVehicle(vehicle);

            await _context.SaveChangesAsync();

            return vehicle;
        }

        //3. Update vehicle info
        //available only prior to the race start
        //(parameters: vehicle)
        public async Task UpdateVehicle(int vehicleId, Vehicle update)
        {
            Vehicle vehicle = await _context.GetVehicle(vehicleId, true);

            vehicle.Team = update.Team;
            vehicle.Model = update.Model;
            vehicle.Manufactured = update.Manufactured;
            vehicle.Class = update.Class;

            _context.CheckVehicle(vehicle);

            await _context.SaveChangesAsync();
        }

        //4. Remove vehicle from the race
        //available only prior to the race start
        //(parameters: vehicle identifier)
        public async Task RemoveVehicle(int vehicleId)
        {
            Vehicle vehicle = await _context.GetVehicle(vehicleId, true);
            if (_context.Races.Find(vehicle.RaceId).Status != RaceStatusEnum.Pending)
            {
                throw new DrException("Can't remove vehicle from non-pending race.");
            }
            vehicle.Race.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        private static object locker = new object();
        private static uint _multiplier = 1;
        //5. Start the race
        //only one race can run at the time
        //(parameters: race identifier)
        public void StartRace(int raceId)
        {
            lock (locker)
            {
                _context.CheckNoRunningRace();
                Race race = _context.CheckRacePending(raceId).Result;
                race.SetStatus(RaceStatusEnum.Running);
                race.CurrentTimeSpan(_multiplier);
                _context.Vehicles.Where(v => v.RaceId == raceId).ToList()
                .ForEach(vehicle => vehicle.SetStatus(VehicleStatusEnum.Running));
                _context.SaveChanges();
            }
        }

        //6. Get leaderboard including all vehicles
        //7. Get leaderboard for specific vehicle type: cars, trucks, motorcycles
        //(parameters: type)
        public async Task<Leaderboard> GetLeaderboard(int? raceId = null, string vehicleType = null)
        {
            VehicleTypeEnum? vehicleTypeEnum = null;
            try
            {
                vehicleTypeEnum = Enum.Parse<VehicleTypeEnum>(vehicleType, true);
            }
            catch { }

            return new Leaderboard(await _context.GetUpdatedRace(raceId, vehicleTypeEnum));
        }

        //8. Get vehicle statistics:
        //distance,
        //malfunction statistics,
        //status,
        //finish time
        //(parameters: vehicle identifier)
        public async Task<VehicleStat> GetStatistics(int vehicleId)
        {
            return new VehicleStat(await _context.GetUpdatedVehicle(vehicleId));
        }

        //9. Find vehicle(s)
        //(parameters: team
        //AND/OR model
        //AND/OR manufacturing date
        //AND/OR status
        //AND/OR distance,
        //sort order)
        public async Task<SearchResult> FindVehicles(SearchParams search)
        {
            await _context.GetUpdatedRace();

            throw new NotImplementedException();
        }

        //10. Get race status that includes:
        //race status (pending, running, finished),
        //number of vehicles grouped by vehicle status,
        //number of vehicles grouped by vehicle type
        //(parameters: race identifier)
        public async Task<RaceStat> GetStatus(int raceId)
        {
            return new RaceStat(await _context.GetUpdatedRace(raceId));
        }


        public void SetMultiplier(uint multiplier)
        {
            _multiplier = multiplier;
            Race race = _context.GetRace().Result;
            if (race != null)
            {
                race.CurrentTimeSpan(_multiplier);
                _context.SaveChanges();
            }

        }


        //test
        public async Task<List<Vehicle>> GetAllVehicles()
        {
            var nj = await _context.Vehicles.ToListAsync();

            return nj;
        }
        public async Task<List<Race>> GetAllRaces()
        {
            return await _context.Races.ToListAsync();
        }
    }
}