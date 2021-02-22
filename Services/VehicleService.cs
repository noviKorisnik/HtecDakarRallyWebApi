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
    public class VehicleService
    {
        private readonly DrDbContext _context;
        private readonly VehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehicleService(DrDbContext context, VehicleRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        //3. Update vehicle info
        //available only prior to the race start
        //(parameters: vehicle)
        public async Task UpdateVehicle(int vehicleId, VehicleRequestDTO vehicleDTO)
        {
            await _repository.UpdateVehicle(vehicleId, _mapper.Map<VehicleRequestDTO, Vehicle>(vehicleDTO));
        }

        //4. Remove vehicle from the race
        //available only prior to the race start
        //(parameters: vehicle identifier)
        public async Task RemoveVehicle(int vehicleId)
        {
            await _repository.RemoveVehicle(vehicleId);
        }

        //8. Get vehicle statistics:
        //distance,
        //malfunction statistics,
        //status,
        //finish time
        //(parameters: vehicle identifier)
        public async Task<VehicleStatDTO> GetStatistics(int vehicleId)
        {
            return _mapper.Map<VehicleStat, VehicleStatDTO>(await _repository.GetStatistics(vehicleId));
        }

        //9. Find vehicle(s)
        //(parameters: team
        //AND/OR model
        //AND/OR manufacturing date
        //AND/OR status
        //AND/OR distance,
        //sort order)
        public async Task<SearchResultDTO> FindVehicles(SearchParmsDTO search)
        {
            return _mapper.Map<SearchResult, SearchResultDTO>(await _repository.FindVehicles(_mapper.Map<SearchParmsDTO, SearchParams>(search)));
        }
    }
}