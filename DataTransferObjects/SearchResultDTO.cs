using System.Collections.Generic;
using System;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class SearchResultDTO
    {
        public DateTime Time { get; set; }
        public SearchParmsDTO Search { get; set; }
        public int Count { get; set; }
        public ICollection<VehicleDTO> Results { get; set; }
    }
}