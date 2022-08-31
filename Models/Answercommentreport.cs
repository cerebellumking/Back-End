using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Answercommentreport
    {
        public int ReportId { get; set; }
        public int AnswerCommentId { get; set; }
        public int UserId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime ReportAnswerDate { get; set; }
        public bool? ReportAnswerResult { get; set; }
        public string ReportReason { get; set; }
        public bool? ReportState { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Answercomment AnswerComment { get; set; }
        public virtual User User { get; set; }
    }
}
