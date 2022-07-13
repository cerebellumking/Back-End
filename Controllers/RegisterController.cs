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
    public class RegisterController : ControllerBase
    {
        private readonly ModelContext myContext;
        public RegisterController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost]
        public string Register(string user_phone, string user_password)
        {
            RegisterMessage message = new RegisterMessage();
            try
            {
                user_phone = Request.Form["user_phone"];
                user_password = Request.Form["user_password"];
                myContext.DetachAll();
                User user = new User();
                user.UserGender = "m";
                user.UserName = "用户" + user_phone;
                user.UserPhone = user_phone;
                user.UserPassword = user_password;
                user.UserCreatetime = DateTime.Now;
                var count = myContext.Users.Count();
                int id= myContext.Users.Count() + 1;
                user.UserId = id;
                myContext.Users.Add(user);
                myContext.SaveChanges();
                message.status = true;
                message.data["user_id"] = id;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("verifyCode")]
        public string sendVerifyCode()
        {
            return "成功";
        }

    }
}
