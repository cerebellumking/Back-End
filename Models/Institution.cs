using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Institution
    {
        public Institution()
        {
            Followinstitutions = new HashSet<Followinstitution>();
        }

        public int InstitutionId { get; set; }
        public string InstitutionEmail { get; set; }
        public string InstitutionPhone { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionQualify { get; set; }
        public string InstitutionProfile { get; set; }
        public string InstitutionCity { get; set; }
        public string InstitutionLessons { get; set; }
        public string InstitutionIntroduction { get; set; }
        public string InstitutionLessonsCharacter { get; set; }
        public string InstitutionLocation { get; set; }
        public DateTime InstitutionCreatetime { get; set; }
        public string InstitutionProvince { get; set; }
        public string InstitutionTarget { get; set; }
        public string InstitutionPhoto { get; set; }

        public virtual ICollection<Followinstitution> Followinstitutions { get; set; }
    }
}
