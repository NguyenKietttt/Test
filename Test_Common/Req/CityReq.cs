using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ncov_Common.Req
{
    public class CityReq
    {
        public string CityName { get; set; }
        public int? TotalCase { get; set; }
        public int? Active { get; set; }
        public int? Recovered { get; set; }
        public int? Deaths { get; set; }
    }
}
