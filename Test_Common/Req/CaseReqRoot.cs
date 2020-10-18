using System;
using System.Collections.Generic;
using System.Text;

namespace Ncovi_Common.Req
{
    public class CaseReqRoot
    {
        public CaseReqGlobal Global { get; set; }
        public List<CaseReqByCountry> Countries { get; set; }
        public DateTime Date { get; set; }
    }
}
