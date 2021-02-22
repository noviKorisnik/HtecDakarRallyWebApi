using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class SearchParams
    {
        public int? RaceId { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public int? Id { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int? ManufacturedFrom { get; set; }
        public int? ManufacturedTo { get; set; }
        public VehicleClassEnum[] Class { get; set; }
        public VehicleTypeEnum[] Type { get; set; }
        public VehicleStatusEnum[] Status { get; set; }
        public int? FromDistance { get; set; }
        public int? ToDistance { get; set; }
        public SortOrderEnum[] SortOrder { get; set; }
//        public GroupByEnum[] GroupBy { get; set; }
    }
}