using System.ComponentModel.DataAnnotations;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class RaceRequestDTO
    {
        [Required]
        [Range(DrConstants.RaceStartYear, DrConstants.RaceEndYear)]
        public int Year { get; set; }
    }
}