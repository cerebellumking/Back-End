using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Followuser
    {
        public int FollowUserId { get; set; }
        public int UserId { get; set; }
        public DateTime FollowTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual User FollowUser { get; set; }
        public virtual User User { get; set; }
    }
}
