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
    public class MoneyController : ControllerBase
    {
        //private readonly ModelContext myContext;
        //public MoneyController(ModelContext modelContext)
        //{
        //    myContext = modelContext;
        //}

        [HttpPost("record")]
        public void getRecord() { }
    }
}
