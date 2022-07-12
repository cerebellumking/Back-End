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
                if (myContext.Answers.Any(b => b.AnswerId == answer_id && b.AnswerUserId == user_id))
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = "不能给自己投币！";
                }
                else
                {
                    message.errorCode = 200;
                    message.status = myContext.Coinanswers.Any(b => b.AnswerId == answer_id && b.UserId == user_id);
                    message.data["answer_coin"] = myContext.Answers.Single(b => b.AnswerId == answer_id).AnswerCoin;
                }
               
                //message.status = myContext.Coinanswers.Any(b => b.AnswerId == answer_id && b.UserId == user_id) || myContext.Answers.Any(b => b.AnswerId == answer_id && b.AnswerUserId == user_id);
                
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
                if (user.UserCoin >= num)
                {
                    Coinanswer coinanswer = new Coinanswer();
                    coinanswer.User = user;
                    coinanswer.Answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                    coinanswer.CoinTime = DateTime.Now;
                    coinanswer.UserId = user_id;
                    coinanswer.AnswerId = answer_id;
                    myContext.Coinanswers.Add(coinanswer);
                    coinanswer.Answer.AnswerCoin += num;
                    coinanswer.User.UserCoin -= num;
                    myContext.SaveChanges();
                    message.data["user_coin_left"] = coinanswer.User.UserCoin;
                    message.data["answer_coin"] = coinanswer.Answer.AnswerCoin;
                    message.errorCode = 200;
                    message.status = true;
                }
                else
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = "投币数额超过持有鸟币数";
                }
            }catch(Exception e)
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
                if (myContext.Blogs.Any(b => b.BlogId == blog_id && b.BlogUserId == user_id))
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = "不能给自己投币！";
                }
                else
                {
                    message.errorCode = 200;
                    message.status = myContext.Coinblogs.Any(b => b.BlogId == blog_id && b.UserId == user_id);
                    message.data["blog_coin"] = myContext.Blogs.Single(b => b.BlogId == blog_id).BlogCoin;
                }
                //message.status = myContext.Coinblogs.Any(b => b.BlogId == blog_id && b.UserId == user_id)||myContext.Blogs.Any(b=>b.BlogId==blog_id&&b.BlogUserId==user_id);
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
                if (user.UserCoin >= num)
                {
                    Coinblog coinblog = new Coinblog();
                    coinblog.User = user;
                    coinblog.Blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                    coinblog.CoinTime = DateTime.Now;
                    coinblog.UserId = user_id;
                    coinblog.BlogId = blog_id;
                    myContext.Coinblogs.Add(coinblog);
                    coinblog.Blog.BlogCoin += num;
                    coinblog.User.UserCoin -= num;
                    myContext.SaveChanges();
                    message.data["user_coin_left"] = coinblog.User.UserCoin;
                    message.data["blog_coin"] = coinblog.Blog.BlogCoin;
                    message.errorCode = 200;
                    message.status = true;
                }
                else
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = "投币数额超过持有鸟币数";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
