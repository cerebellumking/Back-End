using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Qualificationchecking
    {
        public int IdentityId { get; set; }
        public int AdministratorId { get; set; }
        public string ReviewReason { get; set; }
        public string ReviewResult { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime SummitDate { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Qualification Identity { get; set; }
    }
}
