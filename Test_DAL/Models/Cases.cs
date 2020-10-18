using System;
using System.Collections.Generic;

namespace Test_DAL.Models
{
    public partial class Cases
    {
        public DateTime Date { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
        public string CountryId { get; set; }

        public virtual Countries Country { get; set; }
    }
}
