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
    public class VehicleRepository
    {
        private readonly DrDbContext _context;

        public VehicleRepository(DrDbContext context)
        {
            _context = context;
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
            try
            {
                await _context.GetUpdatedRace();
            }
            catch { }

            return await _context.FindVehicles(search);
        }
    }
}