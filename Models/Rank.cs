using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Rank
    {
        public int UniversityId { get; set; }
        public short RankYear { get; set; }
        public short UniversityQsRank { get; set; }
        public short UniversityTheRank { get; set; }
        public short UniversityUsnewsRank { get; set; }

        public virtual University University { get; set; }
    }
}
