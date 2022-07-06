using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Back_End.Models;
using System.Net.Http;
using Aliyun.OSS;

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

        [HttpPost("image")]
        public string PushImage()
        {
            Message m = new Message();
            var files = Request.Form.Files;
            var text = files[0].OpenReadStream();
            const string accessKeyId = "LTAI5tNHm9vkUTD9WshKKhvQ";
            const string accessKeySecret = "wwaOqFeNa3iwkETmcIdYYCkweyAhAx";
            const string endpoint = "http://oss-cn-shanghai.aliyuncs.com";
            const string bucketName = "houniaoliuxue";
            var filebyte = StreamHelp.StreamToBytes(text);
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
            client.PutObject(bucketName, files[0].FileName, stream);
            m.data.Add("imageurl", "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/"+ files[0].FileName);

            return m.ReturnJson();
        }


    }
}
