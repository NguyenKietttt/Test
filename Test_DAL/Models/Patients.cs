using System;
using System.Collections.Generic;

namespace Test_DAL.Models
{
    public partial class Patients
    {
        public string PatientId { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }

        public virtual Cities CityNameNavigation { get; set; }
        public virtual Countries Country { get; set; }
    }
}
