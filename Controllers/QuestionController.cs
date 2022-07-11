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
    public class QuestionController : ControllerBase
    {
        private readonly ModelContext myContext;
        public QuestionController(ModelContext modelContext)
        {
            myContext = modelContext;
        }
        [HttpGet]
        public string getQuestionInfo(int question_id)
        {
            Message message = new Message();
            try
            {
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                if (question.QuestionVisible==true)
                {
                    message.data.Add("question_id", question_id);
                    message.data.Add("question_user_id", question.QuestionUserId);
                    message.data.Add("question_tag", question.QuestionTag);
                    message.data.Add("question_date", question.QuestionDate);
                    message.data.Add("question_title", question.QuestionTitle);
                    message.data.Add("question_description", question.QuestionDescription);
                    message.data.Add("question_reward", question.QuestionReward);
                    message.data.Add("question_apply", question.QuestionApply);
                    message.data.Add("question_image", question.QuestionImage);
                }
                else
                {
                    message.data.Add("question_visible", question.QuestionVisible);
                }
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpGet("time")]
        public string showQuestionByTime()
        {
            Message message = new Message();
            try
            {
                var question = myContext.Questions.Where(c => c.QuestionVisible == true).OrderByDescending(a => a.QuestionDate).Select(b => new
                {
                    b.QuestionId,
                    b.QuestionUserId,
                    b.QuestionTitle,
                    b.QuestionApply,
                    b.QuestionReward,
                    b.QuestionDate,
                    b.QuestionDescription,
                    b.QuestionTag
                }).ToList();
                if (question.Count > 2)
                    question.RemoveRange(2, question.Count - 2);
                message.errorCode =200;
                message.status = true;
                message.data.Add("question", question.ToArray());
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("heat")]
        public string showQuestionByHeat()
        {
            Message message = new Message();
            try
            {
                var question = myContext.Questions.Where(c => c.QuestionVisible == true).OrderByDescending(a => a.Answers.Count()).Select(b => new
                {
                    b.Answers.Count,
                    b.QuestionId,
                    b.QuestionUserId,
                    b.QuestionTitle,
                    b.QuestionApply,
                    b.QuestionReward,
                    b.QuestionDate,
                    b.QuestionDescription,
                    b.QuestionTag
                }).ToList();
                if (question.Count > 2)
                    question.RemoveRange(2, question.Count - 2);
                message.errorCode = 200;
                message.status = true;
                message.data.Add("question", question.ToArray());
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPost]
        public string raiseQuestion(int question_user_id,string question_tag,string question_title,string question_description,decimal question_reward)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Question question = new Question();
                var count = myContext.Questions.Count();
                int id;
                if (count == 0)
                    id = 1;
                else
                {
                    id = myContext.Questions.Select(b => b.QuestionId).Max() + 1;
                }
                question.QuestionId = id;
                question.QuestionUserId = question_user_id;
                question.QuestionUser = myContext.Users.Single(b => b.UserId == question_user_id);
                question.QuestionTag = question_tag;
                question.QuestionTitle = question_title;
                question.QuestionDescription = question_description;
                question.QuestionReward = question_reward;
                question.QuestionApply = 0;
                question.QuestionVisible = true;
                question.QuestionDate = DateTime.Now;
                Questionchecking questionchecking = new Questionchecking();
                questionchecking.AdministratorId = 1;
                questionchecking.QuestionId = id;
                questionchecking.ReviewResult = "待审核";
                myContext.Questioncheckings.Add(questionchecking);
                myContext.Questions.Add(question);
                myContext.SaveChanges();
                message.data.Add("question_id", id);
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
