using System;
using System.Collections.Generic;
using System.Text;

namespace Ncov_Common.Req
{
    public class CasesReqByID
    {
        public string Date { get; set; }
        public int? NewConfirmed { get; set; }
        public int? NewRecovered { get; set; }
    }
}
