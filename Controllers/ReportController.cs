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
    public class ReportController : ControllerBase
    {
        private readonly ModelContext myContext;
        public ReportController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost("answer")]
        public string reportAnswer(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());
                string report_reason = front_end_data.GetProperty("report_reason").ToString();
                Answerreport answerreport = new();
                int id = myContext.Answerreports.Count()+1;
                answerreport.ReportId = id;
                answerreport.UserId = user_id;
                answerreport.AdministratorId = 0;
                answerreport.ReportReason = report_reason;
                answerreport.ReportDate = DateTime.Now;
                answerreport.AnswerId = answer_id;
                answerreport.Answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                myContext.Answerreports.Add(answerreport);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("answercomment")]
        public string reportAnswerComment(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int answer_comment_id = int.Parse(front_end_data.GetProperty("answercomment_id").ToString());
                string report_reason = front_end_data.GetProperty("report_reason").ToString();
                Answercommentreport answercommentreport=new();
                int id = myContext.Answercommentreports.Count() + 1;
                answercommentreport.ReportId = id;
                answercommentreport.UserId = user_id;
                answercommentreport.AdministratorId = 0;
                answercommentreport.ReportReason = report_reason;
                answercommentreport.ReportDate = DateTime.Now;
                answercommentreport.AnswerCommentId = answer_comment_id;
                answercommentreport.AnswerComment = myContext.Answercomments.Single(b => b.AnswerCommentId == answer_comment_id);
                myContext.Answercommentreports.Add(answercommentreport);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("blog")]
        public string reportBlog(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());
                string report_reason = front_end_data.GetProperty("report_reason").ToString();
                Blogreport blogreport = new();
                int id = myContext.Blogreports.Count() + 1;
                blogreport.ReportId = id;
                blogreport.UserId = user_id;
                blogreport.AdministratorId = 0;
                blogreport.ReportReason = report_reason;
                blogreport.ReportDate = DateTime.Now;
                blogreport.BlogId = blog_id;
                blogreport.Blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                myContext.Blogreports.Add(blogreport);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("blogcomment")]
        public string reportBlogComment(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int blog_comment_id = int.Parse(front_end_data.GetProperty("blogcomment_id").ToString());
                string report_reason = front_end_data.GetProperty("report_reason").ToString();
                Blogcommentreport blogcommentreport = new();
                int id = myContext.Blogcommentreports.Count() + 1;
                blogcommentreport.ReportId = id;
                blogcommentreport.UserId = user_id;
                blogcommentreport.AdministratorId = 0;
                blogcommentreport.ReportReason = report_reason;
                blogcommentreport.ReportDate = DateTime.Now;
                blogcommentreport.BlogCommentId = blog_comment_id;
                blogcommentreport.BlogComment = myContext.Blogcomments.Single(b => b.BlogCommentId == blog_comment_id);
                myContext.Blogcommentreports.Add(blogcommentreport);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("unsolved")]
        public string getUnsolvedReport(int user_id)
        {
            Message message = new();
            try
            {
                var answer_report_list = myContext.Answerreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var blog_report_list = myContext.Blogreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var answer_comment_report_list = myContext.Answercommentreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var blog_comment_report_list = myContext.Blogcommentreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null)
                    .Select(b => new { b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                message.data["answer_report"] = answer_report_list.ToArray();
                message.data["blog_report"] = blog_report_list.ToArray();
                message.data["answercomment_report"] = answer_comment_report_list.ToArray();
                message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("unseen")]
        public string getUnseenReport(int user_id)
        {
            Message message = new();
            try
            {
                var answer_report_list = myContext.Answerreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null && b.ReportState == false)
                    .Select(b => new { b.ReportDate, b.ReportId, b.AnswerId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var blog_report_list = myContext.Blogreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null && b.ReportState == false)
                    .Select(b => new { b.ReportDate, b.ReportId, b.BlogId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var answer_comment_report_list = myContext.Answercommentreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null && b.ReportState == false)
                    .Select(b => new { b.ReportDate, b.ReportId, b.AnswerCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                var blog_comment_report_list = myContext.Blogcommentreports
                    .Where(b => b.UserId == user_id && b.ReportAnswerResult == null && b.ReportState == false)
                    .Select(b => new { b.ReportDate, b.ReportId, b.BlogCommentId, b.ReportReason, b.ReportState, b.ReportAnswerResult })
                    .ToList();
                message.data["answer_report"] = answer_report_list.ToArray();
                message.data["blog_report"] = blog_report_list.ToArray();
                message.data["answercomment_report"] = answer_comment_report_list.ToArray();
                message.data["blogcomment_report"] = blog_comment_report_list.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpGet("answer")]
        public string whetherReportedAnswer(int user_id,int answer_id)
        {
            Message message = new();
            try
            {
                message.errorCode = 200;
                message.status = myContext.Answerreports.Any(b => b.UserId == user_id && b.AnswerId == answer_id);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blog")]
        public string whetherReportedBlog(int user_id, int blog_id)
        {
            Message message = new();
            try
            {
                message.errorCode = 200;
                message.status = myContext.Blogreports.Any(b => b.UserId == user_id && b.BlogId == blog_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("answercomment")]
        public string whetherReportedAnswerComment(int user_id, int answercomment_id)
        {
            Message message = new();
            try
            {
                message.errorCode = 200;
                message.status = myContext.Answercommentreports.Any(b => b.UserId == user_id && b.AnswerCommentId == answercomment_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("blogcomment")]
        public string whetherReportedBlogComment(int user_id, int blogcomment_id)
        {
            Message message = new();
            try
            {
                message.errorCode = 200;
                message.status = myContext.Blogcommentreports.Any(b => b.UserId == user_id && b.BlogCommentId == blogcomment_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
