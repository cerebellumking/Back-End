using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class University
    {
        public University()
        {
            Followuniversities = new HashSet<Followuniversity>();
            Qualifications = new HashSet<Qualification>();
            Ranks = new HashSet<Rank>();
        }

        public int UniversityId { get; set; }
        public string UniversityEmail { get; set; }
        public string UniversityBadge { get; set; }
        public string UniversityEnName { get; set; }
        public string UniversityRegion { get; set; }
        public string UniversityCountry { get; set; }
        public string UniversityLocation { get; set; }
        public string UniversityIntroduction { get; set; }
        public decimal UniversityStudentNum { get; set; }
        public string UniversityWebsite { get; set; }
        public string UniversityCollege { get; set; }
        public string UniversityAbbreviation { get; set; }
        public string UniversityChName { get; set; }
        public decimal UniversityAddressX { get; set; }
        public decimal UniversityAddressY { get; set; }
        public short UniversityTeacherNum { get; set; }
        public string UniversityTuition { get; set; }
        public byte UniversityTofelRequirement { get; set; }
        public decimal UniversityIeltsRequirement { get; set; }
        public string UniversityPhoto { get; set; }

        public virtual ICollection<Followuniversity> Followuniversities { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
