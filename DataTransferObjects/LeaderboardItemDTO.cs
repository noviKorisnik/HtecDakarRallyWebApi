using System.Collections.Generic;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class LeaderboardItemDTO
    {
//        public int Position { get; set; }
        public string Team { get; set; }

//        public string Model { get; set; }
//        public int Manufactured { get; set; }

//        public string Class { get; set; }
//        public string Type { get; set; }
        public double Distance { get; set; }
//        public string FinishTime { get; set; }
        public string Status { get; set; }
    }
}