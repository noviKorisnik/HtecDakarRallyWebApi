using HtecDakarRallyWebApi.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public virtual Race Race { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int Manufactured { get; set; }
        private VehicleClassEnum _class;
        public VehicleClassEnum Class
        {
            get
            {
                return _class;
            }
            set
            {
                value.Check();
                _class = value;
                Type = value.ToVehicleTypeEnum();
            }
        }
        public VehicleTypeEnum Type { get; set; }
        public virtual int MaxSpeed
        {
            get { return Class.MaxSpeed(); }
        }
        public virtual int RepairTime
        {
            get { return Type.RepairTime(); }
        }
        public virtual int LightMalfunction
        {
            get { return Class.Malfunction(MalfunctionTypeEnum.Light); }
        }
        public virtual int HeavyMalfunction
        {
            get { return Class.Malfunction(MalfunctionTypeEnum.Heavy); }
        }
        public virtual ICollection<VehicleEvent> VehicleEvents { get; set; }
        public VehicleStatusEnum Status { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public virtual TimeSpan? FinishTime
        {
            get
            {
                return Status == VehicleStatusEnum.Finished ? Time : null;
            }
        }
    }
}