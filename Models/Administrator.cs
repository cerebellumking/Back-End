using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Administrator
    {
        public Administrator()
        {
            Answercheckings = new HashSet<Answerchecking>();
            Answercommentreports = new HashSet<Answercommentreport>();
            Answerreports = new HashSet<Answerreport>();
            Blogcheckings = new HashSet<Blogchecking>();
            Blogcommentreports = new HashSet<Blogcommentreport>();
            Blogreports = new HashSet<Blogreport>();
            Qualificationcheckings = new HashSet<Qualificationchecking>();
            Questioncheckings = new HashSet<Questionchecking>();
        }

        public int AdministratorId { get; set; }
        public string AdministratorEmail { get; set; }
        public string AdministratorPhone { get; set; }
        public string AdministratorPassword { get; set; }
        public string AdministratorName { get; set; }
        public string AdministratorProfile { get; set; }
        public string AdministratorGender { get; set; }
        public DateTime AdministratorCreatetime { get; set; }

        public virtual ICollection<Answerchecking> Answercheckings { get; set; }
        public virtual ICollection<Answercommentreport> Answercommentreports { get; set; }
        public virtual ICollection<Answerreport> Answerreports { get; set; }
        public virtual ICollection<Blogchecking> Blogcheckings { get; set; }
        public virtual ICollection<Blogcommentreport> Blogcommentreports { get; set; }
        public virtual ICollection<Blogreport> Blogreports { get; set; }
        public virtual ICollection<Qualificationchecking> Qualificationcheckings { get; set; }
        public virtual ICollection<Questionchecking> Questioncheckings { get; set; }
    }
}
