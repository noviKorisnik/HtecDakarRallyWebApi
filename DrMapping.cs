using AutoMapper;
using HtecDakarRallyWebApi.Models;
using HtecDakarRallyWebApi.DataTransferObjects;

namespace HtecDakarRallyWebApi
{
    public class DrMapping : Profile
    {
        public DrMapping()
        {
            CreateMap<RaceRequestDTO, Race>();
            CreateMap<Race, RaceDTO>();

            CreateMap<VehicleRequestDTO, Vehicle>();
            CreateMap<Vehicle, VehicleDTO>();

            CreateMap<LeaderboardItem, LeaderboardItemDTO>();
            CreateMap<Leaderboard, LeaderboardDTO>();
            CreateMap<Leaderboard, LbByTypeDTO>();

            CreateMap<VehicleStat, VehicleStatDTO>();

            CreateMap<SearchParmsDTO, SearchParams>();
            CreateMap<SearchParams, SearchParmsDTO>();
            CreateMap<SearchResult, SearchResultDTO>();

            CreateMap<RaceStat, RaceStatDTO>();
        }
    }
}