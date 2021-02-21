using System.Collections.Generic;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class VehicleStatDTO
    {
        public int Id { get; set; }
        public int RaceYear { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int Manufactured { get; set; }
        public string Class{ get; set; }
        public string Type { get; set; }
        public int MaxSpeed{ get; set; }
        public int RepairTime{ get; set; }
        public int LightMalfunction{ get; set; }
        public int HeavyMalfunction{ get; set; }
        public string Status { get; set; }
        public double Distance { get; set; }
        public string Time { get; set; }
        public string FinishTime{ get; set; }
        public int Repairments{ get; set; }
    }
}