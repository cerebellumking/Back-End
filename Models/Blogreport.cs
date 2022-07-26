using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Blogreport
    {
        public int ReportId { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? ReportAnswerDate { get; set; }
        public bool? ReportAnswerResult { get; set; }
        public string ReportReason { get; set; }
        public bool? ReportState { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
