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
        public string starQuestion(int user_id,int question_id)
        {
            Message message = new Message();
            try
            {
                object[] pk = { user_id, question_id };
                Starquestion old_starquestion = myContext.Starquestions.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_starquestion==null)
                {
                    Starquestion starquestion = new Starquestion();
                    starquestion.UserId = user_id;
                    starquestion.User = myContext.Users.Single(b => b.UserId == user_id);
                    starquestion.QuestionId = question_id;
                    starquestion.Question = myContext.Questions.Single(b => b.QuestionId == question_id);
                    starquestion.StarTime = DateTime.Now;
                    myContext.Starquestions.Add(starquestion);
                }
                else
                {
                    old_starquestion.StarTime = DateTime.Now;
                    old_starquestion.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
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
                starquestion.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
        public string starAnswer(int user_id, int answer_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                object[] pk = { user_id, answer_id };
                Staranswer old_staranswer = myContext.Staranswers.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_staranswer==null)
                {
                    User user = myContext.Users.Single(b => b.UserId == user_id);
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
                    old_staranswer.StarTime = DateTime.Now;
                    old_staranswer.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
                staranswer.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
        public string starBlog(int user_id, int blog_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                object[] pk = { user_id, blog_id };
                Starblog old_starblog = myContext.Starblogs.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_starblog==null)
                {
                    User user = myContext.Users.Single(b => b.UserId == user_id);
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
                    old_starblog.StarTime = DateTime.Now;
                    old_starblog.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
                starblog.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
