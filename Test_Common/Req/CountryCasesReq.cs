using System;
using System.Collections.Generic;
using System.Text;

namespace Ncov_Common.Req
{
    public class CountryCasesReq
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
    }
}
