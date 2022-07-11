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
    public class LikeController : ControllerBase
    {
        private readonly ModelContext myContext;
        public LikeController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        // -----点赞问题相关----- //
        [HttpPost("answer")]
        public string likeAnswer(int user_id, int answer_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                

            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpPut("answer")]
        public string cancelAnswerLike(int user_id, int answer_id)
        {
            return "";
        }

        [HttpGet("answer")]
        public string whetherLikeAnswer(int user_id, int answer_id)
        {
            return "";
        }


        // -----点赞问题评论相关----- //
        [HttpPost("answer_comment")]
        public string likeAnswerComment(int user_id, int answercomment_id)
        {
            return "";
        }

        [HttpPut("answer_comment")]
        public string cancelAnswerCommentLike(int user_id, int answercomment_id)
        {
            return "";
        }

        [HttpGet("answer_comment")]
        public string whetherLikeAnswerComment(int user_id, int answercomment_id)
        {
            return "";
        }

        // -----点赞动态相关----- //
        [HttpPost("blog")]
        public string likeBlog(int user_id, int blog_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                //object[] vs = new object[2];
                //vs[0] = user_id;
                //vs[1] = blog_id;
                object[] pk = { blog_id, user_id };
                Likeblog likeblog = myContext.Likeblogs.Find(pk);
                if (likeblog == null)
                {
                    Console.WriteLine("没有找到，新建");
                    Likeblog new_likeblog = new();
                    new_likeblog.UserId = user_id;
                    new_likeblog.BlogId = blog_id;
                    new_likeblog.LikeTime = DateTime.Now;
                    myContext.Likeblogs.Add(new_likeblog);
                }
                else
                {
                    Console.WriteLine("找到");
                    likeblog.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpPut("blog")]
        public string cancelBlogLike(int user_id, int blog_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                object[] pk = { blog_id, user_id };
                Likeblog likeblog = myContext.Likeblogs.Find(pk);
                if(likeblog == null)
                {
                    Console.WriteLine("没有找到，无法删除");
                }
                else
                {
                    likeblog.Cancel = true;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpGet("blog")]
        public string whetherLikeBlog(int user_id, int blog_id)
        {
            Message message = new();
            try
            {
                message.status = myContext.Likeblogs.Any(b => b.UserId == user_id && b.BlogId == blog_id && b.Cancel == false);
                message.errorCode = 200;

            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        // -----点赞动态评论相关----- //
        [HttpPost("blog_comment")]
        public string likeBlogComment(int user_id, int blog_id)
        {
            return "";
        }

        [HttpPut("blog_comment")]
        public string cancelBlogCommentLike(int user_id, int blog_id)
        {
            return "";
        }

        [HttpGet("blog_comment")]
        public string whetherLikeBlogComment(int user_id, int blog_id)
        {
            return "";
        }
    }
}
