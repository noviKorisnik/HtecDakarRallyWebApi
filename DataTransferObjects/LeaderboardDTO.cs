using System.Collections.Generic;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class LeaderboardDTO
    {
        public string DateTime { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public double Distance { get; set; }
        public int Multiplier { get; set; }
        public IEnumerable<LeaderboardItemDTO> RaceOrder { get; set; }
    }
}