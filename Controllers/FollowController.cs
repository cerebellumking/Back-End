using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
namespace Back_End.Controllers
{
    public class FollowUserInformation
    {
        public int user_id { get; set; }
        public string user_profile { get; set; }
        public string user_name { get; set; }
        public string user_signature { get; set; }
        public decimal user_level { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly ModelContext myContext;
        public FollowController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost]
        public string followUser(int user_id, int follow_user_id)
        {
            FollowMessage message = new FollowMessage();
            User user = new User();
            try
            {
                myContext.DetachAll();
                int max_id = myContext.Users.Select(b => b.UserId).Max();
                Followuser followuser = new Followuser();
                if (!myContext.Followusers.Any(b => b.FollowUserId == follow_user_id && b.UserId == user_id && b.Cancel == false) && user_id != follow_user_id && user_id <= max_id && follow_user_id <= max_id)
                {
                    /*对User进行修改*/
                    user = myContext.Users.Single(b => b.UserId == user_id);
                    user.UserFollows++;
                    User follow_user = myContext.Users.Single(b => b.UserId == follow_user_id);
                    follow_user.UserFollower++;
                    /*判断该关注是否取消过*/
                    if (!myContext.Followusers.Any(b => b.FollowUserId == follow_user_id && b.UserId == user_id && b.Cancel == true))
                    {
                        followuser.FollowUserId = follow_user_id;
                        followuser.UserId = user_id;
                        followuser.User = user;
                        followuser.FollowUser = follow_user;
                        followuser.FollowTime = DateTime.Now;
                        myContext.Followusers.Add(followuser);
                    }
                    else
                    {
                        followuser = myContext.Followusers.Single(b => b.UserId == user_id && b.FollowUserId == follow_user_id);
                        followuser.Cancel = false;
                    }
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
        [HttpPost("university")]
        public string followUniversity(int user_id,int university_id)
        {
            FollowMessage message = new FollowMessage();
            Followuniversity follow = new Followuniversity();
            int maxuser = myContext.Users.Count();
            int maxuniversity = myContext.Universities.Count();
            try
            {
                if (!myContext.Followuniversities.Any(b => b.UserId == user_id && b.UniversityId == university_id && b.Cancel == false) && user_id <= maxuser && university_id <= maxuniversity)
                {
                    /*判断该关注是否取消过*/
                    if (!myContext.Followuniversities.Any(b => b.UniversityId == university_id && b.UserId == user_id && b.Cancel == true))
                    {
                        follow.UniversityId= university_id;
                        follow.UserId = user_id;
                        follow.User = myContext.Users.Single(b=>b.UserId==user_id);
                        follow.University = myContext.Universities.Single(b=>b.UniversityId==university_id);
                        follow.FollowTime = DateTime.Now;
                        myContext.Followuniversities.Add(follow);
                    }
                    else
                    {
                        follow = myContext.Followuniversities.Single(b => b.UserId == user_id && b.UniversityId == university_id);
                        follow.Cancel = false;
                    }
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
        [HttpPut]
        public string cancelFollowUser(int user_id, int follow_user_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                myContext.DetachAll();
                Followuser followuser = myContext.Followusers.Single(b => b.UserId == user_id && b.FollowUserId == follow_user_id && b.Cancel == false);
                User follow_user = myContext.Users.Single(b => b.UserId == follow_user_id);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                user.UserFollows--;
                follow_user.UserFollower--;
                followuser.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch
            {
            }
            return message.ReturnJson();
        }
        [HttpPut("university")]
        public string cancelFollowUniversity(int user_id,int university_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                myContext.DetachAll();
                Followuniversity follow = myContext.Followuniversities.Single(b => b.UserId == user_id && b.UniversityId == university_id && b.Cancel == false);
                University follow_university = myContext.Universities.Single(b => b.UniversityId == university_id);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                follow.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch
            {
            }
            return message.ReturnJson();
        }
        [HttpGet]
        public string whetherFollowUser(int user_id, int follow_user_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                bool flag = myContext.Followusers.Any(b => b.UserId == user_id && b.FollowUserId == follow_user_id && b.Cancel == false);
                message.errorCode = 200;
                message.status = flag;
            }
            catch
            {
            }

            return message.ReturnJson();
        }

        [HttpGet("university")]
        public string whetherFollowUniversity(int user_id,int university_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                bool flag = myContext.Followuniversities.Any(b => b.UserId == user_id && b.UniversityId == university_id && b.Cancel == false);
                message.errorCode = 200;
                message.status = flag;
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpGet("userlist")]
        public string getFollowUserList(int user_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                var list = myContext.Followusers.Where(a => a.UserId == user_id && a.Cancel == false).Select(b =>new { b.FollowUserId}).ToList();
                List < FollowUserInformation > followUserList= new List<FollowUserInformation>();
                foreach(var val in list)
                {
                    User user = myContext.Users.Single(b => b.UserId == val.FollowUserId);
                    FollowUserInformation follow = new FollowUserInformation();
                    follow.user_id = user.UserId;
                    follow.user_level = user.UserLevel;
                    follow.user_name = user.UserName;
                    follow.user_profile = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + "user_profile/" + user.UserId.ToString() + ".jpg";
                    follow.user_signature = user.UserSignature;
                    followUserList.Add(follow);
                }
                message.data.Add("follows", followUserList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch
            {

            }
            return message.ReturnJson();
        }
    }
}
