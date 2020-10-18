using System;
using System.Collections.Generic;

namespace Test_DAL.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Patients = new HashSet<Patients>();
        }

        public string CityName { get; set; }
        public int? TotalCase { get; set; }
        public int? Active { get; set; }
        public int? Recovered { get; set; }
        public int? Deaths { get; set; }

        public virtual ICollection<Patients> Patients { get; set; }
    }
}
