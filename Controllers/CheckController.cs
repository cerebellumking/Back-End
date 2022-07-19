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
                        b.Question.QuestionUserId,
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
                        b.Identity.UserId,
                        b.Identity.UniversityId,
                        b.Identity.Identity,
                        b.Identity.IdentityQualificationImage,
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
                message.data.Add("BlogTag", blog.BlogTag);
                message.data.Add("BlogContent", blog.BlogContent);
                message.data.Add("BlogUserId", blog.BlogUserId);
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
                Qualificationchecking qualification_checking = myContext.Qualificationcheckings.Single(b => b.IdentityId == identity_id);
                Qualification qualification = myContext.Qualifications.Single(b => b.IdentityId == identity_id);
                message.data.Add("IdentityId", qualification_checking.IdentityId);
                message.data.Add("UniversityId", qualification.UniversityId);
                message.data.Add("Identity", qualification.Identity);
                message.data.Add("IdentityImage", qualification.IdentityQualificationImage);
                message.data.Add("SummitDate", qualification_checking.SummitDate);
                message.data.Add("ReviewResult", qualification_checking.ReviewResult);
                message.data.Add("ReviewDate", qualification_checking.ReviewDate);
                message.data.Add("ReviewReason", qualification_checking.ReviewReason);
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
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
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
    }
}
