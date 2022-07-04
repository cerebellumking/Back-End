﻿using System;
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
        public byte[] InstitutionQualify { get; set; }
        public byte[] InstitutionProfile { get; set; }
        public string InstitutionCity { get; set; }
        public string InstitutionLessons { get; set; }
        public string InstitutionIntroduction { get; set; }
        public string InstitutionLessonsCharacter { get; set; }
        public string InstitutionLocation { get; set; }
        public DateTime InstitutionCreatetime { get; set; }

        public virtual ICollection<Followinstitution> Followinstitutions { get; set; }
    }
}
