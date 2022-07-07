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
    public class UserInfoController : ControllerBase
    {
        private readonly ModelContext myContext;
        public UserInfoController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getInfo(int user_id)
        {
            Message message = new Message();
            try
            {
                Qualification qualification = myContext.Qualifications.Single(b=>b.UserId==user_id);
                message.data.Add("identity_id", qualification.IdentityId);
                message.data.Add("identity", qualification.Identity);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                message.data.Add("user_follower", user.UserFollower);
                message.data.Add("user_follows", user.UserFollows);
                message.data.Add("user_level", user.UserLevel);
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {
            }
            return message.ReturnJson();
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
