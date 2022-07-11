using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;

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
        private readonly ModelContext myContext;
        public UserInfoController(ModelContext modelContext)
        {
            myContext = modelContext;
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

        [HttpGet("identity")]
        public string getInfo(int user_id)
        {
            Message message = new Message();
            try
            {
                //查询有学历认证的用户
                var qualificationList = myContext.Qualifications.Where(a => a.UserId == user_id && a.Visible == true).Select(b => new { b.UserId, b.Identity, b.UniversityId }).ToList();
                message.data.Add("user_id", user_id);
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
                message.data.Add("identity_info", infos.ToArray());
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpPut("password")]
        public string changePassword(int user_id,string user_password,string new_password)
        {
            Message message = new Message();
            try
            {
                User user = myContext.Users.Single(b => b.UserId == user_id && b.UserPassword == user_password);
                user.UserPassword = new_password;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
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
        public void changeInfo()
        {

        }

        [HttpDelete]
        public void deleteInfo()
        {

        }

    }
}
