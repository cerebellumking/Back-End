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
    public class BlogController : ControllerBase
    {
        private readonly ModelContext myContext;
        public BlogController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost()]
        public void postBlog() { }


        [HttpGet()]
        public string getBlog(int blog_id)
        {
            Message message = new Message();
            try
            {
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id&&b.BlogVisible==true);
                message.data.Add("blog_user_id", blog.BlogUserId);
                User user = myContext.Users.Single(b => b.UserId == blog.BlogUserId);
                message.data.Add("blog_user_name", user.UserName);
                message.data.Add("blog_user_profile", user.UserProfile);
                //message.data.Add("blog_user_name", blog.BlogUser.UserName);
                message.data.Add("blog_tag", blog.BlogTag);
                message.data.Add("blog_date", blog.BlogDate.AddHours(8));
                message.data.Add("blog_content", blog.BlogContent);
                message.data.Add("blog_image", blog.BlogImage);
                message.data.Add("blog_like", blog.BlogLike);
                message.data.Add("blog_coin", blog.BlogCoin);
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpGet("tag")]
        public string getBlogList(int num,string tag)
        {
            Message message = new Message();
            try
            {
                var bloglist = myContext.Blogs
                    .Where(a => a.BlogVisible == true && a.BlogTag.Contains(tag))
                    .OrderByDescending(c=>c.Blogcomments.Count)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate,b.BlogImage})
                    .ToList();
                if (bloglist.Count > num)
                    bloglist.RemoveRange(num, bloglist.Count - num);
                message.data.Add("blog", bloglist.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpGet("time")]
        public string getBlogListByTime(int num)
        {
            Message message = new Message();
            try
            {
                var bloglist = myContext.Blogs
                    .Where(a => a.BlogVisible == true )
                    .OrderByDescending(c => c.BlogDate)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage })
                    .ToList();
                if (bloglist.Count > num)
                    bloglist.RemoveRange(num, bloglist.Count - num);
                message.data.Add("blog", bloglist.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpGet("heat")]
        public string getBlogListByHeat(int num)
        {
            Message message = new Message();
            try
            {
                var bloglist = myContext.Blogs
                    .Where(a => a.BlogVisible == true)
                    .OrderByDescending(c => c.Blogcomments.Count)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage })
                    .ToList();
                if (bloglist.Count > num)
                    bloglist.RemoveRange(num, bloglist.Count - num);
                message.data.Add("blog", bloglist.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpDelete("delete")]
        public void deleteBlog() { }
    }
}
