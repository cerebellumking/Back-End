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
                    message.data.Add("answer_id", answer_id);
                    message.data.Add("answer_user_id", answer.AnswerUserId);
                    message.data.Add("question_id", answer.QuestionId);
                    message.data.Add("tag", question.QuestionTag);
                    message.data.Add("answer_date", answer.AnswerDate);
                    message.data.Add("answer_content", answer.AnswerContent);
                    message.data.Add("answer_contentpic", answer.AnswerContentpic);
                    message.data.Add("answer_coin", answer.AnswerCoin);
                    message.data.Add("answer_like", answer.AnswerLike);
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
                var answercomment = myContext.Answercomments.Where(b => b.AnswerCommentFather == answer_id);
                message.data["comment_num"] = answercomment.Count();
                var list = answercomment
                    .Select(b => new { b.AnswerCommentId, b.AnswerCommentUser.UserName, b.AnswerCommentUser.UserProfile, b.AnswerCommentContent, b.AnswerCommentLike,b.InverseAnswerCommentReplyNavigation.Count,})
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
                    .Select(b => new { b.AnswerCommentId, b.AnswerCommentUser.UserName, b.AnswerCommentUser.UserProfile, b.AnswerCommentContent, b.AnswerCommentLike, b.InverseAnswerCommentReplyNavigation.Count, })
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
        public string sendComment(int answer_id, int answer_comment_user_id, string answer_comment_content)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                answer_id = int.Parse(Request.Form["answer_id"]);
                answer_comment_user_id = int.Parse(Request.Form["answer_comment_user_id"]);
                answer_comment_content = Request.Form["answer_comment_content"];
                Answer answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                User user = myContext.Users.Single(b => b.UserId == answer_comment_user_id);
                int id = myContext.Answercomments.Count() + 1;
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
        public string sendReply()
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int comment_id = int.Parse(Request.Form["comment_id"]);
                int reply_user_id = int.Parse(Request.Form["reply_user_id"]);
                string reply_content = Request.Form["reply_content"];
                Answercomment answercomment = myContext.Answercomments.Single(b => b.AnswerCommentId == comment_id);
                User user = myContext.Users.Single(b => b.UserId == reply_user_id);
                Answercomment new_comment = new Answercomment();
                new_comment.AnswerCommentContent = reply_content;
                new_comment.AnswerCommentFather = null;
                new_comment.AnswerCommentReply = comment_id;
                new_comment.AnswerCommentReplyNavigation = answercomment;
                int id = myContext.Answercomments.Count() + 1;
                new_comment.AnswerCommentId =id;
                new_comment.AnswerCommentTime = DateTime.Now;
                new_comment.AnswerCommentUserId = reply_user_id;
                new_comment.AnswerCommentUser = user;
                new_comment.AnswerCommentVisible = true;
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
    }
}
