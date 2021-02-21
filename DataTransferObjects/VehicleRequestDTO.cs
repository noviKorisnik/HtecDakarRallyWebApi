using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    public class VehicleRequestDTO
    {
        [Required]
        public string Team { get; set; }
        [Required]
        public string Model { get; set; }
        [Range(DrConstants.OldestVehicle, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Manufactured { get; set; }
        [Required]
        public string Class { get; set; }
    }
}