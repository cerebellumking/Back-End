﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Back_End.Models;
using System.IO;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly ModelContext myContext;
        public AnswerController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getAnswerInfo(int answer_id)
        {
            Message message = new Message();
            try
            {
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                if (answer.AnswerVisible == true)
                {
                    Question question = myContext.Questions.Single(b => b.QuestionId == answer.QuestionId);
                    bool flag = answer_id == question.QuestionApply;
                    message.data.Add("answer_id", answer_id);
                    message.data.Add("answer_user_id", answer.AnswerUserId);
                    message.data.Add("question_id", answer.QuestionId);
                    message.data.Add("tag", question.QuestionTag);
                    message.data.Add("answer_date", answer.AnswerDate);
                    message.data.Add("answer_content", answer.AnswerContent);
                    message.data.Add("answer_contentpic", answer.AnswerContentpic);
                    message.data.Add("answer_coin", answer.AnswerCoin);
                    message.data.Add("answer_like", answer.AnswerLike);
                    message.data.Add("apply_flag", flag);
                }
                else
                {
                    message.data.Add("answer_visible", answer.AnswerVisible);
                }
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("comment")]
        public string getAnswerComment(int answer_id)
        {
            Message message = new Message();
            try
            {
                var answercomment = myContext.Answercomments.Where(b => b.AnswerCommentFather == answer_id&&b.AnswerCommentVisible==true);
                message.data["comment_num"] = answercomment.Count();
                var list = answercomment
                    .Select(b => new { b.AnswerCommentUser.UserId, b.AnswerCommentId, b.AnswerCommentUser.UserName, b.AnswerCommentUser.UserProfile, b.AnswerCommentContent, b.AnswerCommentLike,b.InverseAnswerCommentReplyNavigation.Count,})
                    .ToList();
                message.data["comment_list"] = list.ToArray();
                message.status = true;
                message.errorCode = 200;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return message.ReturnJson();
        }

        [HttpGet("reply")]
        public string getAnswerCommentReply(int answer_comment_id)
        {
            Message message = new Message();
            try
            {
                var answercomment = myContext.Answercomments.Where(b => b.AnswerCommentReply == answer_comment_id&&b.AnswerCommentVisible==true);
                message.data["reply_num"] = answercomment.Count();
                var list = answercomment
                    .Select(b => new { b.AnswerCommentUser.UserId,b.AnswerCommentId, b.AnswerCommentUser.UserName, b.AnswerCommentUser.UserProfile, b.AnswerCommentContent, b.AnswerCommentLike, b.InverseAnswerCommentReplyNavigation.Count, })
                    .ToList();
                message.data["reply_list"] = list.ToArray();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return message.ReturnJson();
        }

        [HttpPost("comment")]
        public string sendComment(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                //answer_id = int.Parse(Request.Form["answer_id"]);
                //answer_comment_user_id = int.Parse(Request.Form["answer_comment_user_id"]);
                //answer_comment_content = Request.Form["answer_comment_content"];
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
                int answer_comment_user_id = int.Parse(front_end_data.GetProperty("answer_comment_user_id").ToString());
                string answer_comment_content = front_end_data.GetProperty("answer_comment_content").ToString();
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                User user = myContext.Users.Single(b => b.UserId == answer_comment_user_id);
                int id = myContext.Answercomments.Max(b=>b.AnswerCommentId) + 1;
                Answercomment answercomment = new Answercomment();
                answercomment.AnswerCommentUser = user;
                answercomment.AnswerCommentVisible = true;
                answercomment.AnswerCommentReply = null;
                answercomment.AnswerCommentFather = answer_id;
                answercomment.AnswerCommentContent = answer_comment_content;
                answercomment.AnswerCommentFatherNavigation = answer;
                answercomment.AnswerCommentId = id;
                answercomment.AnswerCommentTime = DateTime.Now;
                answercomment.AnswerCommentUserId = answer_comment_user_id;
                user.UserExp += 3;
                if (user.UserExp >= user.UserLevel * user.UserLevel)
                {
                    user.UserExp -= (int)user.UserLevel * (int)user.UserLevel;
                    user.UserLevel++;
                }
                myContext.Answercomments.Add(answercomment);
                myContext.SaveChanges();
                message.data.Add("answer_comment_id", id);
                message.status = true;
                message.errorCode = 200;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("reply")]
        public string sendReply(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int comment_id = int.Parse(front_end_data.GetProperty("comment_id").ToString());
                int reply_user_id = int.Parse(front_end_data.GetProperty("reply_user_id").ToString());
                string reply_content = front_end_data.GetProperty("reply_content").ToString();
                Answercomment answercomment = myContext.Answercomments.Single(b => b.AnswerCommentId == comment_id);
                User user = myContext.Users.Single(b => b.UserId == reply_user_id);
                Answercomment new_comment = new Answercomment();
                new_comment.AnswerCommentContent = reply_content;
                new_comment.AnswerCommentFather = null;
                new_comment.AnswerCommentReply = comment_id;
                new_comment.AnswerCommentReplyNavigation = answercomment;
                int id = myContext.Answercomments.Max(b=>b.AnswerCommentId) + 1;
                new_comment.AnswerCommentId =id;
                new_comment.AnswerCommentTime = DateTime.Now;
                new_comment.AnswerCommentUserId = reply_user_id;
                new_comment.AnswerCommentUser = user;
                new_comment.AnswerCommentVisible = true;
                user.UserExp += 3;
                if (user.UserExp >= user.UserLevel * user.UserLevel)
                {
                    user.UserExp -= (int)user.UserLevel * (int)user.UserLevel;
                    user.UserLevel++;
                }
                myContext.Answercomments.Add(new_comment);
                myContext.SaveChanges();
                message.data["comment_id"] = id;
                message.errorCode = 200;
                message.status = true;

            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost]
        public string sendAnswer(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int question_id= int.Parse(front_end_data.GetProperty("question_id").ToString());
                string content = front_end_data.GetProperty("content").ToString();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string summary = front_end_data.GetProperty("summary").ToString();
                string img_base64 = front_end_data.GetProperty("image_url").ToString();
                Answer answer = new();
                int id = myContext.Answers.Max(b=>b.AnswerId) + 1;
                byte[] img_bytes = Encoding.UTF8.GetBytes(content);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                string path = "answer/content/" + id.ToString() + ".html";
                string imageurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                client.PutObject(OssHelp.bucketName, path, stream);
                User user= myContext.Users.Single(b => b.UserId == user_id);
                user.UserExp += 5;
                if (user.UserExp >= user.UserLevel * user.UserLevel)
                {
                    user.UserExp -= (int)user.UserLevel * (int)user.UserLevel;
                    user.UserLevel++;
                }
                answer.QuestionId = question_id;
                answer.AnswerUserId = user_id;
                answer.AnswerId = id;
                answer.AnswerUser = user;
                answer.AnswerContent = imageurl;
                answer.AnswerDate = DateTime.Now;
                answer.AnswerContentpic = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/user_profile/5.png";
                answer.AnswerSummary = summary;
                answer.AnswerVisible = false;
                Answerchecking answerchecking = new();
                answerchecking.AdministratorId = 0;
                answerchecking.AnswerId = id;
                answerchecking.AnswerDate = answer.AnswerDate;
                answerchecking.ReviewResult = "待审核";
                //存第一张图到oss
                if (img_base64 != "")
                {
                    string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
                    img_base64 = img_base64.Split("base64,")[1];//非常重要
                    byte[] img_bytes_ = Convert.FromBase64String(img_base64);
                    MemoryStream stream_ = new MemoryStream(img_bytes_, 0, img_bytes_.Length);
                    string path_ = "answer/" + id.ToString() + type;
                    client.PutObject(OssHelp.bucketName, path_, stream_);
                    string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path_;
                    answer.AnswerContentpic = imgurl;
                }
                answerchecking.Answer = answer;
               
                myContext.Answers.Add(answer);
                myContext.Answercheckings.Add(answerchecking);
                myContext.SaveChanges();
                message.data["answer_id"] = id;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpDelete]
        public string deleteAnswer(int answer_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                answer.AnswerVisible = false;
                Answerchecking answerchecking = myContext.Answercheckings.Single(b => b.AnswerId == answer_id);
                answerchecking.ReviewResult = "deleted";
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

        [HttpDelete("comment")]
        public string deleteAnswerComment(int answercomment_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Answercomment answercomment = myContext.Answercomments.Single(b => b.AnswerCommentId == answercomment_id);
                answercomment.AnswerCommentVisible = false;
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
        //[HttpPost("image")]
        //public string uploadImage(dynamic front_end_data)
        //{
        //    Message message = new();
        //    try
        //    {
        //        myContext.DetachAll();
        //        string img_base64 = front_end_data.GetProperty("img").ToString();
        //        int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
        //        int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
        //        Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id && b.AnswerUserId == user_id);
        //        string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
        //        img_base64 = img_base64.Split("base64,")[1];//非常重要
        //        byte[] img_bytes = Convert.FromBase64String(img_base64);
        //        var client = OssHelp.createClient();
        //        MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
        //        string path = "answer/" + answer_id.ToString() + type;
        //        client.PutObject(OssHelp.bucketName, path, stream);
        //        string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
        //        answer.AnswerContentpic = imgurl;
        //        myContext.SaveChanges();
        //        message.data.Add("imageurl", imgurl);
        //        message.status = true;
        //        message.errorCode = 200;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    return message.ReturnJson();
        //}
    }
}
