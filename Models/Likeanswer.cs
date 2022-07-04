using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Likeanswer
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public DateTime LikeTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
    }
}
