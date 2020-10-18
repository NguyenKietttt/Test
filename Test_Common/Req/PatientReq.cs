using System;
using System.Collections.Generic;
using System.Text;

namespace Ncov_Common.Req
{
    public class PatientReq
    {
        public string PatientId { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
    }
}
