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

    public class FollowUniversityInformation
    {
        public int university_id { get; set; }
        public string university_badge { get; set; }
        public string university_chname { get; set; }
        public string university_enname { get; set; }
        public string university_region { get; set; }
        public string university_country { get; set; }
    }

    public class FollowInstitutionInformation
    {
        public int institution_id { get; set; }
        public string institution_name { get; set; }
        public string institution_profile { get; set; }
        public string institution_province { get; set; }
        public string institution_city { get; set; }
        public string institution_target { get; set; }
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
        public string followUser(dynamic front_end_data)
        {

            FollowMessage message = new FollowMessage();
            User user = new User();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int follow_user_id = int.Parse(front_end_data.GetProperty("follow_user_id").ToString());
                myContext.DetachAll();
                /*对User进行修改*/
                user = myContext.Users.Single(b => b.UserId == user_id);
                user.UserFollows++;
                User follow_user = myContext.Users.Single(b => b.UserId == follow_user_id);
                follow_user.UserFollower++;
                object[] pk = { follow_user_id, user_id};
                Followuser old_followuser = myContext.Followusers.Find(pk);
                /*判断该关注是否取消过*/
                if (old_followuser == null)
                {
                    Followuser followuser = new Followuser();
                    followuser.FollowUserId = follow_user_id;
                    followuser.UserId = user_id;
                    followuser.User = user;
                    followuser.FollowUser = follow_user;
                    followuser.FollowTime = DateTime.Now;
                    myContext.Followusers.Add(followuser);
                }
                else
                {
                    old_followuser.FollowTime = DateTime.Now;
                    old_followuser.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPost("university")]
        public string followUniversity(dynamic front_end_data)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int university_id = int.Parse(front_end_data.GetProperty("university_id").ToString());
                object[] pk = {  university_id, user_id };
                Followuniversity old_follow = myContext.Followuniversities.Find(pk);
                /*判断该关注是否取消过*/
                if (old_follow == null)
                {
                    Followuniversity follow = new Followuniversity();
                    follow.UniversityId = university_id;
                    follow.UserId = user_id;
                    follow.User = myContext.Users.Single(b => b.UserId == user_id);
                    follow.University = myContext.Universities.Single(b => b.UniversityId == university_id);
                    follow.FollowTime = DateTime.Now;
                    myContext.Followuniversities.Add(follow);
                }
                else
                {
                    old_follow.FollowTime = DateTime.Now;
                    old_follow.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();


            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("institution")]
        public string followInstitution(dynamic front_end_data)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int institution_id = int.Parse(front_end_data.GetProperty("institution_id").ToString());
                object[] pk = { institution_id, user_id };
                Followinstitution old_follow = myContext.Followinstitutions.Find(pk);
                /*判断该关注是否取消过*/
                if (old_follow == null)
                {
                    Followinstitution follow = new Followinstitution();
                    follow.InstitutionId = institution_id;
                    follow.UserId = user_id;
                    follow.User = myContext.Users.Single(b => b.UserId == user_id);
                    follow.Institution = myContext.Institutions.Single(b => b.InstitutionId == institution_id);
                    follow.FollowTime = DateTime.Now;
                    myContext.Followinstitutions.Add(follow);
                }
                else
                {
                    old_follow.FollowTime = DateTime.Now;
                    old_follow.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPut]
        public string cancelFollowUser(dynamic front_end_data)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int follow_user_id = int.Parse(front_end_data.GetProperty("follow_user_id").ToString());
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
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPut("university")]
        public string cancelFollowUniversity(dynamic front_end_data)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int university_id = int.Parse(front_end_data.GetProperty("university_id").ToString());
                myContext.DetachAll();
                Followuniversity follow = myContext.Followuniversities.Single(b => b.UserId == user_id && b.UniversityId == university_id && b.Cancel == false);
                University follow_university = myContext.Universities.Single(b => b.UniversityId == university_id);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                follow.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPut("institution")]
        public string cancelFollowInstitution(dynamic front_end_data)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int institution_id = int.Parse(front_end_data.GetProperty("institution_id").ToString());
                myContext.DetachAll();
                Followinstitution follow = myContext.Followinstitutions.Single(b => b.UserId == user_id && b.InstitutionId == institution_id && b.Cancel == false);
                Institution follow_institution = myContext.Institutions.Single(b => b.InstitutionId == institution_id);
                User user = myContext.Users.Single(b => b.UserId == user_id);
                follow.Cancel = true;
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
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
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("university")]
        public string whetherFollowUniversity(int user_id, int university_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                bool flag = myContext.Followuniversities.Any(b => b.UserId == user_id && b.UniversityId == university_id && b.Cancel == false);
                message.errorCode = 200;
                message.status = flag;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("institution")]
        public string whetherFollowInstitution(int user_id, int institution_id)
        {
            FollowMessage message = new FollowMessage();
            try
            {
                bool flag = myContext.Followinstitutions.Any(b => b.UserId == user_id && b.InstitutionId == institution_id && b.Cancel == false);
                message.errorCode = 200;
                message.status = flag;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("follows")]
        public string getFollowUserList(int user_id)
        {
            // 获取用户的关注列表
            FollowMessage message = new FollowMessage();
            try
            {
                var list = myContext.Followusers.Where(a => a.UserId == user_id && a.Cancel == false).Select(b => new { b.FollowUserId }).ToList();
                List<FollowUserInformation> followUserList = new List<FollowUserInformation>();
                foreach (var val in list)
                {
                    User user = myContext.Users.Single(b => b.UserId == val.FollowUserId);
                    FollowUserInformation follow = new FollowUserInformation();
                    follow.user_id = user.UserId;
                    follow.user_level = user.UserLevel;
                    follow.user_name = user.UserName;
                    follow.user_profile = user.UserProfile;
                    //follow.user_profile = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + "user_profile/" + user.UserId.ToString() + ".jpg";
                    follow.user_signature = user.UserSignature;
                    followUserList.Add(follow);
                }
                message.data.Add("follows", followUserList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("follower")]
        public string getUserFollowList(int user_id)
        {
            // 获取用户的粉丝列表
            Message message = new();
            try
            {
                var list = myContext.Followusers.Where(a => a.FollowUserId == user_id && a.Cancel == false).Select(b => new { b.UserId }).ToList();
                List<FollowUserInformation> followUserList = new();
                foreach(var val in list)
                {
                    User user = myContext.Users.Single(b => b.UserId == val.UserId);
                    FollowUserInformation follow = new FollowUserInformation();
                    follow.user_id = user.UserId;
                    follow.user_level = user.UserLevel;
                    follow.user_name = user.UserName;
                    follow.user_profile = user.UserProfile;
                    follow.user_signature = user.UserSignature;
                    followUserList.Add(follow);
                }
                message.data.Add("follows", followUserList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("universities")]
        public string getUniversityFollowList(int user_id)
        {
            // 获取关注的高校列表
            Message message = new();
            try
            {
                var list = myContext.Followuniversities.Where(a => a.UserId == user_id && a.Cancel == false).Select(b => new { b.UniversityId }).ToList();
                List<FollowUniversityInformation> followUniversityList = new();
                foreach(var val in list)
                {
                    University university = myContext.Universities.Single(b => b.UniversityId == val.UniversityId);
                    FollowUniversityInformation follow = new();
                    follow.university_id = university.UniversityId;
                    follow.university_badge = university.UniversityBadge;
                    follow.university_enname = university.UniversityEnName;
                    follow.university_chname = university.UniversityChName;
                    follow.university_country = university.UniversityCountry;
                    follow.university_region = university.UniversityRegion;
                    followUniversityList.Add(follow);
                }
                message.data.Add("follows", followUniversityList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("institutions")]
        public string getInstitutionFollowList(int user_id)
        {
            // 获取关注的机构列表
            Message message = new();
            try
            {
                var list = myContext.Followinstitutions.Where(a => a.UserId == user_id && a.Cancel == false).Select(b => new { b.InstitutionId }).ToList();
                List<FollowInstitutionInformation> followInstitutionList = new();
                foreach (var val in list)
                {
                    Institution institution = myContext.Institutions.Single(b => b.InstitutionId == val.InstitutionId);
                    FollowInstitutionInformation follow = new();
                    follow.institution_id = institution.InstitutionId;
                    follow.institution_profile = institution.InstitutionProfile;
                    follow.institution_name = institution.InstitutionName;
                    follow.institution_province = institution.InstitutionProvince;
                    follow.institution_city = institution.InstitutionCity;
                    follow.institution_target = institution.InstitutionTarget;
                    followInstitutionList.Add(follow);
                }
                message.data.Add("follows", followInstitutionList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
