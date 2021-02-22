using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class SearchResult
    {
        public SearchResult(SearchParams search, ICollection<Vehicle> results)
        {
            Results = results;
            Search = search;
        }
        public DateTime Time { get { return DateTime.Now; } }
        public SearchParams Search { get; private set; }
        public int Count { get { return Results.Count; } }
        public ICollection<Vehicle> Results { get; private set; }
    }
}