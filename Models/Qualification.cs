using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Qualification
    {
        public int IdentityId { get; set; }
        public int UserId { get; set; }
        public int UniversityId { get; set; }
        public string IdentityQualificationImage { get; set; }
        public bool? Visible { get; set; }
        public string Identity { get; set; }
        public string EnrollmentTime { get; set; }
        public string Major { get; set; }

        public virtual University University { get; set; }
        public virtual User User { get; set; }
        public virtual Qualificationchecking Qualificationchecking { get; set; }
    }
}
