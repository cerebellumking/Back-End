﻿using Microsoft.AspNetCore.Http;
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
    public class StarController : ControllerBase
    {
        private readonly ModelContext myContext;
        public StarController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost("question")]
        public string starQuestion(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int question_id = int.Parse(front_end_data.GetProperty("question_id").ToString());

                object[] pk = { question_id,user_id };
                Starquestion old_starquestion = myContext.Starquestions.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_starquestion==null)
                {
                    Starquestion starquestion = new Starquestion();
                    starquestion.UserId = user_id;
                    starquestion.User = myContext.Users.Single(b => b.UserId == user_id);
                    starquestion.QuestionId = question_id;
                    starquestion.Question = myContext.Questions.Single(b => b.QuestionId == question_id);
                    starquestion.StarTime = DateTime.Now;
                    myContext.Starquestions.Add(starquestion);
                }
                else
                {
                    old_starquestion.StarTime = DateTime.Now;
                    old_starquestion.Cancel = false;
                }
                message.errorCode = 200;
                message.status = true;
                myContext.SaveChanges();
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPut("question")]
        public string cancelQuestionStar(dynamic front_end_data)
        {
            int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
            int question_id = int.Parse(front_end_data.GetProperty("question_id").ToString());

            Message message = new Message();
            try
            {
                myContext.DetachAll();
                Starquestion starquestion = myContext.Starquestions.Single(b => b.UserId == user_id && b.QuestionId == question_id && b.Cancel == false);
                starquestion.Cancel = true;
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

        [HttpGet("question")]
        public string whetherStarQuestion(int user_id, int question_id)
        {
            Message message = new Message();
            bool flag = myContext.Starquestions.Any(b => b.UserId == user_id && b.QuestionId == question_id && b.Cancel == false);
            int star_nums = myContext.Starquestions.Count(b => b.QuestionId == question_id && b.Cancel == false);
            message.data.Add("star_nums", star_nums);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }

        [HttpPost("answer")]
        public string starAnswer(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());

                myContext.DetachAll();
                object[] pk = { answer_id,user_id };
                Staranswer old_staranswer = myContext.Staranswers.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_staranswer==null)
                {
                    User user = myContext.Users.Single(b => b.UserId == user_id);
                    Staranswer staranswer = new Staranswer();
                    staranswer.UserId = user_id;
                    staranswer.User = user;
                    staranswer.AnswerId = answer_id;
                    staranswer.Answer = myContext.Answers.Single(b => b.AnswerId == answer_id);
                    staranswer.StarTime = DateTime.Now;
                    myContext.Staranswers.Add(staranswer);
                }
                else
                {
                    old_staranswer.StarTime = DateTime.Now;
                    old_staranswer.Cancel = false;
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

        [HttpPut("answer")]
        public string cancelAnswerStar(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int answer_id = int.Parse(front_end_data.GetProperty("answer_id").ToString());

                myContext.DetachAll();
                Staranswer staranswer = myContext.Staranswers.Single(b => b.UserId == user_id && b.AnswerId == answer_id&&b.Cancel==false);
                staranswer.Cancel = true;
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

        [HttpGet("answer")]
        public string whetherStarAnswer(int user_id, int answer_id)
        {
            Message message = new Message();
            bool flag = myContext.Staranswers.Any(b => b.UserId == user_id && b.AnswerId == answer_id && b.Cancel == false);
            int star_nums = myContext.Staranswers.Count(b => b.AnswerId == answer_id && b.Cancel == false);
            message.data.Add("star_nums", star_nums);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }

        [HttpPost("blog")]
        public string starBlog(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());

                myContext.DetachAll();
                object[] pk = { blog_id, user_id };
                Starblog old_starblog = myContext.Starblogs.Find(pk);
                /*判断该收藏是否取消过*/
                if (old_starblog==null)
                {
                    User user = myContext.Users.Single(b => b.UserId == user_id);
                    Starblog starblog = new Starblog();
                    starblog.UserId = user_id;
                    starblog.User = user;
                    starblog.BlogId = blog_id;
                    starblog.Blog = myContext.Blogs.Single(b => b.BlogId == blog_id);
                    starblog.StarTime = DateTime.Now;
                    myContext.Starblogs.Add(starblog);
                }
                else
                {
                    old_starblog.StarTime = DateTime.Now;
                    old_starblog.Cancel = false;
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

        [HttpPut("blog")]
        public string cancelBlogStar(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int blog_id = int.Parse(front_end_data.GetProperty("blog_id").ToString());

                myContext.DetachAll();
                Starblog starblog = myContext.Starblogs.Single(b => b.UserId == user_id && b.BlogId == blog_id && b.Cancel == false);
                starblog.Cancel = true;
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

        [HttpGet("blog")]
        public string whetherStarBlog(int user_id, int blog_id)
        {
            Message message = new Message();
            bool flag = myContext.Starblogs.Any(b => b.UserId == user_id && b.BlogId == blog_id && b.Cancel == false);
            int star_nums = myContext.Starblogs.Count(b => b.BlogId == blog_id && b.Cancel == false);
            message.data.Add("star_nums", star_nums);
            message.errorCode = 200;
            message.status = flag;
            return message.ReturnJson();
        }

        public class StarQuestionInfo
        {
            public int question_id { get; set; }
            public int user_id { get; set; }
            public string user_name { get; set; }
            public string[] question_tag { get; set; }
            public DateTime question_date { get; set; }
            public DateTime star_time { get; set; }
            public string question_title { get; set; }
            public decimal question_reward { get; set; }
            public int question_apply { get; set; }
            public string question_summary { get; set; }
        }

        [HttpGet("questions")]
        public string getStarQuestionList(int user_id)
        {
            Message message = new();
            try
            {
                var list = myContext.Starquestions.Where(a => a.UserId == user_id && a.Cancel == false);
                List<StarQuestionInfo> starQuestionList = new();
                foreach(var val in list)
                {
                    Question question = myContext.Questions.Single(b => b.QuestionId == val.QuestionId);
                    if (question.QuestionVisible == false)
                        continue;
                    StarQuestionInfo star = new();
                    star.question_id = question.QuestionId;
                    star.user_id = (int)question.QuestionUserId;
                    var user = myContext.Users.Where(b => b.UserId == question.QuestionUserId)
                        .Select(b => new
                        {
                            b.UserName,
                        }).First();
                    star.user_name = user.UserName;
                    star.question_tag = question.QuestionTag.Split('-');
                    star.question_date = question.QuestionDate;
                    star.star_time = val.StarTime;
                    star.question_title = question.QuestionTitle;
                    star.question_reward = (decimal)question.QuestionReward;
                    star.question_apply = (int)question.QuestionApply;
                    star.question_summary = question.QuestionSummary;
                    starQuestionList.Add(star);
                }
                message.data.Add("count", starQuestionList.Count);
                message.data.Add("stars", starQuestionList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        public class StarAnswerInfo
        {
            public int answer_id { get; set; }
            public int? question_id { get; set; }
            public int? answer_user_id { get; set; }
            public string answer_user_name { get; set; }
            public int? question_user_id { get; set; }
            public string question_user_name { get; set; }
            public DateTime answer_date { get; set; }
            public DateTime star_time { get; set; }
            public string question_title { get; set; }
            public decimal? answer_like { get; set; }
            public decimal? answer_coin { get; set; }
            public string answer_summary { get; set; }
        }

        [HttpGet("answers")]
        public string getStarAnswerList(int user_id)
        {
            Message message = new();
            try
            {
                var list = myContext.Staranswers.Where(a => a.UserId == user_id && a.Cancel == false);
                List<StarAnswerInfo> starAnswerList = new();
                foreach(var val in list)
                {
                    Answer answer = myContext.Answers.Single(b => b.AnswerId == val.AnswerId);
                    if (answer.AnswerVisible == false)
                        continue;
                    StarAnswerInfo star = new();
                    User answer_user = myContext.Users.Single(b => b.UserId == answer.AnswerUserId);
                    Question question = myContext.Questions.Single(b => b.QuestionId == answer.QuestionId);
                    User question_user = myContext.Users.Single(b => b.UserId == question.QuestionUserId);
                    star.answer_id = answer.AnswerId;
                    star.question_id = answer.QuestionId;
                    star.answer_user_id = answer.AnswerUserId;
                    star.answer_user_name = answer_user.UserName;
                    star.question_user_id = question.QuestionUserId;
                    star.question_user_name = question_user.UserName;
                    star.answer_date = answer.AnswerDate;
                    star.star_time = val.StarTime;
                    star.question_title = question.QuestionTitle;
                    star.answer_like = answer.AnswerLike;
                    star.answer_coin = answer.AnswerCoin;
                    star.answer_summary = answer.AnswerSummary;
                    starAnswerList.Add(star);
                }
                message.data.Add("count", starAnswerList.Count);
                message.data.Add("stars", starAnswerList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
        public class StarBlogInfo
        {
            public int blog_id { get; set; }
            public int? blog_user_id { get; set; }
            public string blog_user_name { get; set; }
            public string[] blog_tag { get; set; }
            public DateTime blog_date { get; set; }
            public DateTime star_date { get; set; }
            public decimal? blog_like { get; set; }
            public decimal? blog_coin { get; set; }
            public string blog_summary { get; set; }
            public string image_url { get; set; }
        }

        [HttpGet("blogs")]
        public string getStarBlogList(int user_id)
        {
            Message message = new();
            try
            {
                var list = myContext.Starblogs.Where(a => a.UserId ==user_id && a.Cancel == false);
                List<StarBlogInfo> starBlogList = new();
                foreach(var val in list)
                {
                    Blog blog = myContext.Blogs.Single(b => b.BlogId == val.BlogId);
                    if (blog.BlogVisible == false)
                        continue;
                    StarBlogInfo star = new();
                    star.blog_id = blog.BlogId;
                    star.blog_user_id = blog.BlogUserId;
                    User user = myContext.Users.Single(b => b.UserId == blog.BlogUserId);
                    star.blog_user_name = user.UserName;
                    star.blog_tag = blog.BlogTag.Split('-');
                    star.blog_date = blog.BlogDate;
                    star.star_date = val.StarTime;
                    star.blog_like = blog.BlogLike;
                    star.blog_coin = blog.BlogCoin;
                    star.blog_summary = blog.BlogSummary;
                    star.image_url = blog.BlogImage;
                    starBlogList.Add(star);
                }
                message.data.Add("count", starBlogList.Count);
                message.data.Add("stars", starBlogList.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
    }
}
