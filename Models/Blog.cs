using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Blog
    {
        public Blog()
        {
            Blogcomments = new HashSet<Blogcomment>();
            Blogreports = new HashSet<Blogreport>();
            Coinblogs = new HashSet<Coinblog>();
            Likeblogs = new HashSet<Likeblog>();
            Starblogs = new HashSet<Starblog>();
        }

        public int BlogId { get; set; }
        public int? BlogUserId { get; set; }
        public string BlogTag { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogContent { get; set; }
        public string BlogImage { get; set; }
        public decimal? BlogLike { get; set; }
        public decimal? BlogCoin { get; set; }
        public bool? BlogVisible { get; set; }

        public virtual User BlogUser { get; set; }
        public virtual Blogchecking Blogchecking { get; set; }
        public virtual ICollection<Blogcomment> Blogcomments { get; set; }
        public virtual ICollection<Blogreport> Blogreports { get; set; }
        public virtual ICollection<Coinblog> Coinblogs { get; set; }
        public virtual ICollection<Likeblog> Likeblogs { get; set; }
        public virtual ICollection<Starblog> Starblogs { get; set; }
    }
}
