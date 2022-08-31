using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Answer
    {
        public Answer()
        {
            Answercomments = new HashSet<Answercomment>();
            Answerreports = new HashSet<Answerreport>();
            Coinanswers = new HashSet<Coinanswer>();
            Likeanswers = new HashSet<Likeanswer>();
            Staranswers = new HashSet<Staranswer>();
        }

        public int AnswerId { get; set; }
        public int AnswerUserId { get; set; }
        public int QuestionId { get; set; }
        public DateTime AnswerDate { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerContentpic { get; set; }
        public decimal AnswerLike { get; set; }
        public decimal AnswerCoin { get; set; }
        public bool? AnswerVisible { get; set; }
        public string AnswerSummary { get; set; }

        public virtual User AnswerUser { get; set; }
        public virtual Question Question { get; set; }
        public virtual Answerchecking Answerchecking { get; set; }
        public virtual ICollection<Answercomment> Answercomments { get; set; }
        public virtual ICollection<Answerreport> Answerreports { get; set; }
        public virtual ICollection<Coinanswer> Coinanswers { get; set; }
        public virtual ICollection<Likeanswer> Likeanswers { get; set; }
        public virtual ICollection<Staranswer> Staranswers { get; set; }
    }
}
