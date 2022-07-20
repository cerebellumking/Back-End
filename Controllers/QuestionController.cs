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
                Question question = myContext.Questions.Single(b => b.QuestionId == question_id);
                // 获取提问者相关信息
                User user = myContext.Users.Single(b => b.UserId == question.QuestionUserId && b.UserState == true);
                string qualification;
                int university_id = -1;
                var all_qualification = myContext.Qualifications.Where(c => c.UserId == user.UserId && c.Visible == true).Select(b => new { b.Identity,b.UniversityId });
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
                    message.data.Add("question_tag", question.QuestionTag);
                    message.data.Add("question_date", question.QuestionDate);
                    message.data.Add("question_title", question.QuestionTitle);
                    message.data.Add("question_description", question.QuestionDescription);
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
                var answers = myContext.Answers.Where(c => c.AnswerVisible == true && c.QuestionId == question_id).OrderByDescending(a => a.AnswerDate).Select(b => new
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
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("related")]
        public string getRelatedQuestions(int question_id)
        {
            Message message = new();
            try
            {
                string tag = myContext.Questions.Single(c => c.QuestionId == question_id).QuestionTag;
                var questions = myContext.Questions.Where(c => c.QuestionTag == tag && c.QuestionId != question_id).OrderByDescending(a => a.QuestionDate).Select(b => new
                {
                    b.QuestionId,
                    b.QuestionUserId,
                    b.QuestionDate,
                    b.QuestionTitle,
                    //b.QuestionDescription,
                    b.QuestionSummary
                }).ToList();
                message.errorCode = 200;
                message.status = true;
                message.data.Add("tag", tag);
                message.data.Add("count", questions.Count);
                message.data.Add("related_questions", questions.ToArray());
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("time")]
        public string showQuestionByTime()
        {
            Message message = new Message();
            try
            {
                var question = myContext.Questions.Where(c => c.QuestionVisible == true).OrderByDescending(a => a.QuestionDate).Select(b => new
                {
                    b.Answers.Count,
                    b.QuestionId,
                    b.QuestionUserId,
                    b.QuestionTitle,
                    b.QuestionApply,
                    b.QuestionReward,
                    b.QuestionDate,
                    b.QuestionDescription,
                    b.QuestionTag
                }).ToList();
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
                var question = myContext.Questions.Where(c => c.QuestionVisible == true).OrderByDescending(a => a.Answers.Count).Select(b => new
                {
                    b.Answers.Count,
                    b.QuestionId,
                    b.QuestionUserId,
                    b.QuestionTitle,
                    b.QuestionApply,
                    b.QuestionReward,
                    b.QuestionDate,
                    b.QuestionDescription,
                    b.QuestionTag
                }).ToList();
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
        [HttpPost]
        public string raiseQuestion(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int question_user_id = int.Parse(front_end_data.GetProperty("question_user_id").ToString());
                string question_tag = front_end_data.GetProperty("question_tag").ToString();
                string question_title = front_end_data.GetProperty("question_title").ToString();
                string question_description = front_end_data.GetProperty("question_description").ToString();
                decimal question_reward = decimal.Parse(front_end_data.GetProperty("question_tag").ToString());

                myContext.DetachAll();
                Question question = new Question();
                var count = myContext.Questions.Count();
                int id;
                if (count == 0)
                    id = 1;
                else
                {
                    id = myContext.Questions.Select(b => b.QuestionId).Max() + 1;
                }
                question.QuestionId = id;
                question.QuestionUserId = question_user_id;
                question.QuestionUser = myContext.Users.Single(b => b.UserId == question_user_id);
                question.QuestionTag = question_tag;
                question.QuestionTitle = question_title;
                question.QuestionDescription = question_description;
                question.QuestionReward = question_reward;
                question.QuestionApply = 0;
                question.QuestionVisible = false; // 未通过审核，不可见——lc改
                question.QuestionDate = DateTime.Now;
                question.QuestionSummary = question_description.Substring(0, question_description.Length > 50 ? 50 : question_description.Length);
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
    }
}
