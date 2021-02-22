using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System;
using HtecDakarRallyWebApi.Extensions;

namespace HtecDakarRallyWebApi.Models
{
    public class SearchParams
    {
        public int? Id { get; set; }
        public int? RaceId { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public int? ManufacturedFrom { get; set; }
        public int? ManufacturedTo { get; set; }
        public int? FromDistance { get; set; }
        public int? ToDistance { get; set; }
        public ICollection<VehicleClassEnum> Class { get; set; }
        public ICollection<VehicleTypeEnum> Type { get; set; }
        public ICollection<VehicleStatusEnum> Status { get; set; }
        public ICollection<SortOrderEnum> SortOrder { get; set; }
//        public GroupByEnum[] GroupBy { get; set; }
    }
}