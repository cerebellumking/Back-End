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
    public class AnswerController : ControllerBase
    {
        private readonly ModelContext myContext;
        public AnswerController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getAnswerInfo(int answer_id)
        {
            Message message = new Message();
            try
            {
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                if (answer.AnswerVisible == true)
                {
                    Question question = myContext.Questions.Single(b => b.QuestionId == answer.QuestionId);
                    message.data.Add("answer_id", answer_id);
                    message.data.Add("answer_user_id", answer.AnswerUserId);
                    message.data.Add("question_id", answer.QuestionId);
                    message.data.Add("tag", question.QuestionTag);
                    message.data.Add("answer_date", answer.AnswerDate);
                    message.data.Add("answer_content", answer.AnswerContent);
                    message.data.Add("answer_contentpic", answer.AnswerContentpic);
                    message.data.Add("answer_coin", answer.AnswerCoin);
                    message.data.Add("answer_like", answer.AnswerLike);
                }
                else
                {
                    message.data.Add("answer_visible", answer.AnswerVisible);
                }
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
