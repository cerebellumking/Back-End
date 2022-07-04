using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Coinanswer
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public DateTime CoinTime { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
    }
}
