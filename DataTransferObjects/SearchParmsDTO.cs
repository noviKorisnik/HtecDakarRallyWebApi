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
        public bool AndOr { get; set; }//true means OR, false means AND, for example
        public string Team { get; set; }
        public string Model { get; set; }
        public int? Manufactured { get; set; }
        public string Status { get; set; }
        public double Distance { get; set; }
        public string SortOrder { get; set; }
    }
}