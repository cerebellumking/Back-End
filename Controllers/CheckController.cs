using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        private readonly ModelContext myContext;
        public CheckController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet("all_questions")]
        public string getAllQuestionsToCheck(int admin_id)
        {
            Message message = new();
            try
            {
                var question = myContext.Questioncheckings
                    .Where(b => (b.AdministratorId == 0 || b.AdministratorId == admin_id)) // 要么是未审核，要么该管理员审核过
                    .OrderByDescending(b => b.QuestionDate)
                    .Select(b => new
                    {
                        b.QuestionId,
                        b.Question.QuestionTitle,
                        b.Question.QuestionUserId,
                        b.Question.QuestionUser.UserName,
                        b.Question.QuestionUser.UserProfile,
                        b.Question.QuestionSummary,
                        b.AdministratorId,
                        b.QuestionDate,
                        b.ReviewResult,
                        b.ReviewDate,
                        b.ReviewReason,
                    }).ToList();
                // 如果未审核，ReviewResult是“待审核”，ReviewDate, ReviewReason都是null
                // 如果审核通过，则ReviewResult是“通过”，否则“不通过”
                message.errorCode = 200;
                message.status = true;
                message.data.Add("question_list", question.ToArray());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("all_blogs")]
        public string getAllBlogsToCheck(int admin_id)
        {
            Message message = new();
            try
            {
                var blog = myContext.Blogcheckings
                    .Where(b => b.AdministratorId == 0 || b.AdministratorId == admin_id)
                    .OrderByDescending(b => b.BlogDate)
                    .Select(b => new
                    {
                        b.BlogId,
                        b.Blog.BlogUserId,
                        //增加动态发布者的用户名和头像以及动态标题
                        b.Blog.BlogUser.UserName,
                        b.Blog.BlogUser.UserProfile,
                        b.Blog.BlogSummary,

                        b.AdministratorId,
                        b.BlogDate,
                        b.ReviewResult,
                        b.ReviewDate,
                        b.ReviewReason,
                    }).ToList();
                // 如果未审核，ReviewResult是“待审核”，ReviewDate, ReviewReason都是null
                // 如果审核通过，则ReviewResult是“通过”，否则“不通过”
                message.errorCode = 200;
                message.status = true;
                message.data.Add("blog_list", blog.ToArray());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("all_answers")]
        public string getAllAnswersToCheck(int admin_id)
        {
            Message message = new();
            try
            {
                var answer = myContext.Answercheckings
                    .Where(b => (b.AdministratorId == 0 || b.AdministratorId == admin_id))
                    .OrderByDescending(b => b.AnswerDate)
                    .Select(b => new
                    {
                        b.AnswerId,
                        b.Answer.AnswerUserId,
                        //添加管理员id，回答标题，答主用户名和头像
                        b.AdministratorId,
                        b.Answer.Question.QuestionTitle,
                        b.Answer.AnswerSummary,
                        b.Answer.AnswerUser.UserName,
                        b.Answer.AnswerUser.UserProfile,

                        b.AnswerDate,
                        b.ReviewResult,
                        b.ReviewDate,
                        b.ReviewReason,
                    }).ToList();
                // 如果未审核，ReviewResult是“待审核”，ReviewDate, ReviewReason都是null
                // 如果审核通过，则ReviewResult是“通过”，否则“不通过”
                message.errorCode = 200;
                message.status = true;
                message.data.Add("answer_list", answer.ToArray());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("all_qualifications")]
        public string getAllQualificationToCheck(int admin_id)
        {
            Message message = new();
            try
            {
                var qualification = myContext.Qualificationcheckings
                    .Where(b => b.AdministratorId == 0 || b.AdministratorId == admin_id)
                    .OrderByDescending(b => b.SummitDate)
                    .Select(b => new
                    {
                        b.IdentityId,
                        b.Identity.UserId,
                        b.Identity.User.UserName,
                        b.Identity.User.UserProfile,
                        b.Identity.UniversityId,
                        b.Identity.University.UniversityChName,
                        b.Identity.Identity,
                        b.Identity.IdentityQualificationImage,
                        b.Identity.Major,
                        b.Identity.EnrollmentTime,
                        b.AdministratorId,
                        b.ReviewResult,
                        b.SummitDate,
                        b.ReviewDate,
                        b.ReviewReason,
                    }).ToList();
                message.errorCode = 200;
                message.status = true;
                message.data.Add("qualification_list", qualification.ToArray());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("single_question")]
        public string getSingleQuestionToCheck(int question_id)
        {
            Message message = new();
            try
            {
                Questionchecking question_checking = myContext.Questioncheckings.Single(b => b.QuestionId == question_id);
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                message.data.Add("QuestionId", question_checking.QuestionId);
                message.data.Add("QuestionTitle", question.QuestionTitle);
                message.data.Add("QuestionContent", question.QuestionDescription);
                message.data.Add("QuestionUserId", question.QuestionUserId);
                var user = myContext.Users.Where(b => b.UserId == question.QuestionUserId)
                    .Select(b => new
                    {
                        b.UserName,
                        b.UserProfile,
                    }).First();
                message.data.Add("QuestionUserName", user.UserName);
                message.data.Add("QuestionUserProfile", user.UserProfile);
                message.data.Add("QuestionTag", question.QuestionTag);
                message.data.Add("AdministratorId", question_checking.AdministratorId);
                message.data.Add("QuestionDate", question_checking.QuestionDate);
                message.data.Add("ReviewResult", question_checking.ReviewResult);
                message.data.Add("ReviewDate", question_checking.ReviewDate);
                message.data.Add("ReviewReason", question_checking.ReviewReason);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("single_blog")]
        public string getSingleBlogToCheck(int blog_id)
        {
            Message message = new();
            try
            {
                Blogchecking blog_checking = myContext.Blogcheckings.Single(b => b.BlogId == blog_id);
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                message.data.Add("BlogId", blog_checking.BlogId);
                message.data.Add("BlogUserId", blog.BlogUserId);
                message.data.Add("BlogTag", blog.BlogTag);
                message.data.Add("BlogContent", blog.BlogContent);
                var user = myContext.Users.Where(b => b.UserId == blog.BlogUserId)
                    .Select(b => new
                    {
                        b.UserName,
                        b.UserProfile,
                    }).First();
                message.data.Add("BlogUserName", user.UserName);
                message.data.Add("BlogUserProfile", user.UserProfile);
                message.data.Add("AdministratorId", blog_checking.AdministratorId);
                message.data.Add("BlogDate", blog_checking.BlogDate);
                message.data.Add("ReviewResult", blog_checking.ReviewResult);
                message.data.Add("ReviewDate", blog_checking.ReviewDate);
                message.data.Add("ReviewReason", blog_checking.ReviewReason);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("single_answer")]
        public string getSingleAnswerToCheck(int answer_id)
        {
            Message message = new();
            try
            {
                Answerchecking answer_checking = myContext.Answercheckings.Single(b => b.AnswerId == answer_id);
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                message.data.Add("AnswerId", answer_checking.AnswerId);
                message.data.Add("AnswerContent", answer.AnswerContent);
                message.data.Add("AnswerUserId", answer.AnswerUserId);
                var user = myContext.Users.Where(b => b.UserId == answer.AnswerUserId)
                    .Select(b => new
                    {
                        b.UserName,
                        b.UserProfile,
                    }).First();
                message.data.Add("AnswerUserName", user.UserName);
                message.data.Add("AnswerUserProfile", user.UserProfile);
                var question = myContext.Questions.Where(b => b.QuestionId == answer.QuestionId)
                    .Select(b => new
                    {
                        b.QuestionTitle,
                    }).First();
                message.data.Add("QuestionTitle", question.QuestionTitle);
                message.data.Add("AnswerSummary", answer.AnswerSummary);
                message.data.Add("AdministratorId", answer_checking.AdministratorId);
                message.data.Add("AnswerDate", answer_checking.AnswerDate);
                message.data.Add("ReviewResult", answer_checking.ReviewResult);
                message.data.Add("ReviewDate", answer_checking.ReviewDate);
                message.data.Add("ReviewReason", answer_checking.ReviewReason);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("single_qualification")]
        public string getSingleQualificationToCheck(int identity_id)
        {
            Message message = new();
            try
            {
                //Qualificationchecking qualification_checking = myContext.Qualificationcheckings.Single(b => b.IdentityId == identity_id);
                var qualification_checking = myContext.Qualificationcheckings
                    .Where(b => b.IdentityId == identity_id)
                    .Select(b => new
                    {
                        b.IdentityId,
                        b.AdministratorId,
                        b.SummitDate,
                        b.ReviewResult,
                        b.ReviewDate,
                        b.ReviewReason,
                    }).First();
                //Qualification qualification = myContext.Qualifications.Single(b => b.IdentityId == identity_id);
                var qualification = myContext.Qualifications
                    .Where(b => b.IdentityId == identity_id)
                    .Select(b => new
                    {
                        b.User.UserName,
                        b.UniversityId,
                        b.Identity,
                        b.IdentityQualificationImage,
                        b.EnrollmentTime,
                        b.Major,
                    }).First();
                message.data.Add("IdentityId", qualification_checking.IdentityId);
                message.data.Add("UniversityId", qualification.UniversityId);
                var university = myContext.Universities.Where(b => b.UniversityId == qualification.UniversityId)
                    .Select(b => new
                    {
                        b.UniversityChName,
                        b.UniversityEnName,
                    }).First();
                message.data.Add("UniversityChName", university.UniversityChName);
                message.data.Add("UniversityEnName", university.UniversityEnName);
                message.data.Add("UserName", qualification.UserName);
                message.data.Add("Identity", qualification.Identity);
                message.data.Add("IdentityImage", qualification.IdentityQualificationImage);
                message.data.Add("EnrollmentTime", qualification.EnrollmentTime);
                message.data.Add("Major", qualification.Major);
                message.data.Add("AdministratorId", qualification_checking.AdministratorId);
                message.data.Add("SummitDate", qualification_checking.SummitDate);
                message.data.Add("ReviewResult", qualification_checking.ReviewResult);
                message.data.Add("ReviewDate", qualification_checking.ReviewDate);
                message.data.Add("ReviewReason", qualification_checking.ReviewReason);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("submit_question")]
        public string submitQuestionCheck(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                int question_id = int.Parse(front_end_data.GetProperty("question_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool review_result = bool.Parse(front_end_data.GetProperty("review_result").ToString()); // 只能是通过/不通过
                string review_reason = front_end_data.GetProperty("review_reason").ToString();

                myContext.DetachAll();
                Questionchecking question_checking = myContext.Questioncheckings.Single(b => b.QuestionId == question_id);
                question_checking.AdministratorId = administrator_id;
                question_checking.ReviewDate = DateTime.Now;
                question_checking.ReviewResult = review_result ? "通过" : "不通过";
                question_checking.ReviewReason = review_reason;
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                question.QuestionVisible = review_result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("submit_blog")]
        public string submitBlogCheck(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool review_result = bool.Parse(front_end_data.GetProperty("review_result").ToString()); // 只能是通过/不通过
                string review_reason = front_end_data.GetProperty("review_reason").ToString();
                myContext.DetachAll();
                Blogchecking blogchecking = myContext.Blogcheckings.Single(b => b.BlogId == blog_id);
                blogchecking.AdministratorId = administrator_id;
                blogchecking.ReviewDate = DateTime.Now;
                blogchecking.ReviewResult = review_result ? "通过" : "不通过";
                blogchecking.ReviewReason = review_reason;
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                blog.BlogVisible = review_result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("submit_answer")]
        public string submitAnswerCheck(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool review_result = bool.Parse(front_end_data.GetProperty("review_result").ToString()); // 只能是通过/不通过
                string review_reason = front_end_data.GetProperty("review_reason").ToString();
                myContext.DetachAll();
                Answerchecking answerchecking = myContext.Answercheckings.Single(b => b.AnswerId == answer_id);
                answerchecking.AdministratorId = administrator_id;
                answerchecking.ReviewDate = DateTime.Now;
                answerchecking.ReviewResult = review_result ? "通过" : "不通过";
                answerchecking.ReviewReason = review_reason;
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                answer.AnswerVisible = review_result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("submit_qualification")]
        public string submitQualificationCheck(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                int identity_id = int.Parse(front_end_data.GetProperty("identity_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool review_result = bool.Parse(front_end_data.GetProperty("review_result").ToString()); // 只能是通过/不通过
                string review_reason = front_end_data.GetProperty("review_reason").ToString();
                myContext.DetachAll();
                Qualificationchecking qualificationchecking = myContext.Qualificationcheckings.Single(b => b.IdentityId == identity_id);
                qualificationchecking.AdministratorId = administrator_id;
                qualificationchecking.ReviewDate = DateTime.Now;
                qualificationchecking.ReviewResult= review_result ? "通过" : "不通过";
                qualificationchecking.ReviewReason = review_reason;
                Qualification qualification = myContext.Qualifications.Single(b => b.IdentityId == identity_id);
                qualification.Visible = review_result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answer_report/unsolved")]
        public string getUnsolvedAnswerReport()
        {
            Message message = new();
            try
            {
                var answer_report_list = myContext.Answerreports
                    .Where(b => b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult,b.Answer.AnswerSummary,b.User.UserProfile,b.User.UserName })
                    .ToList();
                message.data["answer_report"] = answer_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answercomment_report/unsolved")]
        public string getUnsolvedAnswerCommentReport()
        {
            Message message = new();
            try
            {
                var answer_comment_report_list = myContext.Answercommentreports
                     .Where(b => b.ReportAnswerResult == null)
                     .Select(b => new { b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult, b.User.UserProfile, b.User.UserName,b.AnswerComment.AnswerCommentContent })
                     .ToList();
                message.data["answercomment_report"] = answer_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blog_report/unsolved")]
        public string getUnsolvedBlogReport()
        {
            Message message = new();
            try
            {
                var blog_report_list = myContext.Blogreports
                   .Where(b => b.ReportAnswerResult == null)
                   .Select(b => new { b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult, b.User.UserProfile, b.User.UserName,b.Blog.BlogSummary })
                   .ToList();
                message.data["blog_report"] = blog_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blogcomment_report/unsolved")]
        public string getUnsolvedBlogCommentReport()
        {
            Message message = new();
            try
            {
                var blog_comment_report_list = myContext.Blogcommentreports
                    .Where(b => b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult,b.BlogComment.BlogCommentContent, b.User.UserProfile, b.User.UserName })
                    .ToList();
                message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        //[HttpGet("unsolved")]
        //public string getUnsolvedReport(int administrator_id)
        //{
        //    Message message = new();
        //    try
        //    {
        //        var answer_report_list = myContext.Answerreports
        //            .Where(b=>b.ReportAnswerResult==null)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var blog_report_list = myContext.Blogreports
        //            .Where(b=>b.ReportAnswerResult==null)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var answer_comment_report_list = myContext.Answercommentreports
        //            .Where(b=>b.ReportAnswerResult==null)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var blog_comment_report_list = myContext.Blogcommentreports
        //            .Where(b=>b.ReportAnswerResult==null)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        message.data["answer_report"] = answer_report_list.ToArray();
        //        message.data["blog_report"] = blog_report_list.ToArray();
        //        message.data["answercomment_report"] = answer_comment_report_list.ToArray();
        //        message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
        //        message.errorCode = 200;
        //        message.status = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return message.ReturnJson();
        //}

        [HttpGet("answer_report/solved")]
        public string getSolvedAnswerReport()
        {
            Message message = new();
            try
            {
                var answer_report_list = myContext.Answerreports
                    .Where(b => b.ReportAnswerResult != null)
                    .Select(b => new { b.AdministratorId, b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult, b.User.UserProfile, b.User.UserName,b.Answer.AnswerSummary })
                    .ToList();
                message.data["answer_report"] = answer_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answercomment_report/solved")]
        public string getSolvedAnswerCommentReport()
        {
            Message message = new();
            try
            {
                var answer_comment_report_list = myContext.Answercommentreports
                     .Where(b => b.ReportAnswerResult != null)
                     .Select(b => new { b.AdministratorId, b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult,b.AnswerComment.AnswerCommentContent, b.User.UserProfile, b.User.UserName })
                     .ToList();
                message.data["answercomment_report"] = answer_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blog_report/solved")]
        public string getSolvedBlogReport()
        {
            Message message = new();
            try
            {
                var blog_report_list = myContext.Blogreports
                   .Where(b => b.ReportAnswerResult != null)
                   .Select(b => new { b.AdministratorId, b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult, b.User.UserProfile, b.User.UserName,b.Blog.BlogSummary })
                   .ToList();
                message.data["blog_report"] = blog_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blogcomment_report/solved")]
        public string getSolvedBlogCommentReport()
        {
            Message message = new();
            try
            {
                var blog_comment_report_list = myContext.Blogcommentreports
                    .Where(b => b.ReportAnswerResult != null)
                    .Select(b => new { b.AdministratorId, b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult, b.User.UserProfile, b.User.UserName,b.BlogComment.BlogCommentContent })
                    .ToList();
                message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
        //[HttpGet("solved")]
        //public string getSolvedReport(int administrator_id)
        //{
        //    Message message = new();
        //    try
        //    {
        //        var answer_report_list = myContext.Answerreports
        //            .Where(b => b.ReportAnswerResult != null&&b.AdministratorId==administrator_id)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var blog_report_list = myContext.Blogreports
        //            .Where(b => b.ReportAnswerResult != null && b.AdministratorId == administrator_id)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var answer_comment_report_list = myContext.Answercommentreports
        //            .Where(b => b.ReportAnswerResult != null && b.AdministratorId == administrator_id)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        var blog_comment_report_list = myContext.Blogcommentreports
        //            .Where(b => b.ReportAnswerResult != null && b.AdministratorId == administrator_id)
        //            .Select(b => new { b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
        //            .ToList();
        //        message.data["answer_report"] = answer_report_list.ToArray();
        //        message.data["blog_report"] = blog_report_list.ToArray();
        //        message.data["answercomment_report"] = answer_comment_report_list.ToArray();
        //        message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
        //        message.errorCode = 200;
        //        message.status = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return message.ReturnJson();
        //}
        [HttpGet("answer")]
        public string getAnswerInfo(int report_id)
        {
            Message message = new();
            try{
                Answerreport answerreport = myContext.Answerreports.Single(b => b.ReportId == report_id);
                Answer answer = myContext.Answers.Single(b=>b.AnswerId==answerreport.AnswerId);
                User user = myContext.Users.Single(b => b.UserId == answerreport.UserId);
                User user2 = myContext.Users.Single(b => b.UserId == answer.AnswerUserId);
                Question question = myContext.Questions.Single(b => b.QuestionId == answer.QuestionId);
                message.data["UserName"] = user.UserName;
                message.data["UserProfile"] = user.UserProfile;
                message.data["ReportedUserName"] = user2.UserName;
                message.data["ReportedUserProfile"] = user2.UserProfile;
                message.data["Question"] =question.QuestionDescription;
                message.data["QuestionTitle"] = question.QuestionTitle;
                message.data["QuestionDate"] = question.QuestionDate;
                message.data["ReportedDate"] = answerreport.ReportDate;
                message.data["AnswerId"] = answer.AnswerId;
                message.data["AnswerContent"] = answer.AnswerContent;
                message.data["AnswerDate"] = answer.AnswerDate;
                message.data["ReportReason"] = answerreport.ReportReason;
                message.errorCode = 200;
                message.status = true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answercomment")]
        public string getAnswerCommentInfo(int report_id)
        {
            Message message = new();
            try
            {
                Answercommentreport answercommentreport = myContext.Answercommentreports.Single(b => b.ReportId == report_id);
                Answercomment answercomment =myContext.Answercomments.Single(b=>b.AnswerCommentId==answercommentreport.AnswerCommentId);
                User user = myContext.Users.Single(b => b.UserId == answercommentreport.UserId);
                User user2 = myContext.Users.Single(b => b.UserId == answercomment.AnswerCommentUserId);
                message.data["UserName"] = user.UserName;
                message.data["UserProfile"] = user.UserProfile;
                message.data["ReportedUserName"] = user2.UserName;
                message.data["ReportedUserProfile"] = user2.UserProfile;
                if (answercomment.AnswerCommentReply == null)
                {
                    Answer answer = myContext.Answers.Single(b => b.AnswerId == answercomment.AnswerCommentFather);
                    message.data["RepliedAnswerContent"] = answer.AnswerContent;
                    message.data["RepliedAnswerDate"] = answer.AnswerDate;
                    message.data["RepliedComment"] = null;
                    message.data["RepliedCommentDate"] = null;
                }
                else
                {
                    Answercomment answercomment1 = myContext.Answercomments.Single(b => b.AnswerCommentId == answercomment.AnswerCommentReply);
                    message.data["RepliedAnswerContent"] = null;
                    message.data["RepliedAnswerDate"] = null;
                    message.data["RepliedComment"] = answercomment1.AnswerCommentContent;
                    message.data["RepliedCommentDate"] = answercomment1.AnswerCommentTime;
                }
                message.data["ReportedDate"] = answercommentreport.ReportDate;
                message.data["AnswerCommentId"] = answercomment.AnswerCommentId;
                message.data["AnswerCommentContent"] = answercomment.AnswerCommentContent;
                message.data["AnswerCommentDate"] = answercomment.AnswerCommentTime;
                message.data["ReportReason"] = answercommentreport.ReportReason;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blog")]
        public string getBlogInfo(int report_id)
        {
            Message message = new();
            try
            {
                Blogreport blogreport = myContext.Blogreports.Single(b => b.ReportId == report_id);
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blogreport.BlogId);
                User user = myContext.Users.Single(b => b.UserId == blogreport.UserId);
                User user2 = myContext.Users.Single(b => b.UserId == blog.BlogUserId);
                message.data["UserName"] = user.UserName;
                message.data["UserProfile"] = user.UserProfile;
                message.data["ReportedUserName"] = user2.UserName;
                message.data["ReportedUserProfile"] = user2.UserProfile;
                message.data["ReportedDate"] = blogreport.ReportDate;
                message.data["BlogId"] = blog.BlogId;
                message.data["BlogContent"] = blog.BlogContent;
                message.data["BlogDate"] = blog.BlogDate;
                message.data["ReportReason"] = blogreport.ReportReason;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blogcomment")]
        public string getBlogCommentInfo(int report_id)
        {
            Message message = new();
            try
            {
                Blogcommentreport blogcommentreport = myContext.Blogcommentreports.Single(b => b.ReportId == report_id);
                Blogcomment blogcomment = myContext.Blogcomments.Single(b => b.BlogCommentId == blogcommentreport.BlogCommentId);
                User user = myContext.Users.Single(b => b.UserId == blogcommentreport.UserId);
                User user2 = myContext.Users.Single(b => b.UserId == blogcomment.BlogCommentUserId);
                message.data["UserName"] = user.UserName;
                message.data["UserProfile"] = user.UserProfile;
                message.data["ReportedUserName"] = user2.UserName;
                message.data["ReportedUserProfile"] = user2.UserProfile;
                if (blogcomment.BlogCommentReply == null)
                {
                    Blog blog = myContext.Blogs.Single(b => b.BlogId == blogcomment.BlogCommentFather);
                    message.data["RepliedBlogContent"] = blog.BlogContent;
                    message.data["RepliedBlogDate"] = blog.BlogDate;
                    message.data["RepliedComment"] = null;
                    message.data["RepliedCommentDate"] = null;
                }
                else
                {
                    Blogcomment blogcomment1 = myContext.Blogcomments.Single(b => b.BlogCommentId == blogcomment.BlogCommentReply);
                    message.data["RepliedBlogContent"] = null;
                    message.data["RepliedBlogDate"] = null;
                    message.data["RepliedComment"] = blogcomment1.BlogCommentContent;
                    message.data["RepliedCommentDate"] = blogcomment1.BlogCommentTime;
                }
                message.data["ReportedDate"] = blogcommentreport.ReportDate;
                message.data["AnswerCommentId"] = blogcomment.BlogCommentId;
                message.data["AnswerCommentContent"] = blogcomment.BlogCommentContent;
                message.data["AnswerCommentDate"] = blogcomment.BlogCommentTime;
                message.data["ReportReason"] = blogcommentreport.ReportReason;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("answer")]
        public string solveAnswerReport(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int report_id = int.Parse(front_end_data.GetProperty("report_id").ToString());
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool result = bool.Parse(front_end_data.GetProperty("result").ToString()); // 只能是通过/不通过
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                answer.AnswerVisible = !result;
                Answerreport answerreport = myContext.Answerreports.Single(b => b.ReportId == report_id);
                answerreport.ReportAnswerDate = DateTime.Now;
                answerreport.AdministratorId = administrator_id;
                answerreport.ReportAnswerResult = result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("answercomment")]
        public string solveAnswerCommentReport(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int report_id = int.Parse(front_end_data.GetProperty("report_id").ToString());
                int answercomment_id = int.Parse(front_end_data.GetProperty("answercomment_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool result = bool.Parse(front_end_data.GetProperty("result").ToString()); // 只能是通过/不通过
                Answercomment answercomment = myContext.Answercomments.Single(b => b.AnswerCommentId == answercomment_id);
                answercomment.AnswerCommentVisible = !result;
                Answercommentreport answercommentreport = myContext.Answercommentreports.Single(b => b.ReportId == report_id);
                answercommentreport.ReportAnswerDate = DateTime.Now;
                answercommentreport.AdministratorId = administrator_id;
                answercommentreport.ReportAnswerResult = result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("blog")]
        public string solveBlogReport(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int report_id = int.Parse(front_end_data.GetProperty("report_id").ToString());
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool result = bool.Parse(front_end_data.GetProperty("result").ToString()); // 只能是通过/不通过
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                blog.BlogVisible = !result;
                Blogreport blogreport = myContext.Blogreports.Single(b => b.ReportId == report_id);
                blogreport.ReportAnswerDate = DateTime.Now;
                blogreport.AdministratorId = administrator_id;
                blogreport.ReportAnswerResult = result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("blogcomment")]
        public string solveBlogCommentReport(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int report_id = int.Parse(front_end_data.GetProperty("report_id").ToString());
                int blogcomment_id = int.Parse(front_end_data.GetProperty("blogcomment_id").ToString());
                int administrator_id = int.Parse(front_end_data.GetProperty("administrator_id").ToString());
                bool result = bool.Parse(front_end_data.GetProperty("result").ToString()); // 只能是通过/不通过
                Blogcomment blogcomment = myContext.Blogcomments.Single(b => b.BlogCommentId == blogcomment_id);
                blogcomment.BlogCommentVisible = !result;
                Blogcommentreport blogcommentreport = myContext.Blogcommentreports.Single(b => b.ReportId == report_id);
                blogcommentreport.ReportAnswerDate = DateTime.Now;
                blogcommentreport.AdministratorId = administrator_id;
                blogcommentreport.ReportAnswerResult = result;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
