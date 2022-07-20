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

                var blog_id_list = myContext.Blogs.Where(a => a.BlogUserId == user_id)
                    .Select(b => new
                    {
                        b.BlogId,
                    }).ToList();

                // 遍历所有blog
                foreach(var blog_id in blog_id_list)
                {
                    comment_times += myContext.Blogcomments.Count(b => b.BlogCommentFather != null && b.BlogCommentFather == blog_id.BlogId);
                }

                var answer_id_list = myContext.Answers.Where(a => a.AnswerUserId == user_id)
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
                    .Count(b => b.UserId == user_id);
                var star_answer_times = myContext.Staranswers
                    .Count(b => b.UserId == user_id);
                var star_blog_times = myContext.Starblogs
                    .Count(b => b.UserId == user_id);
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

        [HttpGet("questions")]
        public string getUserQuestions(int user_id)
        {
            Message message = new();
            try
            {
                var questions = myContext.Questions
                    .Where(c => c.QuestionUserId == user_id && c.QuestionVisible == true)
                    .Select(b => new
                    {
                        b.QuestionId,
                        b.QuestionTag,
                        b.QuestionDate,
                        b.QuestionTitle,
                        b.QuestionSummary,
                    }).ToList();
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
                    .Where(c => c.AnswerUserId == user_id && c.AnswerVisible == true)
                    .Select(b => new
                    {
                        b.AnswerId,
                        b.AnswerLike,
                        b.AnswerDate,
                        b.AnswerSummary,
                    }).ToList();
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
                    .Where(c => c.BlogUserId == user_id && c.BlogVisible == true)
                    .Select(b => new
                    {
                        b.BlogId,
                        b.BlogSummary,
                        b.BlogDate,
                    }).ToList();
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
