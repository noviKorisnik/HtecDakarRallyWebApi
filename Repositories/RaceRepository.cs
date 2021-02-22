using HtecDakarRallyWebApi.Models;
using System.Threading.Tasks;
using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Repositories
{
    public class RaceRepository
    {
        private readonly DrDbContext _context;

        public RaceRepository(DrDbContext context)
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
    }
}