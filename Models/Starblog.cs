using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Starblog
    {
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public DateTime StarTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
