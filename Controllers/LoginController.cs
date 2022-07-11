using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Back_End.Models;
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
        public string Login(int user_id, string user_password)
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
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("image")]
        public string PushImage(int user_id)
        {
            Message m = new Message();
            try
            {
                var files = Request.Form.Files;
                var text = files[0].OpenReadStream();

                /*响应速度相对比较快*/
                var filebyte = StreamHelp.StreamToBytes(text);
                var client = OssHelp.createClient();
                var type = files[0].FileName.Substring(files[0].FileName.LastIndexOf('.'));
                MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
                string path = "user_profile/" + user_id.ToString() + type;
                client.PutObject(OssHelp.bucketName, path, stream);
                string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                m.data.Add("imageurl", imgurl);

                /*响应速度不行*/
                //var type = files[0].FileName.Substring(files[0].FileName.LastIndexOf('.'));
                //string path = "user_profile/" + user_id.ToString() + type;
                //string imgurl = OssHelp.uploadImage(text, path);
                //m.data.Add("imageurl", imgurl);


                User user = myContext.Users.Single(b => b.UserId == user_id);
                user.UserProfile = imgurl;
                myContext.SaveChanges();
                m.errorCode = 200;
                m.status = true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            return m.ReturnJson();
        }


    }
}
