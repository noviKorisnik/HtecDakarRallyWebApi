using HtecDakarRallyWebApi.Enumerations;
using HtecDakarRallyWebApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HtecDakarRallyWebApi.Extensions
{
    public static class MiscExtensions
    {

        public static void GenerateVehicles(this Race race, int count)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    race.Vehicles.Add(Generator.Vehicle(race));
                }
                catch { }
            }
        }

        public static T Some<T>(this List<T> list)
        {
            return list[DrConstants.Random.Next(list.Count)];
        }

        public static object DrMsgObject(this Exception exception, bool callback = false)
        {
            if (exception == null)
            {
                return null;
            }
            if (exception is DrException)
            {
                return new { Message = exception.Message, };
            }
            object result = exception.InnerException.DrMsgObject(true);
            if (result != null)
            {
                return result;
            }
            return callback ? null : new { Message = exception.Message, };
        }
    }
}