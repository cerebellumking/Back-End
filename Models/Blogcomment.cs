using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Blogcomment
    {
        public Blogcomment()
        {
            Blogcommentreports = new HashSet<Blogcommentreport>();
            InverseBlogCommentReplyNavigation = new HashSet<Blogcomment>();
            Likeblogcomments = new HashSet<Likeblogcomment>();
        }

        public int BlogCommentId { get; set; }
        public int? BlogCommentUserId { get; set; }
        public DateTime BlogCommentTime { get; set; }
        public string BlogCommentContent { get; set; }
        public int? BlogCommentReply { get; set; }
        public int? BlogCommentFather { get; set; }
        public decimal? BlogCommentLike { get; set; }
        public bool? BlogCommentVisible { get; set; }

        public virtual Blog BlogCommentFatherNavigation { get; set; }
        public virtual Blogcomment BlogCommentReplyNavigation { get; set; }
        public virtual User BlogCommentUser { get; set; }
        public virtual ICollection<Blogcommentreport> Blogcommentreports { get; set; }
        public virtual ICollection<Blogcomment> InverseBlogCommentReplyNavigation { get; set; }
        public virtual ICollection<Likeblogcomment> Likeblogcomments { get; set; }
    }
}
