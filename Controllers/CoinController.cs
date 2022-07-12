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
    public class CoinController : ControllerBase
    {
        private readonly ModelContext myContext;
        public CoinController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet("answer")]
        public string whetherCoinAnswer(int user_id,int answer_id)
        {
            Message message = new Message();
            try
            {
                 message.errorCode = 200;
                 message.status = myContext.Coinanswers.Any(b => b.AnswerId == answer_id && b.UserId == user_id);
                 message.data["answer_coin"] = myContext.Answers.Single(b => b.AnswerId == answer_id).AnswerCoin;   
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("answer")]
        public string coinAnswer(int user_id,int answer_id,int num)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                User user = myContext.Users.Single(b => b.UserId == user_id);
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                if (user.UserCoin <= 0 || user.UserCoin < num)
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 1;
                    message.data["user_coin_left"] = user.UserCoin;
                    return message.ReturnJson();
                }
                
                if (myContext.Answers.Any(b => b.AnswerId == answer_id && b.AnswerUserId == user_id))
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 2;
                    return message.ReturnJson();
                }
                //正常操作
                Coinanswer coinanswer = new Coinanswer();
                coinanswer.CoinTime = DateTime.Now;
                coinanswer.UserId = user_id;
                coinanswer.AnswerId = answer_id;
                answer.AnswerCoin += num;
                user.UserCoin -= num;
                coinanswer.User = user;
                coinanswer.Answer = answer;
                myContext.Coinanswers.Add(coinanswer);
                myContext.SaveChanges();
                message.data["user_coin_left"] = user.UserCoin;
                message.data["answer_coin"] = answer.AnswerCoin;
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }


        [HttpGet("blog")]
        public string whetherCoinBlog(int user_id, int blog_id)
        {
            Message message = new Message();
            try
            {
                message.errorCode = 200;
                message.status = myContext.Coinblogs.Any(b => b.BlogId == blog_id && b.UserId == user_id);
                message.data["blog_coin"] = myContext.Blogs.Single(b => b.BlogId == blog_id).BlogCoin;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("blog")]
        public string coinBlog(int user_id, int blog_id, int num)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                User user = myContext.Users.Single(b => b.UserId == user_id);
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                if (user.UserCoin <= 0 || user.UserCoin < num)
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 1;
                    message.data["user_coin_left"] = user.UserCoin;
                    return message.ReturnJson();
                }
                if (myContext.Blogs.Any(b => b.BlogId == blog_id && b.BlogUserId == user_id))
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 2;
                    return message.ReturnJson();
                }
                //正常操作
                Coinblog coinblog = new Coinblog();
                coinblog.CoinTime = DateTime.Now;
                coinblog.UserId = user_id;
                coinblog.BlogId = blog_id;
                blog.BlogCoin += num;
                user.UserCoin -= num;
                coinblog.User = user;
                coinblog.Blog = blog;
                myContext.Coinblogs.Add(coinblog);
                myContext.SaveChanges();
                message.data["user_coin_left"] = coinblog.User.UserCoin;
                message.data["blog_coin"] = coinblog.Blog.BlogCoin;
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
