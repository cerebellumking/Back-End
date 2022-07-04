using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Blogchecking
    {
        public int BlogId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime? BlogDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ReviewResult { get; set; }
        public string ReviewReason { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
