using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Likeblogcomment
    {
        public int BlogCommentId { get; set; }
        public int UserId { get; set; }
        public DateTime LikeTime { get; set; }
        public bool? Cancel { get; set; }

        public virtual Blogcomment BlogComment { get; set; }
        public virtual User User { get; set; }
    }
}
