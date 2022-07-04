using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Followuniversity
    {
        public int UniversityId { get; set; }
        public int UserId { get; set; }
        public DateTime FollowTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual University University { get; set; }
        public virtual User User { get; set; }
    }
}
