using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class User
    {
        public User()
        {
            Answercommentreports = new HashSet<Answercommentreport>();
            Answercomments = new HashSet<Answercomment>();
            Answerreports = new HashSet<Answerreport>();
            Answers = new HashSet<Answer>();
            Blogcommentreports = new HashSet<Blogcommentreport>();
            Blogcomments = new HashSet<Blogcomment>();
            Blogreports = new HashSet<Blogreport>();
            Blogs = new HashSet<Blog>();
            Coinanswers = new HashSet<Coinanswer>();
            Coinblogs = new HashSet<Coinblog>();
            Followinstitutions = new HashSet<Followinstitution>();
            Followuniversities = new HashSet<Followuniversity>();
            FollowuserFollowUsers = new HashSet<Followuser>();
            FollowuserUsers = new HashSet<Followuser>();
            Likeanswercomments = new HashSet<Likeanswercomment>();
            Likeanswers = new HashSet<Likeanswer>();
            Likeblogcomments = new HashSet<Likeblogcomment>();
            Likeblogs = new HashSet<Likeblog>();
            Qualifications = new HashSet<Qualification>();
            Questions = new HashSet<Question>();
            Staranswers = new HashSet<Staranswer>();
            Starblogs = new HashSet<Starblog>();
            Starquestions = new HashSet<Starquestion>();
        }

        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public byte[] UserProfile { get; set; }
        public string UserGender { get; set; }
        public bool? UserState { get; set; }
        public string UserSignature { get; set; }
        public DateTime UserCreatetime { get; set; }
        public DateTime? UserBirthday { get; set; }
        public decimal UserFollower { get; set; }
        public decimal UserFollows { get; set; }
        public decimal UserLevel { get; set; }
        public decimal UserCoin { get; set; }

        public virtual ICollection<Answercommentreport> Answercommentreports { get; set; }
        public virtual ICollection<Answercomment> Answercomments { get; set; }
        public virtual ICollection<Answerreport> Answerreports { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Blogcommentreport> Blogcommentreports { get; set; }
        public virtual ICollection<Blogcomment> Blogcomments { get; set; }
        public virtual ICollection<Blogreport> Blogreports { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Coinanswer> Coinanswers { get; set; }
        public virtual ICollection<Coinblog> Coinblogs { get; set; }
        public virtual ICollection<Followinstitution> Followinstitutions { get; set; }
        public virtual ICollection<Followuniversity> Followuniversities { get; set; }
        public virtual ICollection<Followuser> FollowuserFollowUsers { get; set; }
        public virtual ICollection<Followuser> FollowuserUsers { get; set; }
        public virtual ICollection<Likeanswercomment> Likeanswercomments { get; set; }
        public virtual ICollection<Likeanswer> Likeanswers { get; set; }
        public virtual ICollection<Likeblogcomment> Likeblogcomments { get; set; }
        public virtual ICollection<Likeblog> Likeblogs { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Staranswer> Staranswers { get; set; }
        public virtual ICollection<Starblog> Starblogs { get; set; }
        public virtual ICollection<Starquestion> Starquestions { get; set; }
    }
}
