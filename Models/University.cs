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
        }

        public int UniversityId { get; set; }
        public string UniversityEmail { get; set; }
        public string UniversityBadge { get; set; }
        public string UniversityName { get; set; }
        public string UniversityRegion { get; set; }
        public string UniversityCountry { get; set; }
        public string UniversityLocation { get; set; }
        public string UniversityIntroduction { get; set; }
        public decimal UniversityStudentNum { get; set; }
        public string UniversityWebsite { get; set; }
        public string UniversityCollege { get; set; }
        public string UniversityAbbreviation { get; set; }
        public short UniversityQsRank { get; set; }
        public short UniversityTheRank { get; set; }
        public short UniversityUsnewsRank { get; set; }
        public short Year { get; set; }

        public virtual ICollection<Followuniversity> Followuniversities { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }
    }
}
