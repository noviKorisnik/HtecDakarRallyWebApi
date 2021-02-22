using System.Collections.Generic;

namespace HtecDakarRallyWebApi.DataTransferObjects
{
    //9. Find vehicle(s)
    //(parameters: team
    //AND/OR model
    //AND/OR manufacturing date
    //AND/OR status
    //AND/OR distance,
    //sort order)
    public class SearchParmsDTO
    {
        public int? RaceId { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public int? Id { get; set; }
        public string Team { get; set; }
        public string Model { get; set; }
        public int? ManufacturedFrom { get; set; }
        public int? ManufacturedTo { get; set; }
        public string[] Class { get; set; }
        public string[] Type { get; set; }
        public string[] Status { get; set; }
        public int? FromDistance { get; set; }
        public int? ToDistance { get; set; }
        public string[] SortOrder { get; set; }
//        public string[] GroupBy { get; set; }
    }
}