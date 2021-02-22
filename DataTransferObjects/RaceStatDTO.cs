using System.Collections.Generic;
using System;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class RaceStatDTO : RaceDTO
    {
        public IDictionary<string, int> VehiclesByStatus { get; set; }
        public IDictionary<string, int> VehiclesByType { get; set; }

    }
}