using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Back_End.Models;
using Aliyun.OSS;
using System.Text.Json;

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

        public class NewUser
        {
            public int user_id { get; set; }
            public string user_password { get; set; }
        }

        [HttpPost]
        public string Login(dynamic new_user)
        {
            LoginMessage message = new LoginMessage();

            //JsonElement jsonElement = new();
            //jsonElement.GetProperty().ToString();
            string user_phone = new_user.GetProperty("user_phone").ToString();
            string user_password = new_user.GetProperty("user_password").ToString();
            try
            {
                myContext.DetachAll();
                var user = myContext.Users
                    .Single(b => b.UserPhone == user_phone);
                if (user != null)
                {
                    message.errorCode = 200;
                    if (user.UserPassword == user_password)
                    {
                        message.status = true;
                        message.data["user_id"] = user.UserId;
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
                        DateTime tomorrow = (DateTime)user.UserLogintime;
                        tomorrow = tomorrow.AddDays(1);
                        //判断是否为今天第一次登陆
                        //实现每日登录领取鸟币的功能
                        if (DateTime.Now.Year == tomorrow.Year && DateTime.Now.Month == tomorrow.Month && DateTime.Now.Day == tomorrow.Day)
                        {
                            user.UserCoin++;
                            int record_id = myContext.Moneychangerecords.Count() + 1;
                            Moneychangerecord moneychangerecord = new();
                            moneychangerecord.ChangeDate = DateTime.Now;
                            moneychangerecord.UserId = user.UserId;
                            moneychangerecord.RecordId = record_id;
                            moneychangerecord.ChangeNum = 1;
                            moneychangerecord.ChangeReason = "登录奖励";
                            myContext.Moneychangerecords.Add(moneychangerecord);
                        }
                        user.UserLogintime = DateTime.Now;
                        myContext.SaveChanges();
                        message.data["user_logintime"] = user.UserLogintime;
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
