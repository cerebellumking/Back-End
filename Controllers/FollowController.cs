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
        [HttpPut]
        public string cancelFollow(int user_id, int follow_user_id)
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

        [HttpGet]
        public string whetherFollow(int user_id, int follow_user_id)
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
    }
}
