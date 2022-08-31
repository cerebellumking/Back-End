using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Answercomment
    {
        public Answercomment()
        {
            Answercommentreports = new HashSet<Answercommentreport>();
            InverseAnswerCommentReplyNavigation = new HashSet<Answercomment>();
            Likeanswercomments = new HashSet<Likeanswercomment>();
        }

        public int AnswerCommentId { get; set; }
        public int AnswerCommentUserId { get; set; }
        public DateTime AnswerCommentTime { get; set; }
        public string AnswerCommentContent { get; set; }
        public int? AnswerCommentReply { get; set; }
        public int? AnswerCommentFather { get; set; }
        public decimal AnswerCommentLike { get; set; }
        public bool? AnswerCommentVisible { get; set; }

        public virtual Answer AnswerCommentFatherNavigation { get; set; }
        public virtual Answercomment AnswerCommentReplyNavigation { get; set; }
        public virtual User AnswerCommentUser { get; set; }
        public virtual ICollection<Answercommentreport> Answercommentreports { get; set; }
        public virtual ICollection<Answercomment> InverseAnswerCommentReplyNavigation { get; set; }
        public virtual ICollection<Likeanswercomment> Likeanswercomments { get; set; }
    }
}
