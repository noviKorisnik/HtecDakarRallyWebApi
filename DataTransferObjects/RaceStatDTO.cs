using System.Collections.Generic;
using System;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class RaceStatDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double Distance { get; set; }
        public string Status { get; set; }
        public DateTime? DateTime { get; set; }
        public string Time { get; set; }
        public string Multiplier { get; set; }
        public IDictionary<string, int> VehiclesByStatus { get; set; }
        public IDictionary<string, int> VehiclesByType { get; set; }

    }
}