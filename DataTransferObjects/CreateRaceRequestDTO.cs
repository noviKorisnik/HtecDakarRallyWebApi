using System.ComponentModel.DataAnnotations;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class CreateRaceRequestDTO
    {
        [Required]
        [Range(DrConstants.RaceStartYear, DrConstants.RaceEndYear)]
        public int Year { get; set; }
    }
}