using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsFinishStatus(this VehicleStatusEnum status)
        {
            return status == VehicleStatusEnum.Broken || status == VehicleStatusEnum.Finished;
        }

        public static VehicleStatusEnum ToVehicleStatus(this MalfunctionTypeEnum malfunctionType)
        {
            return
                malfunctionType == MalfunctionTypeEnum.Light
                ? VehicleStatusEnum.InRepairment
                : VehicleStatusEnum.Broken;
        }
        public static VehicleTypeEnum ToVehicleTypeEnum(this VehicleClassEnum vehicleClass)
        {
            switch (vehicleClass)
            {
                case VehicleClassEnum.SportsCar:
                case VehicleClassEnum.TerrainCar:
                    return VehicleTypeEnum.Car;
                case VehicleClassEnum.Truck:
                    return VehicleTypeEnum.Truck;
                case VehicleClassEnum.CrossMotorcycle:
                case VehicleClassEnum.SportMotorcycle:
                    return VehicleTypeEnum.Motorcycle;
                default:
                    throw new DrException("Unsupported vehicle type.");
            }
        }

        public static void Check(this VehicleClassEnum vehicleClass)
        {
            if (vehicleClass == VehicleClassEnum.None || DrConstants.IsNumericRegex.IsMatch(vehicleClass.ToString()))
            {
                throw new DrException("Unsupported vehicle class.");
            }
        }

        public static int MaxSpeed(this VehicleClassEnum vehicleClass)
        {
            switch (vehicleClass)
            {
                case VehicleClassEnum.SportsCar: return 140;
                case VehicleClassEnum.TerrainCar: return 100;
                case VehicleClassEnum.Truck: return 80;
                case VehicleClassEnum.CrossMotorcycle: return 85;
                case VehicleClassEnum.SportMotorcycle: return 130;
                default: throw new DrException("Unsupported vehicle class.");
            };
        }
        public static int RepairTime(this VehicleTypeEnum vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleTypeEnum.Car: return 5;
                case VehicleTypeEnum.Truck: return 7;
                case VehicleTypeEnum.Motorcycle: return 3;
                default: throw new DrException("Unsupported vehicle type.");
            }
        }
        public static int Malfunction(this VehicleClassEnum vehicleClass, MalfunctionTypeEnum type)
        {
            bool light = type == MalfunctionTypeEnum.Light;
            switch (vehicleClass)
            {
                case VehicleClassEnum.SportsCar: return light ? 12 : 2;
                case VehicleClassEnum.TerrainCar: return light ? 3 : 1;
                case VehicleClassEnum.Truck: return light ? 6 : 4;
                case VehicleClassEnum.CrossMotorcycle: return light ? 3 : 2;
                case VehicleClassEnum.SportMotorcycle: return light ? 18 : 10;
                default: throw new DrException("Unsupported vehicle class.");
            };
        }
    }
}