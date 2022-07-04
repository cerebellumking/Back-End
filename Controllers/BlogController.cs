using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Back_End.Models;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        //private readonly ModelContext myContext;
        //public BlogController(ModelContext modelContext)
        //{
        //    myContext = modelContext;
        //}

        [HttpPost("post")]
        public void postBlog() { }

        [HttpPost("show")]
        public void showBlog() { }

        [HttpDelete("delete")]
        public void deleteBlog() { }
    }
}
