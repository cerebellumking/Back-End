using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
namespace Back_End.Controllers
{
    public class BlogList
    {
        public int blog_id { get; set; }
        public string blog_summary { get; set; }
        public string blog_tag { get; set; }
        public decimal blog_like { get; set; }
        public decimal blog_coin { get; set; }
        public int blog_user_id { get; set; }
        public DateTime blog_date { get; set; }
        public string blog_image { get; set; }
        public int blog_comment_num { get; set; }
    }



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
                tag = System.Web.HttpUtility.UrlDecode(tag);
                var bloglist = myContext.Blogs
                    .Where(a => a.BlogVisible == true && a.BlogTag.Contains(tag))
                    .OrderByDescending(c => c.Blogcomments.Count * 3 + c.BlogLike * 3 + c.BlogCoin * 4)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage, b.Blogcomments.Count })
                    .ToList();
                foreach (var blog in bloglist)
                {
                    string[] tag_array = tag.Split(',');
                    bool flag = true;
                    foreach (var val in tag_array)
                    {
                        if (!blog.BlogTag.Contains(val))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                        bloglist.Remove(blog);
                }
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
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage,b.Blogcomments.Count })
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
                    //.OrderByDescending(c => c.Blogcomments.Count)
                    .OrderByDescending(c=>c.Blogcomments.Count*3+c.BlogLike*3+c.BlogCoin*4)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage, b.Blogcomments.Count })
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
