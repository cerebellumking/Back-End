using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Followinstitution
    {
        public int InstitutionId { get; set; }
        public int UserId { get; set; }
        public DateTime FollowTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual User User { get; set; }
    }
}
