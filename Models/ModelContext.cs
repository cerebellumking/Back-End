using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
#nullable disable

namespace Back_End.Models
{
    public partial class ModelContext : DbContext
    {
        private readonly IConfiguration _Configuration;//读取配置文件

        public ModelContext(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration configuration)
            : base(options)
        {
            _Configuration = configuration;
        }

        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Answerchecking> Answercheckings { get; set; }
        public virtual DbSet<Answercomment> Answercomments { get; set; }
        public virtual DbSet<Answercommentreport> Answercommentreports { get; set; }
        public virtual DbSet<Answerreport> Answerreports { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Blogchecking> Blogcheckings { get; set; }
        public virtual DbSet<Blogcomment> Blogcomments { get; set; }
        public virtual DbSet<Blogcommentreport> Blogcommentreports { get; set; }
        public virtual DbSet<Blogreport> Blogreports { get; set; }
        public virtual DbSet<Coinanswer> Coinanswers { get; set; }
        public virtual DbSet<Coinblog> Coinblogs { get; set; }
        public virtual DbSet<Followinstitution> Followinstitutions { get; set; }
        public virtual DbSet<Followuniversity> Followuniversities { get; set; }
        public virtual DbSet<Followuser> Followusers { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }
        public virtual DbSet<Likeanswer> Likeanswers { get; set; }
        public virtual DbSet<Likeanswercomment> Likeanswercomments { get; set; }
        public virtual DbSet<Likeblog> Likeblogs { get; set; }
        public virtual DbSet<Likeblogcomment> Likeblogcomments { get; set; }
        public virtual DbSet<Moneychangerecord> Moneychangerecords { get; set; }
        public virtual DbSet<Newsflash> Newsflashes { get; set; }
        public virtual DbSet<Qualification> Qualifications { get; set; }
        public virtual DbSet<Qualificationchecking> Qualificationcheckings { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Questionchecking> Questioncheckings { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Staranswer> Staranswers { get; set; }
        public virtual DbSet<Starblog> Starblogs { get; set; }
        public virtual DbSet<Starquestion> Starquestions { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public void DetachAll()
        {
            //循环遍历DbContext中所有被跟踪的实体
            while (true)
            {
                //每次循环获取DbContext中一个被跟踪的实体
                var currentEntry = ChangeTracker.Entries().FirstOrDefault();

                //currentEntry不为null，就将其State设置为EntityState.Detached，即取消跟踪该实体
                if (currentEntry != null)
                {
                    //设置实体State为EntityState.Detached，取消跟踪该实体，之后dbContext.ChangeTracker.Entries().Count()的值会减1
                    currentEntry.State = EntityState.Detached;
                }
                //currentEntry为null，表示DbContext中已经没有被跟踪的实体了，则跳出循环
                else
                {
                    break;
                }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string _DataSource = _Configuration["DataSource"];
                string _Password = _Configuration["Password"];
                string _UserID = _Configuration["UserID"];
                optionsBuilder.UseOracle("Data Source=" + _DataSource + ";Password=" + _Password + ";User ID=" + _UserID + ";");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOE");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.ToTable("Administrator");

                entity.HasIndex(e => e.AdministratorName, "SYS_C0010164")
                    .IsUnique();

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorCreatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("ADMINISTRATOR_CREATETIME")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.Property(e => e.AdministratorEmail)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_EMAIL")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AdministratorGender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_GENDER");

                entity.Property(e => e.AdministratorName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_NAME")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AdministratorPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_PASSWORD")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AdministratorPhone)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_PHONE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AdministratorProfile)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("ADMINISTRATOR_PROFILE");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("ANSWER");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerCoin)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ANSWER_COIN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerContent)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("ANSWER_CONTENT")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AnswerContentpic)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("ANSWER_CONTENTPIC")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.AnswerLike)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ANSWER_LIKE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerSummary)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("ANSWER_SUMMARY");

                entity.Property(e => e.AnswerUserId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("ANSWER_VISIBLE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionId)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_ID")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.AnswerUser)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.AnswerUserId)
                    .HasConstraintName("SYS_C0010188");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("SYS_C0010189");
            });

            modelBuilder.Entity<Answerchecking>(entity =>
            {
                entity.HasKey(e => e.AnswerId)
                    .HasName("ANSWERCHECKING_PK");

                entity.ToTable("ANSWERCHECKING");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REVIEW_DATE")
                    .HasDefaultValueSql("null");

                entity.Property(e => e.ReviewReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_REASON");

                entity.Property(e => e.ReviewResult)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_RESULT");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Answercheckings)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010191");

                entity.HasOne(d => d.Answer)
                    .WithOne(p => p.Answerchecking)
                    .HasForeignKey<Answerchecking>(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010190");
            });

            modelBuilder.Entity<Answercomment>(entity =>
            {
                entity.ToTable("ANSWERCOMMENT");

                entity.Property(e => e.AnswerCommentId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerCommentContent)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("ANSWER_COMMENT_CONTENT")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.AnswerCommentFather)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_FATHER")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.AnswerCommentLike)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ANSWER_COMMENT_LIKE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerCommentReply)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_REPLY")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.AnswerCommentTime)
                    .HasColumnType("DATE")
                    .HasColumnName("ANSWER_COMMENT_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.AnswerCommentUserId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerCommentVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("ANSWER_COMMENT_VISIBLE")
                    .HasDefaultValueSql("0\n   ");

                entity.HasOne(d => d.AnswerCommentFatherNavigation)
                    .WithMany(p => p.Answercomments)
                    .HasForeignKey(d => d.AnswerCommentFather)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C0010194");

                entity.HasOne(d => d.AnswerCommentReplyNavigation)
                    .WithMany(p => p.InverseAnswerCommentReplyNavigation)
                    .HasForeignKey(d => d.AnswerCommentReply)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C0010193");

                entity.HasOne(d => d.AnswerCommentUser)
                    .WithMany(p => p.Answercomments)
                    .HasForeignKey(d => d.AnswerCommentUserId)
                    .HasConstraintName("SYS_C0010192");
            });

            modelBuilder.Entity<Answercommentreport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("ANSWERCOMMENTREPORT_PK");

                entity.ToTable("ANSWERCOMMENTREPORT");

                entity.Property(e => e.ReportId)
                    .HasPrecision(10)
                    .HasColumnName("REPORT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerCommentId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReportAnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportAnswerResult)
                    .HasPrecision(1)
                    .HasColumnName("REPORT_ANSWER_RESULT");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REPORT_REASON");

                entity.Property(e => e.ReportState)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("REPORT_STATE")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Answercommentreports)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010197");

                entity.HasOne(d => d.AnswerComment)
                    .WithMany(p => p.Answercommentreports)
                    .HasForeignKey(d => d.AnswerCommentId)
                    .HasConstraintName("SYS_C0010195");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Answercommentreports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010196");
            });

            modelBuilder.Entity<Answerreport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("ANSWERREPORT_PK");

                entity.ToTable("ANSWERREPORT");

                entity.Property(e => e.ReportId)
                    .HasPrecision(10)
                    .HasColumnName("REPORT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReportAnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportAnswerResult)
                    .HasPrecision(1)
                    .HasColumnName("REPORT_ANSWER_RESULT");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REPORT_REASON");

                entity.Property(e => e.ReportState)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("REPORT_STATE")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Answerreports)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010200");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Answerreports)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010198");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Answerreports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010199");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("BLOG");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("BLOG_ID");

                entity.Property(e => e.BlogCoin)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("BLOG_COIN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogContent)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BLOG_CONTENT");

                entity.Property(e => e.BlogDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOG_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.BlogImage)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BLOG_IMAGE");

                entity.Property(e => e.BlogLike)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("BLOG_LIKE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogSummary)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BLOG_SUMMARY");

                entity.Property(e => e.BlogTag)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BLOG_TAG")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.BlogUserId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_USER_ID")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.BlogVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("BLOG_VISIBLE")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.BlogUser)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.BlogUserId)
                    .HasConstraintName("SYS_C0010201");
            });

            modelBuilder.Entity<Blogchecking>(entity =>
            {
                entity.HasKey(e => e.BlogId)
                    .HasName("BLOGCHECKING_PK");

                entity.ToTable("BLOGCHECKING");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOG_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REVIEW_DATE")
                    .HasDefaultValueSql("null");

                entity.Property(e => e.ReviewReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_REASON");

                entity.Property(e => e.ReviewResult)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_RESULT");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Blogcheckings)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010203");

                entity.HasOne(d => d.Blog)
                    .WithOne(p => p.Blogchecking)
                    .HasForeignKey<Blogchecking>(d => d.BlogId)
                    .HasConstraintName("SYS_C0010202");
            });

            modelBuilder.Entity<Blogcomment>(entity =>
            {
                entity.ToTable("BLOGCOMMENT");

                entity.Property(e => e.BlogCommentId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogCommentContent)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BLOG_COMMENT_CONTENT")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.BlogCommentFather)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_FATHER")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.BlogCommentLike)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("BLOG_COMMENT_LIKE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogCommentReply)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_REPLY")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.BlogCommentTime)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOG_COMMENT_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.BlogCommentUserId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogCommentVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("BLOG_COMMENT_VISIBLE")
                    .HasDefaultValueSql("0\n   ");

                entity.HasOne(d => d.BlogCommentFatherNavigation)
                    .WithMany(p => p.Blogcomments)
                    .HasForeignKey(d => d.BlogCommentFather)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C0010206");

                entity.HasOne(d => d.BlogCommentReplyNavigation)
                    .WithMany(p => p.InverseBlogCommentReplyNavigation)
                    .HasForeignKey(d => d.BlogCommentReply)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C0010205");

                entity.HasOne(d => d.BlogCommentUser)
                    .WithMany(p => p.Blogcomments)
                    .HasForeignKey(d => d.BlogCommentUserId)
                    .HasConstraintName("SYS_C0010204");
            });

            modelBuilder.Entity<Blogcommentreport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("BLOGCOMMENTREPORT_PK");

                entity.ToTable("BLOGCOMMENTREPORT");

                entity.Property(e => e.ReportId)
                    .HasPrecision(10)
                    .HasColumnName("REPORT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogCommentId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReportAnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportAnswerResult)
                    .HasPrecision(1)
                    .HasColumnName("REPORT_ANSWER_RESULT");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REPORT_REASON");

                entity.Property(e => e.ReportState)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("REPORT_STATE")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Blogcommentreports)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010209");

                entity.HasOne(d => d.BlogComment)
                    .WithMany(p => p.Blogcommentreports)
                    .HasForeignKey(d => d.BlogCommentId)
                    .HasConstraintName("SYS_C0010207");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogcommentreports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010208");
            });

            modelBuilder.Entity<Blogreport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("BLOGREPORT_PK");

                entity.ToTable("BLOGREPORT");

                entity.Property(e => e.ReportId)
                    .HasPrecision(10)
                    .HasColumnName("REPORT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReportAnswerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_ANSWER_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportAnswerResult)
                    .HasPrecision(1)
                    .HasColumnName("REPORT_ANSWER_RESULT");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REPORT_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REPORT_REASON");

                entity.Property(e => e.ReportState)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("REPORT_STATE")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Blogreports)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010212");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Blogreports)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("SYS_C0010210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogreports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010211");
            });

            modelBuilder.Entity<Coinanswer>(entity =>
            {
                entity.HasKey(e => new { e.AnswerId, e.UserId })
                    .HasName("COINANSWER_PK");

                entity.ToTable("COINANSWER");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CoinNum)
                    .HasPrecision(10)
                    .HasColumnName("COIN_NUM")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.CoinTime)
                    .HasColumnType("DATE")
                    .HasColumnName("COIN_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Coinanswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010213");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Coinanswers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010214");
            });

            modelBuilder.Entity<Coinblog>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.UserId })
                    .HasName("COINBLOG_PK");

                entity.ToTable("COINBLOG");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CoinNum)
                    .HasPrecision(10)
                    .HasColumnName("COIN_NUM")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.CoinTime)
                    .HasColumnType("DATE")
                    .HasColumnName("COIN_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Coinblogs)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("SYS_C0010215");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Coinblogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010216");
            });

            modelBuilder.Entity<Followinstitution>(entity =>
            {
                entity.HasKey(e => new { e.InstitutionId, e.UserId })
                    .HasName("FOLLOWINSTITUTION_PK");

                entity.ToTable("FOLLOWINSTITUTION");

                entity.Property(e => e.InstitutionId)
                    .HasPrecision(10)
                    .HasColumnName("INSTITUTION_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.FollowTime)
                    .HasColumnType("DATE")
                    .HasColumnName("FOLLOW_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Followinstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("SYS_C0010217");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Followinstitutions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010218");
            });

            modelBuilder.Entity<Followuniversity>(entity =>
            {
                entity.HasKey(e => new { e.UniversityId, e.UserId })
                    .HasName("FOLLOWUNIVERSITY_PK");

                entity.ToTable("FOLLOWUNIVERSITY");

                entity.Property(e => e.UniversityId)
                    .HasPrecision(10)
                    .HasColumnName("UNIVERSITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.FollowTime)
                    .HasColumnType("DATE")
                    .HasColumnName("FOLLOW_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Followuniversities)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("SYS_C0010219");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Followuniversities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010220");
            });

            modelBuilder.Entity<Followuser>(entity =>
            {
                entity.HasKey(e => new { e.FollowUserId, e.UserId })
                    .HasName("FOLLOWUSER_PK");

                entity.ToTable("FOLLOWUSER");

                entity.Property(e => e.FollowUserId)
                    .HasPrecision(10)
                    .HasColumnName("FOLLOW_USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.FollowTime)
                    .HasColumnType("DATE")
                    .HasColumnName("FOLLOW_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.FollowUser)
                    .WithMany(p => p.FollowuserFollowUsers)
                    .HasForeignKey(d => d.FollowUserId)
                    .HasConstraintName("SYS_C0010221");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FollowuserUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010222");
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.ToTable("INSTITUTION");

                entity.HasIndex(e => e.InstitutionName, "SYS_C0010178")
                    .IsUnique();

                entity.Property(e => e.InstitutionId)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("INSTITUTION_ID");

                entity.Property(e => e.InstitutionCity)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_CITY");

                entity.Property(e => e.InstitutionCreatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("INSTITUTION_CREATETIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.InstitutionEmail)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_EMAIL")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionIntroduction)
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_INTRODUCTION");

                entity.Property(e => e.InstitutionLessons)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_LESSONS")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionLessonsCharacter)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_LESSONS_CHARACTER")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionLocation)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_LOCATION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_NAME")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionPhone)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_PHONE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.InstitutionPhoto)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_PHOTO");

                entity.Property(e => e.InstitutionProfile)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_PROFILE");

                entity.Property(e => e.InstitutionProvince)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_PROVINCE");

                entity.Property(e => e.InstitutionQualify)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_QUALIFY");

                entity.Property(e => e.InstitutionTarget)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_TARGET");
            });

            modelBuilder.Entity<Likeanswer>(entity =>
            {
                entity.HasKey(e => new { e.AnswerId, e.UserId })
                    .HasName("LIKEANSWER_PK");

                entity.ToTable("LIKEANSWER");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.LikeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIKE_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Likeanswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010223");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likeanswers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010224");
            });

            modelBuilder.Entity<Likeanswercomment>(entity =>
            {
                entity.HasKey(e => new { e.AnswerCommentId, e.UserId })
                    .HasName("LIKEANSWERCOMMENT_PK");

                entity.ToTable("LIKEANSWERCOMMENT");

                entity.Property(e => e.AnswerCommentId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.LikeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIKE_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.AnswerComment)
                    .WithMany(p => p.Likeanswercomments)
                    .HasForeignKey(d => d.AnswerCommentId)
                    .HasConstraintName("SYS_C0010225");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likeanswercomments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010226");
            });

            modelBuilder.Entity<Likeblog>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.UserId })
                    .HasName("LIKEBLOG_PK");

                entity.ToTable("LIKEBLOG");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.LikeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIKE_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Likeblogs)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("SYS_C0010227");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likeblogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010228");
            });

            modelBuilder.Entity<Likeblogcomment>(entity =>
            {
                entity.HasKey(e => new { e.BlogCommentId, e.UserId })
                    .HasName("LIKEBLOGCOMMENT_PK");

                entity.ToTable("LIKEBLOGCOMMENT");

                entity.Property(e => e.BlogCommentId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_COMMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.LikeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIKE_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.BlogComment)
                    .WithMany(p => p.Likeblogcomments)
                    .HasForeignKey(d => d.BlogCommentId)
                    .HasConstraintName("SYS_C0010229");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likeblogcomments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010230");
            });

            modelBuilder.Entity<Moneychangerecord>(entity =>
            {
                entity.HasKey(e => new { e.RecordId, e.UserId })
                    .HasName("MONEYCHANGERECORD_PK");

                entity.ToTable("MONEYCHANGERECORD");

                entity.Property(e => e.RecordId)
                    .HasPrecision(10)
                    .HasColumnName("RECORD_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CHANGE_DATE");

                entity.Property(e => e.ChangeNum)
                    .HasPrecision(10)
                    .HasColumnName("CHANGE_NUM")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ChangeReason)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CHANGE_REASON");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Moneychangerecords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010231");
            });

            modelBuilder.Entity<Newsflash>(entity =>
            {
                entity.ToTable("NEWSFLASH");

                entity.Property(e => e.NewsFlashId)
                    .HasPrecision(10)
                    .HasColumnName("NEWS_FLASH_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NewsFlashContent)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_CONTENT")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.NewsFlashDate)
                    .HasColumnType("DATE")
                    .HasColumnName("NEWS_FLASH_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.NewsFlashImage)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_IMAGE");

                entity.Property(e => e.NewsFlashRegion)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_REGION");

                entity.Property(e => e.NewsFlashSummary)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_SUMMARY");

                entity.Property(e => e.NewsFlashTag)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_TAG");

                entity.Property(e => e.NewsFlashTitle)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("NEWS_FLASH_TITLE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.NewsFlashVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("NEWS_FLASH_VISIBLE")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.IdentityId)
                    .HasName("SYS_C0010169");

                entity.ToTable("QUALIFICATION");

                entity.Property(e => e.IdentityId)
                    .HasPrecision(10)
                    .HasColumnName("IDENTITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EnrollmentTime)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("ENROLLMENT_TIME");

                entity.Property(e => e.Identity)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("IDENTITY");

                entity.Property(e => e.IdentityQualificationImage)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("IDENTITY_QUALIFICATION_IMAGE");

                entity.Property(e => e.Major)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("MAJOR");

                entity.Property(e => e.UniversityId)
                    .HasPrecision(10)
                    .HasColumnName("UNIVERSITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("VISIBLE")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Qualifications)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("SYS_C0010233");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Qualifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010232");
            });

            modelBuilder.Entity<Qualificationchecking>(entity =>
            {
                entity.HasKey(e => e.IdentityId)
                    .HasName("SYS_C0010081");

                entity.ToTable("QUALIFICATIONCHECKING");

                entity.Property(e => e.IdentityId)
                    .HasPrecision(10)
                    .HasColumnName("IDENTITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REVIEW_DATE")
                    .HasDefaultValueSql("null");

                entity.Property(e => e.ReviewReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_REASON");

                entity.Property(e => e.ReviewResult)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_RESULT");

                entity.Property(e => e.SummitDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SUMMIT_DATE")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Qualificationcheckings)
                    .HasForeignKey(d => d.AdministratorId)
                    .HasConstraintName("SYS_C0010235");

                entity.HasOne(d => d.Identity)
                    .WithOne(p => p.Qualificationchecking)
                    .HasForeignKey<Qualificationchecking>(d => d.IdentityId)
                    .HasConstraintName("SYS_C0010234");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("QUESTION");

                entity.Property(e => e.QuestionId)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionApply)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_APPLY")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("QUESTION_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.QuestionDescription)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("QUESTION_DESCRIPTION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.QuestionImage)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("QUESTION_IMAGE");

                entity.Property(e => e.QuestionReward)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("QUESTION_REWARD")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionSummary)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("QUESTION_SUMMARY");

                entity.Property(e => e.QuestionTag)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("QUESTION_TAG")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.QuestionTitle)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("QUESTION_TITLE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.QuestionUserId)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionVisible)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("QUESTION_VISIBLE")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.QuestionUser)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionUserId)
                    .HasConstraintName("SYS_C0010236");
            });

            modelBuilder.Entity<Questionchecking>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("QUESTIONCHECKING_PK");

                entity.ToTable("QUESTIONCHECKING");

                entity.Property(e => e.QuestionId)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AdministratorId)
                    .HasPrecision(10)
                    .HasColumnName("ADMINISTRATOR_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.QuestionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("QUESTION_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REVIEW_DATE");

                entity.Property(e => e.ReviewReason)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_REASON");

                entity.Property(e => e.ReviewResult)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW_RESULT");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Questioncheckings)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0010238");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.Questionchecking)
                    .HasForeignKey<Questionchecking>(d => d.QuestionId)
                    .HasConstraintName("SYS_C0010237");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.HasKey(e => new { e.UniversityId, e.RankYear })
                    .HasName("RANK_PK");

                entity.ToTable("RANK");

                entity.Property(e => e.UniversityId)
                    .HasPrecision(10)
                    .HasColumnName("UNIVERSITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RankYear)
                    .HasPrecision(5)
                    .HasColumnName("RANK_YEAR")
                    .HasDefaultValueSql("1970");

                entity.Property(e => e.UniversityQsRank)
                    .HasPrecision(5)
                    .HasColumnName("UNIVERSITY_QS_RANK");

                entity.Property(e => e.UniversityTheRank)
                    .HasPrecision(5)
                    .HasColumnName("UNIVERSITY_THE_RANK");

                entity.Property(e => e.UniversityUsnewsRank)
                    .HasPrecision(5)
                    .HasColumnName("UNIVERSITY_USNEWS_RANK");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Ranks)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("SYS_C0010239");
            });

            modelBuilder.Entity<Staranswer>(entity =>
            {
                entity.HasKey(e => new { e.AnswerId, e.UserId })
                    .HasName("STARANSWER_PK");

                entity.ToTable("STARANSWER");

                entity.Property(e => e.AnswerId)
                    .HasPrecision(10)
                    .HasColumnName("ANSWER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.StarTime)
                    .HasColumnType("DATE")
                    .HasColumnName("STAR_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Staranswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010240");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Staranswers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010241");
            });

            modelBuilder.Entity<Starblog>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.UserId })
                    .HasName("STARBLOG_PK");

                entity.ToTable("STARBLOG");

                entity.Property(e => e.BlogId)
                    .HasPrecision(10)
                    .HasColumnName("BLOG_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.StarTime)
                    .HasColumnType("DATE")
                    .HasColumnName("STAR_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Starblogs)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("SYS_C0010242");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Starblogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010243");
            });

            modelBuilder.Entity<Starquestion>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.UserId })
                    .HasName("STARQUESTION_PK");

                entity.ToTable("STARQUESTION");

                entity.Property(e => e.QuestionId)
                    .HasPrecision(10)
                    .HasColumnName("QUESTION_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Cancel)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("CANCEL")
                    .HasDefaultValueSql("0\n   ");

                entity.Property(e => e.StarTime)
                    .HasColumnType("DATE")
                    .HasColumnName("STAR_TIME")
                    .HasDefaultValueSql("SYSDATE");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Starquestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("SYS_C0010244");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Starquestions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010245");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("UNIVERSITY");

                entity.HasIndex(e => e.UniversityEnName, "SYS_C0010058")
                    .IsUnique();

                entity.Property(e => e.UniversityId)
                    .HasPrecision(10)
                    .HasColumnName("UNIVERSITY_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityAbbreviation)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_ABBREVIATION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityAddressX)
                    .HasColumnType("NUMBER(10,7)")
                    .HasColumnName("UNIVERSITY_ADDRESS_X")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityAddressY)
                    .HasColumnType("NUMBER(10,7)")
                    .HasColumnName("UNIVERSITY_ADDRESS_Y")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityBadge)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_BADGE");

                entity.Property(e => e.UniversityChName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_CH_NAME")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityCollege)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_COLLEGE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityCountry)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_COUNTRY")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityEmail)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_EMAIL")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityEnName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_EN_NAME")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityIeltsRequirement)
                    .HasColumnType("NUMBER(2,1)")
                    .HasColumnName("UNIVERSITY_IELTS_REQUIREMENT")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityIntroduction)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_INTRODUCTION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityLocation)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_LOCATION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityPhoto)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_PHOTO")
                    .HasDefaultValueSql("'SB'\n   ");

                entity.Property(e => e.UniversityRegion)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_REGION")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UniversityStudentNum)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("UNIVERSITY_STUDENT_NUM")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityTeacherNum)
                    .HasPrecision(5)
                    .HasColumnName("UNIVERSITY_TEACHER_NUM")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityTofelRequirement)
                    .HasPrecision(3)
                    .HasColumnName("UNIVERSITY_TOFEL_REQUIREMENT")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UniversityTuition)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_TUITION");

                entity.Property(e => e.UniversityWebsite)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UNIVERSITY_WEBSITE")
                    .HasDefaultValueSql("'none'");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");

                entity.HasIndex(e => e.UserPhone, "SYS_C0010074")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("USER_ID");

                entity.Property(e => e.ContinusLogin)
                    .HasPrecision(10)
                    .HasColumnName("CONTINUS_LOGIN")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.UserBirthday)
                    .HasColumnType("DATE")
                    .HasColumnName("USER_BIRTHDAY");

                entity.Property(e => e.UserCoin)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("USER_COIN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserCreatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("USER_CREATETIME");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("USER_EMAIL");

                entity.Property(e => e.UserExp)
                    .HasPrecision(10)
                    .HasColumnName("USER_EXP")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.UserFollower)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("USER_FOLLOWER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserFollows)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("USER_FOLLOWS")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserGender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("USER_GENDER");

                entity.Property(e => e.UserLevel)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("USER_LEVEL")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UserLogintime)
                    .HasColumnType("DATE")
                    .HasColumnName("USER_LOGINTIME")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USER_PASSWORD");

                entity.Property(e => e.UserPhone)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("USER_PHONE");

                entity.Property(e => e.UserProfile)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("USER_PROFILE")
                    .HasDefaultValueSql("'https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/user_profile/default.png'");

                entity.Property(e => e.UserSignature)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("USER_SIGNATURE")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.UserState)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("USER_STATE")
                    .HasDefaultValueSql("1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
