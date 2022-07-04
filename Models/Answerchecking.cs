using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Answerchecking
    {
        public int AnswerId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ReviewResult { get; set; }
        public string ReviewReason { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
