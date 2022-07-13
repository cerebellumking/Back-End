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
        public string getNewsFlashs()
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
        public string getNewsFlash(int newsflash_id)
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
    }
}
