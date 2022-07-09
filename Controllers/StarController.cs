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
    public class StarController : ControllerBase
    {
        private readonly ModelContext myContext;
        public StarController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost("question")]
        public string statQuestion(int user_id,int question_id)
        {
            Message message = new Message();
            User user = new User();
            try
            {
                myContext.DetachAll();
                int max_id = myContext.Users.Select(b => b.UserId).Max();
                int max_question_id = myContext.Questions.Select(b => b.QuestionId).Max();
                if (!myContext.Starquestions.Any(b => b.QuestionId== question_id && b.UserId == user_id && b.Cancel == false)&& user_id <= max_id && question_id <= max_question_id)
                {
                    user = myContext.Users.Single(b => b.UserId == user_id);
                    /*判断该点赞是否取消过*/
                    if (!myContext.Starquestions.Any(b => b.QuestionId == question_id && b.UserId == user_id && b.Cancel == true))
                    {
                        Starquestion starquestion = new Starquestion();
                        starquestion.UserId = user_id;
                        starquestion.User = user;
                        starquestion.QuestionId = question_id;
                        starquestion.Question = myContext.Questions.Single(b => b.QuestionId == question_id);
                        starquestion.StarTime = DateTime.Now;
                        myContext.Starquestions.Add(starquestion);
                    }
                    else
                    {
                        Starquestion starquestion = myContext.Starquestions.Single(b => b.UserId == user_id && b.QuestionId == question_id);
                        starquestion.Cancel = false;
                    }
                    message.errorCode = 200;
                    message.status = true;
                    myContext.SaveChanges();
                }
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpPut("question")]
        public string cancelQuestionStar(int user_id,int question_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Starquestion starquestion = myContext.Starquestions.Single(b => b.UserId == user_id && b.QuestionId == question_id && b.Cancel == false);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                starquestion.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpGet("question")]
        public string whetherStarQuestion(int user_id, int question_id)
        {
            Message message = new Message();
            bool flag = myContext.Starquestions.Any(b => b.UserId == user_id && b.QuestionId == question_id && b.Cancel == false);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }

        [HttpPost("answer")]
        public string statAnswer(int user_id, int answer_id)
        {
            Message message = new Message();
            User user = new User();
            try
            {
                myContext.DetachAll();
                int max_id = myContext.Users.Select(b => b.UserId).Max();
                int max_question_id = myContext.Answers.Select(b => b.AnswerId).Max();
                if (!myContext.Staranswers.Any(b => b.AnswerId == answer_id && b.UserId == user_id && b.Cancel == false) && user_id <= max_id && answer_id <= max_question_id)
                {
                    user = myContext.Users.Single(b => b.UserId == user_id);
                    /*判断该点赞是否取消过*/
                    if (!myContext.Staranswers.Any(b => b.AnswerId == answer_id && b.UserId == user_id && b.Cancel == true))
                    {
                        Staranswer staranswer = new Staranswer();
                        staranswer.UserId = user_id;
                        staranswer.User = user;
                        staranswer.AnswerId = answer_id;
                        staranswer.Answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                        staranswer.StarTime = DateTime.Now;
                        myContext.Staranswers.Add(staranswer);
                    }
                    else
                    {
                        Staranswer staranswer = myContext.Staranswers.Single(b => b.UserId == user_id && b.AnswerId == answer_id);
                        staranswer.Cancel = false;
                    }
                    message.errorCode = 200;
                    message.status = true;
                    myContext.SaveChanges();
                }
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpPut("answer")]
        public string cancelAnswerStar(int user_id, int answer_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Staranswer staranswer = myContext.Staranswers.Single(b => b.UserId == user_id && b.AnswerId == answer_id&&b.Cancel==false);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                staranswer.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpGet("answer")]
        public string whetherStarAnswer(int user_id, int answer_id)
        {
            Message message = new Message();
            bool flag = myContext.Staranswers.Any(b => b.UserId == user_id && b.AnswerId == answer_id && b.Cancel == false);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }

        [HttpPost("blog")]
        public string statBlog(int user_id, int blog_id)
        {
            Message message = new Message();
            User user = new User();
            try
            {
                myContext.DetachAll();
                int max_id = myContext.Users.Select(b => b.UserId).Max();
                int max_question_id = myContext.Blogs.Select(b => b.BlogId).Max();
                if (!myContext.Starblogs.Any(b => b.BlogId == blog_id && b.UserId == user_id && b.Cancel == false) && user_id <= max_id && blog_id <= max_question_id)
                {
                    user = myContext.Users.Single(b => b.UserId == user_id);
                    /*判断该点赞是否取消过*/
                    if (!myContext.Starblogs.Any(b => b.BlogId == blog_id && b.UserId == user_id && b.Cancel == true))
                    {
                        Starblog starblog = new Starblog();
                        starblog.UserId = user_id;
                        starblog.User = user;
                        starblog.BlogId = blog_id;
                        starblog.Blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                        starblog.StarTime = DateTime.Now;
                        myContext.Starblogs.Add(starblog);
                    }
                    else
                    {
                        Starblog starblog = myContext.Starblogs.Single(b => b.UserId == user_id && b.BlogId == blog_id);
                        starblog.Cancel = false;
                    }
                    message.errorCode = 200;
                    message.status = true;
                    myContext.SaveChanges();
                }
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpPut("blog")]
        public string cancelBlogStar(int user_id, int blog_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Starblog starblog = myContext.Starblogs.Single(b => b.UserId == user_id && b.BlogId == blog_id && b.Cancel == false);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                starblog.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpGet("blog")]
        public string whetherStarBlog(int user_id, int blog_id)
        {
            Message message = new Message();
            bool flag = myContext.Starblogs.Any(b => b.UserId == user_id && b.BlogId == blog_id && b.Cancel == false);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }
    }
}
