using HtecDakarRallyWebApi.Enumerations;
using System;
using System.Linq;

namespace HtecDakarRallyWebApi.Models
{
    public class VehicleEvent
    {
        private DrDbContext _context;
        public VehicleEvent(DrDbContext context = null)
        {
            SetContext(context);
        }
        public void SetContext(DrDbContext context = null)
        {
            _context = context;
        }

        public int Id { get; set; }
        public int VehicleId { get; set; }
        private Vehicle vehicle;
        public virtual Vehicle Vehicle
        {
            get
            {
                if (vehicle == null || vehicle.Id != VehicleId)
                {
                    vehicle = _context.Vehicles.Find(VehicleId);
                }
                return vehicle;
            }
            set
            {
                VehicleId = value.Id;
            }
        }
        public VehicleStatusEnum Status { get; set; }
        public TimeSpan Time { get; set; }
    }
}