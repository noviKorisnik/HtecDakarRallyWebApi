using System.Text.RegularExpressions;
using System;

namespace HtecDakarRallyWebApi
{
    public static class DrConstants
    {
        public static readonly Random Random = new Random();
        public static readonly Regex IsNumericRegex = new Regex(@"^\d+$");
        public const int RaceStartYear = 1979, RaceEndYear = 2979, OldestVehicle = 1884;
        public const double RaceDistance = 10000;
    }
}