using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Starquestion
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public DateTime StarTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
