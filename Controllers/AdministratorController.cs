using Microsoft.AspNetCore.Http;
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
    public class AdministratorController : ControllerBase
    {
        private readonly ModelContext myContext;
        public AdministratorController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getAdministratorInfo(int admin_id)
        {
            Message message = new();
            try
            {
                Administrator administrator = myContext.Administrators.Single(b => b.AdministratorId == admin_id);
                message.data.Add("AdministratorId", administrator.AdministratorId);
                message.data.Add("AdministratorEmail", administrator.AdministratorEmail);
                message.data.Add("AdministratorPhone", administrator.AdministratorPhone);
                message.data.Add("AdministratorName", administrator.AdministratorName);
                message.data.Add("AdministratorProfile", administrator.AdministratorProfile);
                message.data.Add("AdministratorGender", administrator.AdministratorGender);
                message.data.Add("AdministratorCreatetime", administrator.AdministratorCreatetime);
                message.status = true;
                message.errorCode = 200;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
    }
}
