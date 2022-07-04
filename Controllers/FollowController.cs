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
    public class FollowController : ControllerBase
    {
        //private readonly ModelContext myContext;
        //public FollowController(ModelContext modelContext)
        //{
        //    myContext = modelContext;
        //}

        [HttpPost("show")]
        public void showFollowInfo()
        {

        }
        [HttpDelete("delete")]
        public void deleteFollow() { }

    }
}
