using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Likeanswercomment
    {
        public int AnswerCommentId { get; set; }
        public int UserId { get; set; }
        public DateTime LikeTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Answercomment AnswerComment { get; set; }
        public virtual User User { get; set; }
    }
}
