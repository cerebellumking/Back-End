using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Moneychangerecord
    {
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public int ChangeNum { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeReason { get; set; }

        public virtual User User { get; set; }
    }
}
