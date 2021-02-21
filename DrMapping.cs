using AutoMapper;
using HtecDakarRallyWebApi.Models;
using HtecDakarRallyWebApi.DataTransferObjects;

namespace HtecDakarRallyWebApi
{
    public class DrMapping : Profile
    {
        public DrMapping()
        {
            CreateMap<CreateRaceRequestDTO, Race>();
            CreateMap<Race, CreateRaceResponseDTO>();

            CreateMap<VehicleRequestDTO, Vehicle>();
            CreateMap<Vehicle, VehicleResponseDTO>();

            CreateMap<LeaderboardItem, LeaderboardItemDTO>();
            CreateMap<Leaderboard, LeaderboardDTO>();
            CreateMap<Leaderboard, LbByTypeDTO>();

            CreateMap<VehicleStat, VehicleStatDTO>();

            CreateMap<SearchParmsDTO, SearchParams>();
            CreateMap<SearchResult, SearchResultDTO>();

            CreateMap<RaceStat, RaceStatDTO>();
        }
    }
}