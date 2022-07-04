using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Blogcommentreport
    {
        public int ReportId { get; set; }
        public int BlogCommentId { get; set; }
        public int UserId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? ReportAnswerDate { get; set; }
        public bool? ReportAnswerResult { get; set; }
        public string ReportReason { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Blogcomment BlogComment { get; set; }
        public virtual User User { get; set; }
    }
}
