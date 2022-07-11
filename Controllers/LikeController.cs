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
                object[] pk = { answer_id, user_id }; // 根据主键查询
                Likeanswer like_answer = myContext.Likeanswers.Find(pk);
                if (like_answer == null)
                {
                    // 若不存在，则新建
                    Likeanswer new_like_answer = new();
                    new_like_answer.UserId = user_id;
                    new_like_answer.AnswerId = answer_id;
                    new_like_answer.LikeTime = DateTime.Now;
                    myContext.Likeanswers.Add(new_like_answer);
                }
                else
                {
                    // 若存在，则将Cancel改为false
                    like_answer.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpPut("answer")]
        public string cancelAnswerLike(int user_id, int answer_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                object[] pk = { answer_id, user_id };
                Likeanswer like_answer = myContext.Likeanswers.Find(pk);
                if (like_answer != null)
                {
                    // 若存在，则删除；若不存在则不管
                    like_answer.Cancel = true;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpGet("answer")]
        public string whetherLikeAnswer(int user_id, int answer_id)
        {
            Message message = new();
            try
            {
                message.status = myContext.Likeanswers.Any(b => b.UserId == user_id && b.AnswerId == answer_id && b.Cancel == false);
                message.errorCode = 200;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }


        // -----点赞问题评论相关----- //
        [HttpPost("answer_comment")]
        public string likeAnswerComment(int user_id, int answer_comment_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                object[] pk = { answer_comment_id, user_id };
                Likeanswercomment like_answer_comment = myContext.Likeanswercomments.Find(pk);
                if (like_answer_comment == null)
                {
                    Likeanswercomment new_like_answer_comment = new();
                    new_like_answer_comment.UserId = user_id;
                    new_like_answer_comment.AnswerCommentId = answer_comment_id;
                    new_like_answer_comment.LikeTime = DateTime.Now;
                    myContext.Likeanswercomments.Add(new_like_answer_comment);
                }
                else
                {
                    like_answer_comment.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpPut("answer_comment")]
        public string cancelAnswerCommentLike(int user_id, int answer_comment_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                object[] pk = { answer_comment_id, user_id };
                Likeanswercomment like_answer_comment = myContext.Likeanswercomments.Find(pk);
                if (like_answer_comment != null)
                {
                    like_answer_comment.Cancel = true;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpGet("answer_comment")]
        public string whetherLikeAnswerComment(int user_id, int answer_comment_id)
        {
            Message message = new();
            try
            {
                message.status = myContext.Likeanswercomments.Any(b => b.UserId == user_id && b.AnswerCommentId == answer_comment_id && b.Cancel == false);
                message.errorCode = 200;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        // -----点赞动态相关----- //
        [HttpPost("blog")]
        public string likeBlog(int user_id, int blog_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                object[] pk = { blog_id, user_id };
                Likeblog like_blog = myContext.Likeblogs.Find(pk);
                if (like_blog == null)
                {
                    Likeblog new_like_blog = new();
                    new_like_blog.UserId = user_id;
                    new_like_blog.BlogId = blog_id;
                    new_like_blog.LikeTime = DateTime.Now;
                    myContext.Likeblogs.Add(new_like_blog);
                }
                else
                {
                    like_blog.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
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
                if (likeblog != null)
                {
                    likeblog.Cancel = true;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
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
        public string likeBlogComment(int user_id, int blog_comment_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                object[] pk = { blog_comment_id, user_id };
                Likeblogcomment like_blog_comment = myContext.Likeblogcomments.Find(pk);
                if (like_blog_comment == null)
                {
                    Likeblogcomment new_like_blog_comment = new();
                    new_like_blog_comment.UserId = user_id;
                    new_like_blog_comment.BlogCommentId = blog_comment_id;
                    new_like_blog_comment.LikeTime = DateTime.Now;
                    myContext.Likeblogcomments.Add(new_like_blog_comment);
                }
                else
                {
                    like_blog_comment.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpPut("blog_comment")]
        public string cancelBlogCommentLike(int user_id, int blog_comment_id)
        {
            Message message = new();
            try
            {
                object[] pk = { blog_comment_id, user_id };
                Likeblogcomment like_blog_comment = myContext.Likeblogcomments.Find(pk);
                if (like_blog_comment != null)
                {
                    like_blog_comment.Cancel = true;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }

        [HttpGet("blog_comment")]
        public string whetherLikeBlogComment(int user_id, int blog_comment_id)
        {
            Message message = new();
            try
            {
                message.status = myContext.Likeblogcomments.Any(b => b.UserId == user_id && b.BlogCommentId == blog_comment_id && b.Cancel == false);
                message.errorCode = 200;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                message.errorCode = 300;
            }
            return message.ReturnJson();
        }
    }
}
