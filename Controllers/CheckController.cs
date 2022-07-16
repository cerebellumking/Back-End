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
                    .Select(b => new {
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
                question_checking.QuestionDate = DateTime.Now;
                question_checking.ReviewResult = review_result ? "通过" : "不通过";
                question_checking.ReviewReason = review_reason;
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

    }
}
