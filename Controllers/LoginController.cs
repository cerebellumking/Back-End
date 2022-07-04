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
    public class LoginController : ControllerBase
    {
        private readonly ModelContext myContext;
        public LoginController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost]
         public string Login(int user_id,string user_password)
        {
            LoginMessage message = new LoginMessage();
            //var user = myContext.Users
            //       .Single(b => b.UserId == user_id);
            ////message.data.Add("user_id:", user_id);
            ////message.data.Add("user_password:", user_password);
            //if (user != null)
            //{
            //    message.errorCode = 200;
            //    if (user.UserPassword == user_password)
            //    {
            //        message.status = true;
            //        message.data["true_user_id"] = user_id;
            //    }
            //}
            try
            {
                var user = myContext.Users
                    .Single(b => b.UserId == user_id);
                if (user != null)
                {
                    message.errorCode = 200;
                    if (user.UserPassword == user_password)
                    {
                        message.status = true;
                        message.data["user_id"] = user_id;
                        message.data["user_email"] = user.UserEmail;
                        message.data["user_phone"] = user.UserPhone;
                        message.data["user_password"] = user.UserPassword;
                        message.data["user_name"] = user.UserName;
                        message.data["user_profile"] = user.UserProfile;
                        message.data["user_createtime"] = user.UserCreatetime;
                        message.data["user_birthday"] = user.UserBirthday;
                        message.data["user_gender"] = user.UserGender;
                        message.data["user_state"] = user.UserState;
                        message.data["user_signature"] = user.UserSignature;
                        message.data["user_follower"] = user.UserFollower;
                        message.data["user_follows"] = user.UserFollows;
                        message.data["user_level"] = user.UserLevel;
                        message.data["user_coin"] = user.UserCoin;
                    }
                }
            }
            catch
            {
                message.errorCode = 500;
            }
            return message.ReturnJson();
        }
    }
}
