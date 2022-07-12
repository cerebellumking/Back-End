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

        [HttpGet]
        public string getNewsFlash()
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
                    b.NewsFlashContent,
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
        
    }
}
