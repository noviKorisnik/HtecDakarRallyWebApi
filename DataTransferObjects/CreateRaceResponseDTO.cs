using System.Collections.Generic;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class CreateRaceResponseDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
    }
}