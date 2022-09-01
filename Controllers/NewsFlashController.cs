using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using System.Text;
using System.IO;

namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsFlashController : Controller
    {
        private readonly ModelContext myContext;
        public NewsFlashController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet("num")]
        public string getNewsFlashsNum()
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int count = myContext.Newsflashes.Count(b => b.NewsFlashVisible == true);
                message.errorCode = 200;
                message.status = true;
                message.data.Add("num", count);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("all")]
        public string getAllNewsFlashs(int page,int page_size=5)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                var newsflashs = myContext.Newsflashes.Where(c=>c.NewsFlashVisible == true)
                    .OrderByDescending(a => a.NewsFlashDate)
                    .Select(b => new
                {
                    b.NewsFlashId,
                    b.NewsFlashDate,
                    b.NewsFlashTitle,
                    b.NewsFlashTag,
                    b.NewsFlashRegion,
                    b.NewsFlashSummary,
                    //b.NewsFlashContent,
                    b.NewsFlashImage
                })
                    .Skip(page_size * (page - 1))
                    .Take(page_size)
                    .ToList();
                message.errorCode = 200;
                message.status = true;
                message.data.Add("count", newsflashs.Count);
                message.data.Add("newsflashs", newsflashs.ToArray());
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
        
        [HttpGet("single")]
        public string getSingleNewsFlash(int newsflash_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                var newsflash = myContext.Newsflashes.Single(b => b.NewsFlashId == newsflash_id && b.NewsFlashVisible == true);
                message.data.Add("NewsFlashId", newsflash.NewsFlashId);
                message.data.Add("NewsFlashTitle", newsflash.NewsFlashTitle);
                message.data.Add("NewsFlashDate", newsflash.NewsFlashDate);
                message.data.Add("NewsFlashTag", newsflash.NewsFlashTag);
                message.data.Add("NewsFlashRegion", newsflash.NewsFlashRegion);
                message.data.Add("NewsFlashContent", newsflash.NewsFlashContent);
                message.data.Add("NewsFlashImage", newsflash.NewsFlashImage);
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("search")]
        public string getNewsFlashsBySearch(string keyword)
        {
            Message message = new();
            keyword = System.Web.HttpUtility.UrlDecode(keyword);
            try
            {
                var newsflashs = myContext.Newsflashes
                    .Where(c => c.NewsFlashVisible == true && c.NewsFlashTitle.Contains(keyword))
                    .OrderByDescending(b => b.NewsFlashDate)
                    .Select(b => new
                    {
                        b.NewsFlashId,
                        b.NewsFlashDate,
                        b.NewsFlashTitle,
                        b.NewsFlashTag,
                        b.NewsFlashRegion,
                        b.NewsFlashSummary,
                        //b.NewsFlashContent,
                        b.NewsFlashImage
                    }).ToList();
                message.data.Add("count", newsflashs.Count);
                message.data.Add("newsflashs", newsflashs.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("release")]
        public string releaseNewsFlash(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                string title = front_end_data.GetProperty("title").ToString();
                string tag = front_end_data.GetProperty("tag").ToString();
                string region = front_end_data.GetProperty("region").ToString();
                string summary = front_end_data.GetProperty("summary").ToString();
                string content = front_end_data.GetProperty("content").ToString();
                string img_base64 = front_end_data.GetProperty("image_url").ToString();

                Newsflash newsflash = new();

                byte[] img_bytes = Encoding.UTF8.GetBytes(content);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                int id = myContext.Newsflashes.Count() + 1;
                string path = "newsflash/content/" + id.ToString() + ".html";
                string imageurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                client.PutObject(OssHelp.bucketName, path, stream);

                newsflash.NewsFlashContent = imageurl;
                newsflash.NewsFlashId = id;
                newsflash.NewsFlashTitle = title;
                newsflash.NewsFlashDate = DateTime.Now;
                newsflash.NewsFlashRegion = region;
                newsflash.NewsFlashVisible = true;
                newsflash.NewsFlashSummary = summary;
                newsflash.NewsFlashTag = tag;

                if(img_base64 != "")
                {
                    string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
                    img_base64 = img_base64.Split("base64,")[1];
                    byte[] img_bytes_ = Convert.FromBase64String(img_base64);
                    MemoryStream stream_ = new MemoryStream(img_bytes_, 0, img_bytes_.Length);
                    string path_ = "newsflash/" + id.ToString() + type;
                    client.PutObject(OssHelp.bucketName, path_, stream_);
                    string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path_;
                    newsflash.NewsFlashImage = imgurl;
                }

                myContext.Newsflashes.Add(newsflash);
                myContext.SaveChanges();
                message.data["newsflash_id"] = id;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpDelete]
        public string deleteNewsFlash(int newsflash_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                var newsflash = myContext.Newsflashes.Single(b => b.NewsFlashId == newsflash_id);
                newsflash.NewsFlashVisible = false;
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
    }
}
