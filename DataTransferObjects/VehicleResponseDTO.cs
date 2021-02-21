namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class VehicleResponseDTO
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int Manufactured { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
    }
}