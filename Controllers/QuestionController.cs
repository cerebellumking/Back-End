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
        public string showQuestionByTime()
        {
            Message message = new Message();

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
                myContext.Questions.Add(question);
                myContext.SaveChanges();
                message.data.Add("question_id", id);
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }
    }
}
