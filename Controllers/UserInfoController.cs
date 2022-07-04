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
    public class UserInfoController : ControllerBase
    {
        //private readonly ModelContext myContext;
        //public UserInfoController(ModelContext modelContext)
        //{
        //    myContext = modelContext;
        //}

        [HttpPost("show")]
        public void getInfo()
        {
        }

        [HttpPut("change")]
        public void changeInfo()
        {

        }

        [HttpDelete]
        public void deleteInfo()
        {

        }

        [HttpPut]
        public void changeVerification()
        {

        }

    }
}
