using System;
using System.Collections.Generic;

namespace Test_DAL.Models
{
    public partial class News
    {
        public int NewId { get; set; }
        public DateTime? Date { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
