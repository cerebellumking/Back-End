using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            Starquestions = new HashSet<Starquestion>();
        }

        public int QuestionId { get; set; }
        public int? QuestionUserId { get; set; }
        public string QuestionTag { get; set; }
        public DateTime QuestionDate { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public decimal? QuestionReward { get; set; }
        public int? QuestionApply { get; set; }
        public string QuestionImage { get; set; }
        public bool? QuestionVisible { get; set; }

        public virtual User QuestionUser { get; set; }
        public virtual Questionchecking Questionchecking { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Starquestion> Starquestions { get; set; }
    }
}
