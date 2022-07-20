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
    public class NewsFlashController : Controller
    {
        private readonly ModelContext myContext;
        public NewsFlashController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet("all")]
        public string getAllNewsFlashs()
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                var newsflashs = myContext.Newsflashes.Where(c=>c.NewsFlashVisible == true)
                    .OrderBy(a => a.NewsFlashDate)
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
                string news_flash_title = front_end_data.GetProperty("news_flash_title").ToString();
                string news_flash_region = front_end_data.GetProperty("news_flash_region").ToString();
                string news_flash_content = front_end_data.GetProperty("news_flash_content").ToString();
                string news_flash_tag = front_end_data.GetProperty("news_flash_tag").ToString();
                //string news_flash_image = front_end_data.GetProperty("news_flash_image").ToString();
                myContext.DetachAll();
                Newsflash newsflash = new();
                var count = myContext.Newsflashes.Count();
                int id = 1;
                if (count != 0)
                {
                    id = myContext.Newsflashes.Select(b => b.NewsFlashId).Max() + 1;
                }
                newsflash.NewsFlashId = id;
                newsflash.NewsFlashTitle = news_flash_title;
                newsflash.NewsFlashDate = DateTime.Now;
                newsflash.NewsFlashRegion = news_flash_region;
                newsflash.NewsFlashContent = news_flash_content;
                newsflash.NewsFlashTag = news_flash_tag;
                newsflash.NewsFlashVisible = true;
                newsflash.NewsFlashSummary = news_flash_content.Substring(0, 100);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ToString();
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
