﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
namespace Back_End.Controllers
{

    public class IdentityInfo
    {
        //public int user_id { get; set; }
        public string identity { get; set; }
        public string university_name { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IConfiguration _Configuration;//读取配置文件
        public static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private readonly ModelContext myContext;
        public UserInfoController(ModelContext modelContext, IConfiguration configuration)
        {
            myContext = modelContext;
            _Configuration = configuration;
        }
        [HttpGet]
        public string getUserInfo(int user_id)
        {
            Message message = new Message();
            try
            { 
                User user = myContext.Users.Single(b => b.UserId == user_id);
                message.data["user_id"] = user_id;
                message.data["user_email"] = user.UserEmail;
                message.data["user_name"] = user.UserName;
                message.data["user_profile"] = user.UserProfile;
                message.data["user_birthday"] = user.UserBirthday;
                message.data["user_gender"] = user.UserGender;
                message.data["user_signature"] = user.UserSignature;
                message.data["user_follower"] = user.UserFollower;
                message.data["user_follows"] = user.UserFollows;
                message.data["user_level"] = user.UserLevel;
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("achieve")]
        public string getUserAchievement(int user_id)
        {
            // 成就包括：获赞次数、获得评论次数、获得收藏次数
            Message message = new();
            try
            {
                // 获赞次数
                var like_answer_times = myContext.Likeanswers
                    .Count(b => b.UserId == user_id && b.Cancel == false);
                var like_answer_comment_times = myContext.Likeanswercomments
                    .Count(b => b.UserId == user_id && b.Cancel == false);
                var like_blog_times = myContext.Likeblogs
                    .Count(b => b.UserId == user_id && b.Cancel == false);
                var like_blog_comment_times = myContext.Likeblogcomments
                    .Count(b => b.UserId == user_id && b.Cancel == false);
                int like_times = like_answer_times + like_answer_comment_times + like_blog_times + like_blog_comment_times;
                message.data.Add("like_times", like_times);

                // 获评次数
                int comment_times = 0;

                var blog_id_list = myContext.Blogs.Where(a => a.BlogUserId == user_id&&a.BlogVisible==true)
                    .Select(b => new
                    {
                        b.BlogId,
                    }).ToList();

                // 遍历所有blog
                foreach(var blog_id in blog_id_list)
                {
                    comment_times += myContext.Blogcomments.Count(b => b.BlogCommentFather != null && b.BlogCommentFather == blog_id.BlogId);
                }

                var answer_id_list = myContext.Answers.Where(a => a.AnswerUserId == user_id&& a.AnswerVisible==true)
                    .Select(b => new
                    {
                        b.AnswerId,
                    }).ToList();

                // 遍历所有answer
                foreach(var answer_id in answer_id_list)
                {
                    comment_times += myContext.Answercomments.Count(b => b.AnswerCommentFather != null && b.AnswerCommentFather == answer_id.AnswerId);
                }

                message.data.Add("comment_times", comment_times);

                // 获藏次数
                var star_question_times = myContext.Starquestions
                    .Count(b => b.UserId == user_id&&b.Cancel==false);
                var star_answer_times = myContext.Staranswers
                    .Count(b => b.UserId == user_id&&b.Cancel==false);
                var star_blog_times = myContext.Starblogs
                    .Count(b => b.UserId == user_id&&b.Cancel==false);
                int star_times = star_question_times + star_answer_times + star_blog_times;
                message.data.Add("star_times", star_times);

                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("identity")]
        public string getInfo(int user_id)
        {
            Message message = new Message();
            try
            {
                //查询有学历认证的用户
                var qualificationList = myContext.Qualifications.Where(a => a.UserId == user_id && a.Visible == true).Select(b => new { b.UserId, b.Identity, b.UniversityId }).ToList();
                message.data.Add("user_id", user_id);
                decimal coin = myContext.Users.Where(b=>b.UserId==user_id).Select(b => b.UserCoin).First();
                List<IdentityInfo> infos = new List<IdentityInfo>();
                //遍历不同identityID，将用户信息保存到identityInfo后存到infos中
                foreach (var qualification in qualificationList)
                {
                    IdentityInfo identityInfo = new IdentityInfo();
                    identityInfo.identity = qualification.Identity;
                    University university = myContext.Universities.Single(b => b.UniversityId == qualification.UniversityId);
                    identityInfo.university_name = university.UniversityChName;
                    infos.Add(identityInfo);
                }
                //以数组形式返回
                message.data.Add("coin", coin);
                message.data.Add("identity_info", infos.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpGet("questions")]
        public string getUserQuestions(int user_id)
        {
            Message message = new();
            try
            {
                var questions = myContext.Questions
                    .Where(c => c.QuestionUserId == user_id && c.Questionchecking.ReviewResult != "deleted")
                    .Select(b => new
                    {
                        b.QuestionId,
                        b.QuestionTag,
                        b.QuestionDate,
                        b.QuestionTitle,
                        b.QuestionSummary,
                        b.Questionchecking.ReviewResult,
                        b.Questionchecking.ReviewDate,
                        b.Questionchecking.ReviewReason,
                    }).ToList();
                message.data.Add("count", questions.Count);
                message.data.Add("question_list", questions.ToArray()); ;
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answers")]
        public string getUserAnswers(int user_id)
        {
            Message message = new();
            try
            {
                var answers = myContext.Answers
                    .Where(c => c.AnswerUserId == user_id && c.Answerchecking.ReviewResult != "deleted")
                    .Select(b => new
                    {
                        b.AnswerId,
                        b.Question.QuestionTitle,
                        b.AnswerUserId,
                        b.AnswerUser.UserName,
                        b.AnswerLike,
                        b.AnswerDate,
                        b.AnswerSummary,
                        b.Answerchecking.ReviewResult,
                        b.Answerchecking.ReviewDate,
                        b.Answerchecking.ReviewReason,
                    }).ToList();
                message.data.Add("count", answers.Count);
                message.data.Add("answer_list", answers.ToArray()); ;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blogs")]
        public string getUserBlogs(int user_id)
        {
            Message message = new();
            try
            {
                var blogs = myContext.Blogs
                    .Where(c => c.BlogUserId == user_id && c.Blogchecking.ReviewResult != "deleted")
                    .Select(b => new
                    {
                        b.BlogId,
                        b.BlogUserId,
                        b.BlogUser.UserName,
                        b.BlogSummary,
                        b.BlogDate,
                        b.BlogImage,
                        b.BlogTag,
                        b.Blogchecking.ReviewResult,
                        b.Blogchecking.ReviewDate,
                        b.Blogchecking.ReviewReason,
                    }).ToList();
                message.data.Add("count", blogs.Count);
                message.data.Add("blog_list", blogs.ToArray()); ;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("password")]
        public string changePassword(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string user_password = front_end_data.GetProperty("user_password").ToString();
                string new_password = front_end_data.GetProperty("new_password").ToString();
                int code = int.Parse(front_end_data.GetProperty("code").ToString());
                User user = myContext.Users.Single(b => b.UserId == user_id && b.UserPassword == user_password);
                Console.WriteLine(_cache.TryGetValue(user.UserPhone, out code));
                Console.WriteLine(_cache.Get(user.UserPhone));
                if (_cache.TryGetValue(user.UserPhone, out code))
                {
                    _cache.Remove(user.UserPhone);
                    user.UserPassword = new_password;
                    message.errorCode = 200;
                    message.status = true;
                    myContext.SaveChanges();
                }
            }
            catch
            {

            }
            return message.ReturnJson();
        }
        [HttpGet("id")]
        public string findUserID(string user_phone,string user_password)
        {
            Message message = new Message();
            try
            {
                User user = myContext.Users.Single(b => b.UserPhone == user_phone);
                if (user.UserPassword == user_password)
                {
                    message.data.Add("user_id", user.UserId);
                    message.status = true;
                }
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }
        [HttpPut("change")]
        public string changeInfo(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                // 修改信息，包括gender, phone, email, signature, user_name, profile
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string name = front_end_data.GetProperty("user_name").ToString();
                string phone = front_end_data.GetProperty("user_phone").ToString();
                string gender = front_end_data.GetProperty("user_gender").ToString();
                string email = front_end_data.GetProperty("user_email").ToString();
                string signature = front_end_data.GetProperty("user_signature").ToString();
                string birthday = front_end_data.GetProperty("user_birthday").ToString();
                

                myContext.DetachAll();
                User user = myContext.Users.Single(b => b.UserId == user_id);
                user.UserName = name;
                user.UserPhone = phone;
                user.UserGender = gender;
                user.UserEmail = email;
                user.UserSignature = signature;
                if (birthday != "")
                {
                    string[] date = birthday.Split('-');
                    int year = int.Parse(date[0]);
                    int month = int.Parse(date[1]);
                    int second = int.Parse(date[2]);
                    DateTime datetime = new(year, month, second);
                    user.UserBirthday = datetime;
                }
                myContext.SaveChanges();
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("profile")]
        public string upLoadProfile(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                myContext.DetachAll();
                User user = myContext.Users.Single(b => b.UserId == user_id);
                string img_base64 = front_end_data.GetProperty("user_profile").ToString();
                string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
                img_base64 = img_base64.Split("base64,")[1];
                byte[] img_bytes = Convert.FromBase64String(img_base64);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                string path = "user_profile/" + user_id.ToString() + type;
                client.PutObject(OssHelp.bucketName, path, stream); // 直接覆盖
                string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                user.UserProfile = imgurl;
                myContext.SaveChanges();
                message.data.Add("img_url", imgurl);
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("grow")]
        public string getGrowthInfo(int user_id)
        {
            Message message = new();
            try
            {
                User user = myContext.Users.Single(b => b.UserId == user_id);
                message.data.Add("continus", user.ContinusLogin);
                message.data.Add("level", user.UserLevel);
                message.data.Add("exp", user.UserExp);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("phone")]
        public string changePhone(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string phone = front_end_data.GetProperty("user_phone").ToString();
                int code = int.Parse(front_end_data.GetProperty("code").ToString());
                User user = myContext.Users.Single(b => b.UserId == user_id);
                if (_cache.TryGetValue(phone, out code))
                {
                    _cache.Remove(phone);
                    user.UserPhone = phone;
                    message.errorCode = 200;
                    message.status = true;
                    myContext.SaveChanges();
                }
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpPost("verifycode")]
        public string sendVerifyCode(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                string user_phone = front_end_data.GetProperty("user_phone").ToString();
                Random random = new Random();
                int code_ = random.Next(100000, 999999);
                _cache.Set(user_phone, code_, new TimeSpan(0, 0, 180));
                string key = _Configuration["MessageAccessKey"];
                string password = _Configuration["MessageAccessPassword"];
                var client = CreateClient(key, password);
                AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendReq = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
                {
                    PhoneNumbers = user_phone,
                    SignName = "候鸟留学",
                    TemplateCode = "SMS_244555845",
                    TemplateParam = "{\"code\":\"" + code_.ToString() + "\"}"
                };
                AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsResponse sendResp = client.SendSms(sendReq);
                string code = sendResp.Body.Code;
                if (code == "OK")
                {
                    message.errorCode = 200;
                    message.status = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config();
            config.AccessKeyId = accessKeyId;
            config.AccessKeySecret = accessKeySecret;
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }
    }
}
