using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HtecDakarRallyWebApi.DataTransferObjects;
using AutoMapper;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using HtecDakarRallyWebApi.Repositories;

namespace HtecDakarRallyWebApi.Services
{
    public class RaceService
    {
        private readonly DrDbContext _context;
        private readonly RaceRepository _repository;
        private readonly IMapper _mapper;

        public RaceService(DrDbContext context, RaceRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        //1. Create race
        //(parameters: year)
        public async Task<RaceDTO> CreateRace(RaceRequestDTO raceDTO)
        {
            return _mapper.Map<Race, RaceDTO>
            (
                await _repository.CreateRace(_mapper.Map<RaceRequestDTO, Race>(raceDTO))
            );
        }

        //2. Add vehicle to the race
        //available only prior to the race start
        //(parameters: vehicle)
        public async Task<VehicleDTO> AddVehicle(int raceId, VehicleRequestDTO vehicleDTO)
        {
            return _mapper.Map<Vehicle, VehicleDTO>
            (
                await _repository.AddVehicle(raceId, _mapper.Map<VehicleRequestDTO, Vehicle>(vehicleDTO))
            );
        }

        //5. Start the race
        //only one race can run at the time
        //(parameters: race identifier)
        public void StartRace(int raceId)
        {
            _repository.StartRace(raceId);
        }

        //6. Get leaderboard including all vehicles
        public async Task<LeaderboardDTO> GetLeaderboard(int? raceId = null)
        {
            return _mapper.Map<Leaderboard, LeaderboardDTO>(await _repository.GetLeaderboard(raceId));
        }

        //7. Get leaderboard for specific vehicle type: cars, trucks, motorcycles
        //(parameters: type)
        public async Task<LbByTypeDTO> GetLeaderboard(int? raceId, string vehicleType)
        {
            return _mapper.Map<Leaderboard, LbByTypeDTO>(await _repository.GetLeaderboard(raceId, vehicleType));
        }

        //10. Get race status that includes:
        //race status (pending, running, finished),
        //number of vehicles grouped by vehicle status,
        //number of vehicles grouped by vehicle type
        //(parameters: race identifier)
        public async Task<RaceStatDTO> GetStatus(int raceId){
            return _mapper.Map<RaceStat, RaceStatDTO>(await _repository.GetStatus(raceId));
        }

        //Mock
        public async Task<object> Mock()
        {
            await _repository.CreateRace(new Race() { Year = 1997, });

            for (int i = 1; i <= 5; i++)
            {
                await _repository.AddVehicle(1, new Vehicle()
                {
                    Team = "team" + i.ToString(),
                    Model = "model",
                    Manufactured = 1900,
                    Class = (Enumerations.VehicleClassEnum)i,
                });
            }

            _repository.SetMultiplier(60);

            _repository.StartRace(1);

            return true;
        }

        public void SetMultiplier(uint multiplier)
        {
            if (multiplier <= 0)
            {
                throw new DrException("Must be positive value");
            }
            _repository.SetMultiplier(multiplier);
        }
    }
}