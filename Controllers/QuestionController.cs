using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using System.Text;
using System.IO;
using Back_End;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ModelContext myContext;
        public QuestionController(ModelContext modelContext)
        {
            myContext = modelContext;
        }
        [HttpGet]
        public string getQuestionInfo(int question_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                var question = myContext.Questions
                    .Where(b => b.QuestionId == question_id)
                    .Select(b => new
                    {
                        b.QuestionVisible,
                        b.QuestionUserId,
                        b.QuestionTag,
                        b.QuestionDate,
                        b.QuestionTitle,
                        b.QuestionSummary,
                        b.QuestionDescription,
                        b.QuestionReward,
                        b.QuestionApply,
                        b.QuestionImage,
                    }).ToList().First();
                // 获取提问者相关信息
                User user = myContext.Users.Single(b => b.UserId == question.QuestionUserId && b.UserState == true);
                string qualification;
                int university_id = -1;
                var all_qualification = myContext.Qualifications
                    .Where(c => c.UserId == user.UserId && c.Visible == true)
                    .Select(b => new { b.Identity,b.UniversityId });
                if (all_qualification.Any(b => b.Identity == "博士"))
                {
                    qualification = "博士";
                    university_id = all_qualification.Single(b => b.Identity == "博士").UniversityId;
                }
                else if(all_qualification.Any(b => b.Identity == "硕士"))
                {
                    qualification = "硕士";
                    university_id = all_qualification.Single(b => b.Identity == "硕士").UniversityId;
                }
                else if(all_qualification.Any(b => b.Identity == "本科"))
                {
                    qualification = "本科";
                    university_id = all_qualification.Single(b => b.Identity == "本科").UniversityId;
                }
                else
                {
                    // 未进行学历认证
                    qualification = "null";
                    university_id = -1;
                }
                //var university_info = myContext.Universities.Single(b => b.UniversityId == university_id);
                
                    message.data.Add("question_id", question_id);
                if (question.QuestionVisible==true)
                {
                    message.data.Add("user_id", question.QuestionUserId);
                    message.data.Add("user_name", user.UserName);
                    message.data.Add("user_profile", user.UserProfile);
                    message.data.Add("user_qualification", qualification);
                    if (university_id != -1)
                    {
                        var university_info = myContext.Universities.Single(b => b.UniversityId == university_id);
                        message.data.Add("user_university", university_info.UniversityChName);
                        message.data.Add("university_country", university_info.UniversityCountry);
                    }
                    else
                    {
                        // 没有认证学校
                        message.data.Add("user_university", "null");
                        message.data.Add("university_country", "null");
                    }
                    message.data.Add("question_tag", question.QuestionTag.Split("-"));
                    message.data.Add("question_date", question.QuestionDate);
                    message.data.Add("question_title", question.QuestionTitle);
                    message.data.Add("question_description", question.QuestionDescription);
                    message.data.Add("question_summary", question.QuestionSummary);
                    message.data.Add("question_reward", question.QuestionReward);
                    message.data.Add("question_apply", question.QuestionApply);
                    message.data.Add("question_image", question.QuestionImage);
                    message.status = true;
                }
                else
                {
                    //message.data.Add("question_visible", question.QuestionVisible);
                    message.status = false;
                }
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answers")]
        public string getAnswersByQuestion(int question_id)
        {
            Message message = new();
            try
            {
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                var answers = myContext.Answers.Where(c => c.AnswerVisible == true && c.QuestionId == question_id)
                    .OrderByDescending(a => a.AnswerDate)
                    .Select(b => new
                {
                    b.AnswerId,
                    b.AnswerUserId,
                    b.AnswerUser.UserName,
                    b.AnswerUser.UserProfile,
                    b.QuestionId,
                    b.AnswerDate,
                    //b.AnswerContent,
                    //b.AnswerContentpic,
                    b.AnswerLike,
                    b.AnswerCoin,
                    b.AnswerSummary
                }).ToList();
                message.errorCode = 200;
                message.status = true;
                message.data.Add("count", answers.Count);
                message.data.Add("answers", answers.ToArray());
                message.data.Add("apply",question.QuestionApply);
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        public class RelatedQuestionInfo
        {
            public int QuestionId { get; set; }
            public int? QuestionUserId { get; set; }
            public DateTime QuestionDate { get; set; }
            public string[] QuestionTags { get; set; }
            public string QuestionImage { get; set; }
            public string QuestionTitle { get; set; }
            public string QuestionSummary { get; set; }
        }

        [HttpGet("related")]
        public string getRelatedQuestions(int question_id)
        {
            Message message = new();
            try
            {
                string[] tags = myContext.Questions
                    .Where(c => c.QuestionId == question_id && c.QuestionVisible == true)
                    .Select(b => new
                    {
                        b.QuestionTag
                    }).ToList().First().QuestionTag.Split("-");
                List<int> questions_id = new();
                questions_id.Add(question_id);
                List<RelatedQuestionInfo> list_questions = new();
                foreach(var tag in tags)
                {
                    var questions = myContext.Questions
                        .Where(c => c.QuestionTag.Contains(tag) && !questions_id.Contains(c.QuestionId))
                        .OrderByDescending(a => a.QuestionDate).ToList();
                    foreach(var question in questions)
                    {
                        RelatedQuestionInfo new_question_info = new();
                        new_question_info.QuestionId = question.QuestionId;
                        new_question_info.QuestionUserId = question.QuestionUserId;
                        new_question_info.QuestionDate = question.QuestionDate;
                        new_question_info.QuestionTags = question.QuestionTag.Split("-");
                        new_question_info.QuestionImage = question.QuestionImage;
                        new_question_info.QuestionTitle = question.QuestionTitle;
                        new_question_info.QuestionSummary = question.QuestionSummary;
                        list_questions.Add(new_question_info);
                        questions_id.Add(question.QuestionId);
                    }
                }
                message.errorCode = 200;
                message.status = true;
                message.data.Add("tag", tags.ToArray());
                message.data.Add("count", list_questions.Count);
                message.data.Add("related_questions", list_questions.ToArray());
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
        public class QuestionInfo
        {
            public int Count { get; set; }
            public int QuestionId { get; set; }
            //public int? QuestionUserId { get; set; }
            public int QuestionApply { get; set; }
            public decimal QuestionReward { get; set; }
            public DateTime QuestionDate { get; set; }
            //public string[] QuestionTag { get; set; }
            public string QuestionTitle { get; set; }
            //public string Questiondescription { get; set; }
            public double distance { get; set; }
        }

        [HttpGet("time")]
        public string showQuestionByTime()
        {
            Message message = new Message();
            try
            {
                //var question = myContext.Questions.Where(c => c.QuestionVisible == true)
                //    .OrderByDescending(a => a.QuestionDate)
                //    .Select(b => new
                //    {
                //        b.Answers.Count,
                //        b.QuestionId,
                //        b.QuestionUserId,
                //        b.QuestionTitle,
                //        b.QuestionApply,
                //        b.QuestionReward,
                //        b.QuestionDate,
                //        b.QuestionDescription,
                //        b.QuestionTag
                //    }).ToList();
                var question = myContext.Questions.Where(c => c.QuestionVisible == true)
                    .OrderByDescending(a => a.QuestionDate)
                    .Select(b => new QuestionInfo
                    {
                        Count = myContext.Answers.Count(c => c.QuestionId == b.QuestionId && c.AnswerVisible == true),
                        QuestionId = b.QuestionId,
                        //QuestionUserId = b.QuestionUserId,
                        QuestionTitle = b.QuestionTitle,
                        QuestionApply = (int)b.QuestionApply,
                        QuestionReward = (decimal)b.QuestionReward,
                        QuestionDate = b.QuestionDate,
                        //Questiondescription = b.QuestionDescription
                    }).ToList();
                //foreach (var val in question)
                //{
                //    Question question1 = myContext.Questions.Single(b => b.QuestionId == val.QuestionId);
                //    val.QuestionTag = question1.QuestionTag.Split('-');
                //    val.Count = myContext.Answers.Count(c => c.QuestionId == val.QuestionId && c.AnswerVisible == true);
                //}
                //if (question.Count > 2)
                //    question.RemoveRange(2, question.Count - 2);
                message.errorCode = 200;
                message.status = true;
                message.data.Add("question", question.ToArray());
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("heat")]
        public string showQuestionByHeat()
        {
            Message message = new Message();
            try
            {
                //var question = myContext.Questions.Where(c => c.QuestionVisible == true)
                //    .OrderByDescending(a => a.Answers.Count(b => b.AnswerVisible == true))
                //    .Select(b => new
                //    {
                //        b.Answers.Count,
                //        b.QuestionId,
                //        b.QuestionUserId,
                //        b.QuestionTitle,
                //        b.QuestionApply,
                //        b.QuestionReward,
                //        b.QuestionDate,
                //        b.QuestionDescription,
                //        b.QuestionTag
                //    }).ToList();
                var question = myContext.Questions.Where(c => c.QuestionVisible == true)
                    .OrderByDescending(a => a.Answers.Count(b => b.AnswerVisible == true))
                    .Select(b => new QuestionInfo
                    {
                        Count = myContext.Answers.Count(c => c.QuestionId == b.QuestionId && c.AnswerVisible == true),
                        QuestionId = b.QuestionId,
                        //QuestionUserId = b.QuestionUserId,
                        QuestionTitle = b.QuestionTitle,
                        QuestionApply = (int)b.QuestionApply,
                        QuestionReward = (decimal)b.QuestionReward,
                        QuestionDate = b.QuestionDate,
                        //Questiondescription = b.QuestionDescription
                    }).ToList();
                //foreach (var val in question)
                //{
                //    Question question1 = myContext.Questions.Single(b => b.QuestionId == val.QuestionId);
                //    val.QuestionTag = question1.QuestionTag.Split('-');
                //    val.Count = myContext.Answers.Count(c => c.QuestionId == val.QuestionId && c.AnswerVisible == true);
                //}
                //if (question.Count > 2)
                //    question.RemoveRange(2, question.Count - 2);
                message.errorCode = 200;
                message.status = true;
                message.data.Add("question", question.ToArray());
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("search")]
        public string searchQuestionByTitle(string target)
        {
            Message message = new Message();
            try
            {
                target = System.Web.HttpUtility.UrlDecode(target);
                var question = myContext.Questions.Where(c => c.QuestionVisible == true)
                    .Select(b => new QuestionInfo
                    {
                        Count = myContext.Answers.Count(c => c.QuestionId == b.QuestionId && c.AnswerVisible == true),
                        QuestionId = b.QuestionId,
                        QuestionTitle = b.QuestionTitle,
                        QuestionApply = (int)b.QuestionApply,
                        QuestionReward = (decimal)b.QuestionReward,
                        QuestionDate = b.QuestionDate,
                    }).ToList();
                foreach (var val in question)
                {
                    val.distance = (double)SimilarityTool.LevenshteinDistancePercent(val.QuestionTitle, target);
                }
                question.OrderByDescending(b => b.distance);
                //for (int i = 0; i < question.Count; i++)
                //{
                //    if ((double)SimilarityTool.LevenshteinDistancePercent(question[i].QuestionTitle, target) < 0.3)
                //    {
                //        question.Remove(question[i]);
                //    }
                //}
                message.errorCode = 200;
                message.status = true;
                message.data.Add("question", question.ToArray());
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPost]
        public string raiseQuestion(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int question_user_id = int.Parse(front_end_data.GetProperty("question_user_id").ToString());
                string question_title = front_end_data.GetProperty("question_title").ToString();
                string question_tag = front_end_data.GetProperty("question_tag").ToString();
                string question_summary = front_end_data.GetProperty("question_summary").ToString();
                string question_description = front_end_data.GetProperty("question_description").ToString();
                decimal question_reward = decimal.Parse(front_end_data.GetProperty("question_reward").ToString());

                Question question = new Question();
                byte[] img_bytes = Encoding.UTF8.GetBytes(question_description);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                int id = myContext.Questions.Count() + 1;
                string path = "question/content/" + id.ToString() + ".html";
                string imageurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                client.PutObject(OssHelp.bucketName, path, stream);

                question.QuestionId = id;
                question.QuestionUserId = question_user_id;
                question.QuestionTag = question_tag;
                question.QuestionDate = DateTime.Now;
                question.QuestionTitle = question_title;
                question.QuestionDescription = imageurl;
                question.QuestionReward = question_reward;
                question.QuestionApply = 0;
                question.QuestionVisible = true; // 为调试方便，先设定为true，后改为false
                question.QuestionSummary = question_summary;
                // question不存第一张图，因为不一定有图

                Questionchecking questionchecking = new Questionchecking();
                questionchecking.AdministratorId = 0; // 零表示未审核——lc改
                questionchecking.QuestionId = id;
                questionchecking.ReviewResult = "待审核";
                questionchecking.QuestionDate = question.QuestionDate;
                myContext.Questioncheckings.Add(questionchecking);
                myContext.Questions.Add(question);
                myContext.SaveChanges();
                message.data.Add("question_id", id);
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("apply")]
        public string applyAnswer(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int question_id = int.Parse(front_end_data.GetProperty("question_id").ToString());
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                if (question.QuestionApply != 0)
                {
                    message.errorCode = 200;
                    return message.ReturnJson();
                }
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                User user = myContext.Users.Single(b => b.UserId == answer.AnswerUserId);
                user.UserCoin +=(decimal)question.QuestionReward;
                question.QuestionApply = answer_id;
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpDelete]
        public string deleteQuestion(int question_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                question.QuestionVisible = false;
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
