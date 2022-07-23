using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Back_End.Models;
using System.Text.Json;
using System.IO;
namespace Back_End.Controllers
{
    public class BlogList
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfile { get; set; }
        public int BlogId { get; set; }
        public string BlogSummary { get; set; }
        public string[] BlogTag { get; set; }
        public decimal BlogLike { get; set; }
        public decimal BlogCoin { get; set; }
        public int BlogUserId { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogImage { get; set; }
        public int Count { get; set; }
    }

    public class BlogList2
    {
        public int blog_user_id { get; set; }
        public string blog_user_name { get; set; }
        public string blog_user_profile { get; set; }
        public string blog_summary { get; set; }
        public string[] blog_tag { get; set; }
        public decimal blog_like { get; set; }
        public decimal blog_coin { get; set; }
        public DateTime blog_date { get; set; }
        public string blog_image { get; set; }
    }

    public class BlogContent
    {
        public int user_id { get; set; }
        public byte[] content { get; set; }
        public string summary { get; set; }
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


        [HttpGet()]
        public string getBlog(int blog_id)
        {
            Message message = new Message();
            try
            {
                var bloglist = myContext.Blogs.Where(b => b.BlogId == blog_id && b.BlogVisible == true)
                    .Select(b => new { b.BlogUserId, b.BlogUser.UserName, b.BlogUser.UserProfile, b.BlogTag, b.BlogDate, b.BlogImage, b.BlogLike, b.BlogCoin, b.BlogSummary, b.BlogContent2 })
                    .ToList();
                foreach (var blog in bloglist)
                {
                    message.data.Add("blog_user_id", blog.BlogUserId);
                    User user = myContext.Users.Single(b => b.UserId == blog.BlogUserId);
                    message.data.Add("blog_user_name", user.UserName);
                    message.data.Add("blog_user_profile", user.UserProfile);
                    message.data.Add("blog_tag", blog.BlogTag.Split('-'));
                    message.data.Add("blog_date", blog.BlogDate);
                    message.data.Add("blog_content", blog.BlogContent2);
                    message.data.Add("blog_image", blog.BlogImage);
                    message.data.Add("blog_like", blog.BlogLike);
                    message.data.Add("blog_coin", blog.BlogCoin);
                    message.data.Add("blog_summary", blog.BlogSummary);
                }
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("tag")]
        public string getBlogList(int num, string tag)
        {
            // 返回包含某个tag的给定数量的blog
            Message message = new Message();
            tag = System.Web.HttpUtility.UrlDecode(tag);
            try
            {
                if (tag == null)
                {
                    tag = "";
                }
                string[] tags = tag.Split('-');
                int[] blogs_id = { };
                List<BlogList> list_blogs = new();
                foreach (var val in tags)
                {
                    bool flag = true;
                    var blogs = myContext.Blogs
                        .OrderByDescending(a => a.Blogcomments.Count * 2 + a.BlogLike * 3 + a.BlogCoin * 5)
                        .Select(b => new
                        {
                            b.BlogUser.UserId,
                            b.BlogUser.UserName,
                            b.BlogUser.UserProfile,
                            b.BlogId,
                            b.BlogSummary,
                            b.BlogTag,
                            b.BlogLike,
                            b.BlogCoin,
                            b.BlogUserId,
                            b.BlogDate,
                            b.BlogImage,
                        })
                        .Where(c => c.BlogTag.Contains(val) && !blogs_id.Contains(c.BlogId))
                        .Take(num).ToList();
                    foreach (var blog in blogs)
                    {
                        BlogList new_blog_info = new();
                        new_blog_info.UserId = blog.UserId;
                        new_blog_info.UserName = blog.UserName;
                        new_blog_info.UserProfile = blog.UserProfile;
                        new_blog_info.BlogId = blog.BlogId;
                        new_blog_info.BlogSummary = blog.BlogSummary;
                        new_blog_info.BlogTag = blog.BlogTag.Split('-');
                        new_blog_info.BlogLike = (decimal)blog.BlogLike;
                        new_blog_info.BlogCoin = (decimal)blog.BlogCoin;
                        new_blog_info.BlogUserId = (int)blog.BlogUserId;
                        new_blog_info.BlogDate = blog.BlogDate;
                        new_blog_info.BlogImage = blog.BlogImage;
                        list_blogs.Add(new_blog_info);
                        if (list_blogs.Count >= num)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
                message.errorCode = 200;
                message.status = true;
                message.data.Add("tag", tags.ToArray());
                message.data.Add("count", list_blogs.Count);
                message.data.Add("blog", list_blogs.ToArray());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());

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
                    .Where(a => a.BlogVisible == true)
                    .OrderByDescending(c => c.BlogDate)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage, b.Blogcomments.Count, b.BlogUser.UserId, b.BlogUser.UserName, b.BlogUser.UserProfile })
                    .ToList();
                if (bloglist.Count > num)
                    bloglist.RemoveRange(num, bloglist.Count - num);
                List<BlogList> blogs = new();
                foreach (var blog in bloglist)
                {
                    BlogList list = new();
                    list.UserId = blog.UserId;
                    list.UserName = blog.UserName;
                    list.UserProfile = blog.UserProfile;
                    list.BlogUserId = (int)blog.BlogUserId;
                    list.BlogSummary = blog.BlogSummary;
                    list.BlogImage = blog.BlogImage;
                    list.BlogId = blog.BlogId;
                    list.BlogDate = blog.BlogDate;
                    list.BlogCoin = (decimal)blog.BlogCoin;
                    list.Count = blog.Count;
                    list.BlogTag = blog.BlogTag.Split('-');
                    list.BlogLike = (decimal)blog.BlogLike;
                    blogs.Add(list);
                }
                message.data.Add("blog", blogs.ToArray());
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
                    .OrderByDescending(c => c.Blogcomments.Count * 2 + c.BlogLike * 3 + c.BlogCoin * 5)
                    .Select(b => new { b.BlogId, b.BlogSummary, b.BlogTag, b.BlogLike, b.BlogCoin, b.BlogUserId, b.BlogDate, b.BlogImage, b.Blogcomments.Count, b.BlogUser.UserId, b.BlogUser.UserName, b.BlogUser.UserProfile })
                    .ToList();
                if (bloglist.Count > num)
                    bloglist.RemoveRange(num, bloglist.Count - num);
                List<BlogList> blogs = new();
                foreach (var blog in bloglist)
                {
                    BlogList list = new();
                    list.UserId = blog.UserId;
                    list.UserName = blog.UserName;
                    list.UserProfile = blog.UserProfile;
                    list.BlogUserId = (int)blog.BlogUserId;
                    list.BlogSummary = blog.BlogSummary;
                    list.BlogImage = blog.BlogImage;
                    list.BlogId = blog.BlogId;
                    list.BlogDate = blog.BlogDate;
                    list.BlogCoin = (decimal)blog.BlogCoin;
                    list.Count = blog.Count;
                    list.BlogTag = blog.BlogTag.Split('-');
                    list.BlogLike = (decimal)blog.BlogLike;
                    blogs.Add(list);
                }
                message.data.Add("blog", blogs.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpGet("comment")]
        public string getBlogComment(int blog_id)
        {
            Message message = new Message();
            try
            {
                var blogcomment = myContext.Blogcomments.Where(b => b.BlogCommentFather == blog_id);
                message.data["comment_num"] = blogcomment.Count();
                var list = blogcomment
                    .Select(b => new { b.BlogCommentId, b.BlogCommentUser.UserName, b.BlogCommentUser.UserProfile, b.BlogCommentContent, b.BlogCommentLike, b.InverseBlogCommentReplyNavigation.Count, })
                    .ToList();
                message.data["comment_list"] = list.ToArray();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }


        [HttpGet("reply")]
        public string getBlogCommentReply(int blog_comment_id)
        {
            Message message = new Message();
            try
            {
                var blogcomment = myContext.Blogcomments.Where(b => b.BlogCommentReply == blog_comment_id && b.BlogCommentVisible == true);
                message.data["reply_num"] = blogcomment.Count();
                var list = blogcomment
                    .Select(b => new { b.BlogCommentId, b.BlogCommentUser.UserName, b.BlogCommentUser.UserProfile, b.BlogCommentContent, b.BlogCommentLike, b.InverseBlogCommentReplyNavigation.Count, })
                    .ToList();
                message.data["reply_list"] = list.ToArray();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("comment")]
        public string sendComment(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                //answer_id = int.Parse(Request.Form["answer_id"]);
                //answer_comment_user_id = int.Parse(Request.Form["answer_comment_user_id"]);
                //answer_comment_content = Request.Form["answer_comment_content"];
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());
                int blog_comment_user_id = int.Parse(front_end_data.GetProperty("blog_comment_user_id").ToString());
                string blog_comment_content = front_end_data.GetProperty("blog_comment_content").ToString();
                Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                User user = myContext.Users.Single(b => b.UserId == blog_comment_user_id);
                int id = myContext.Blogcomments.Count() + 1;
                Blogcomment blogcomment = new Blogcomment();
                blogcomment.BlogCommentUser = user;
                blogcomment.BlogCommentVisible = true;
                blogcomment.BlogCommentReply = null;
                blogcomment.BlogCommentFather = blog_id;
                blogcomment.BlogCommentContent = blog_comment_content;
                blogcomment.BlogCommentFatherNavigation = blog;
                blogcomment.BlogCommentId = id;
                blogcomment.BlogCommentTime = DateTime.Now;
                blogcomment.BlogCommentUserId = blog_comment_user_id;
                myContext.Blogcomments.Add(blogcomment);
                myContext.SaveChanges();
                message.data.Add("blog_comment_id", id);
                message.status = true;
                message.errorCode = 200;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("reply")]
        public string sendReply(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int comment_id = int.Parse(front_end_data.GetProperty("comment_id").ToString());
                int reply_user_id = int.Parse(front_end_data.GetProperty("reply_user_id").ToString());
                string reply_content = front_end_data.GetProperty("reply_content").ToString();
                Blogcomment blogcomment = myContext.Blogcomments.Single(b => b.BlogCommentId == comment_id);
                User user = myContext.Users.Single(b => b.UserId == reply_user_id);
                Blogcomment new_comment = new Blogcomment();
                new_comment.BlogCommentContent = reply_content;
                new_comment.BlogCommentFather = null;
                new_comment.BlogCommentReply = comment_id;
                new_comment.BlogCommentReplyNavigation = blogcomment;
                int id = myContext.Blogcomments.Count() + 1;
                new_comment.BlogCommentId = id;
                new_comment.BlogCommentTime = DateTime.Now;
                new_comment.BlogCommentUserId = reply_user_id;
                new_comment.BlogCommentUser = user;
                new_comment.BlogCommentVisible = true;
                myContext.Blogcomments.Add(new_comment);
                myContext.SaveChanges();
                message.data["comment_id"] = id;
                message.errorCode = 200;
                message.status = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost]
        public string sendBlog(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                string content = front_end_data.GetProperty("content").ToString();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string summary = front_end_data.GetProperty("summary").ToString();
                string tag = front_end_data.GetProperty("tag").ToString();
                string img_base64 = front_end_data.GetProperty("image_url").ToString();
                Blog blog = new Blog();
                //
                byte[] img_bytes = Encoding.UTF8.GetBytes(content);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                int id = myContext.Blogs.Count() + 1;
                string path = "blog/content/" + id.ToString() + ".html";
                string imageurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                client.PutObject(OssHelp.bucketName, path, stream);
                blog.BlogContent2 = imageurl;
                blog.BlogUserId = user_id;
                blog.BlogId = id;
                blog.BlogTag = tag;
                blog.BlogUser = myContext.Users.Single(b => b.UserId == user_id);
                //blog.BlogContent = content;
                blog.BlogDate = DateTime.Now;
                blog.BlogImage = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/user_profile/5.png";
                blog.BlogSummary = summary;
                blog.BlogVisible = true;
                Blogchecking blogchecking = new();
                blogchecking.AdministratorId = 0;
                blogchecking.BlogId = id;
                blogchecking.BlogDate = blog.BlogDate;
                blogchecking.ReviewResult = "待审核";
                //存第一张图到oss
                if (img_base64 != "")
                {
                    string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
                    img_base64 = img_base64.Split("base64,")[1];//非常重要
                    byte[] img_bytes_ = Convert.FromBase64String(img_base64);
                    MemoryStream stream_ = new MemoryStream(img_bytes_, 0, img_bytes_.Length);
                    string path_ = "blog/" + id.ToString() + type;
                    client.PutObject(OssHelp.bucketName, path_, stream_);
                    string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path_;
                    blog.BlogImage = imgurl;
                }
                blogchecking.Blog = blog;
                myContext.Blogs.Add(blog);
                myContext.Blogcheckings.Add(blogchecking);
                myContext.SaveChanges();
                message.data["blog_id"] = id;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        //[HttpPost("image")]
        //public string uploadImage(dynamic front_end_data)
        //{
        //    Message message = new();
        //    try
        //    {
        //        myContext.DetachAll();
        //        string img_base64 = front_end_data.GetProperty("img").ToString();
        //        int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
        //        int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());
        //        Blog blog = myContext.Blogs.Single(b => b.BlogId == blog_id && b.BlogUserId == user_id);
        //        string type="."+img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
        //        img_base64 = img_base64.Split("base64,")[1];//非常重要
        //        byte[] img_bytes = Convert.FromBase64String(img_base64);
        //        var client = OssHelp.createClient();
        //        MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
        //        string path = "blog/" + blog_id.ToString() + type;
        //        client.PutObject(OssHelp.bucketName, path, stream);
        //        string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
        //        blog.BlogImage = imgurl;
        //        myContext.SaveChanges();
        //        message.data.Add("imageurl", imgurl);
        //        message.status = true;
        //        message.errorCode = 200;
        //    }
        //    catch(Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return message.ReturnJson();
        //}

        [HttpDelete("delete")]
        public void deleteBlog() { }
    }
}
