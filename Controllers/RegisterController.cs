using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _Configuration;//读取配置文件
        public static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private readonly ModelContext myContext;
        public RegisterController(ModelContext modelContext, IConfiguration configuration)
        {
            myContext = modelContext;
            _Configuration = configuration;
        }

        [HttpPost]
        public string Register(dynamic front_end_data)
        {
            RegisterMessage message = new RegisterMessage();
            try
            {
                string user_phone = front_end_data.GetProperty("user_phone").ToString();
                string user_password = front_end_data.GetProperty("user_password").ToString();
                int code = int.Parse(front_end_data.GetProperty("code").ToString());
                if (_cache.TryGetValue(user_phone, out code))
                {
                    _cache.Remove(user_phone);
                    myContext.DetachAll();
                    User user = new User();
                    user.UserGender = "m";
                    user.UserName = "用户" + user_phone;
                    user.UserPhone = user_phone;
                    user.UserPassword = user_password;
                    user.UserCreatetime = DateTime.Now;
                    var count = myContext.Users.Count();
                    int id = myContext.Users.Count() + 1;
                    user.UserId = id;
                    myContext.Users.Add(user);
                    myContext.SaveChanges();
                    message.status = true;
                    message.errorCode = 200;
                    message.data["user_id"] = id;
                }
                else
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = "验证码有误或验证超时";
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
