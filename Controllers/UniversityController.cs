using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using System.Text;
using System.IO;
namespace Back_End.Controllers
{
    public class UniversityList
    {
        public int university_id { get; set; }
        public int university_qs_rank { get; set; }
        public int university_the_rank { get; set; }
        public int university_usnews_rank { get; set; }
        public string university_badge { get; set; }
        public string university_chname { get; set; }
        public string university_enname { get; set; }
        public decimal university_student_num { get; set; }
        public string university_introduction { get; set; }
        public string university_location { get; set; }
        public string university_tuition { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly ModelContext myContext;
        public UniversityController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet("id")]
        public string getUniversityIDByName(string university_chname)
        {
            Message message = new Message();
            try
            {
                University university = myContext.Universities.Single(b => b.UniversityChName == university_chname);
                message.errorCode = 200;
                message.data["university_id"] = university.UniversityId;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet]
        public string getUniversityInfoById(int university_id)
        {
            UniversityMessage message = new UniversityMessage();
            try
            {
                University university = myContext.Universities.Single(b => b.UniversityId == university_id);
                var rank = myContext.Ranks
                    .Where(a => a.UniversityId == university_id)
                    .Select(b => new { b.RankYear, b.UniversityQsRank, b.UniversityTheRank, b.UniversityUsnewsRank })
                    .ToList();
                message.data["university_id"] = university_id;
                message.data["university_badge"] = university.UniversityBadge;
                message.data["university_email"] = university.UniversityEmail;
                message.data["university_chname"] = university.UniversityChName;
                message.data["university_enname"] = university.UniversityEnName;
                message.data["university_region"] = university.UniversityRegion;
                message.data["university_country"] = university.UniversityCountry;
                message.data["university_location"] = university.UniversityLocation;
                message.data["university_introduction"] = university.UniversityIntroduction;
                message.data["university_student_num"] = university.UniversityStudentNum;
                message.data["university_website"] = university.UniversityWebsite;
                message.data["university_college"] = university.UniversityCollege.Split('\n');
                message.data["university_abbreviation"] = university.UniversityAbbreviation;
                message.data["university_address_x"] = university.UniversityAddressX;
                message.data["university_address_y"] = university.UniversityAddressY;
                message.data["university_teacher_num"] = university.UniversityTeacherNum;
                message.data["university_tuition"] = university.UniversityTuition;
                message.data["university_tofel_requirement"] = university.UniversityTofelRequirement;
                message.data["university_ielts_requirement"] = university.UniversityIeltsRequirement;
                message.data["university_photo"] = university.UniversityPhoto.Split(';');
                message.data["rank"] = rank.ToArray();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("chname")]
        public string getUniversityInfoByChName(string chname)
        {
            chname = System.Web.HttpUtility.UrlDecode(chname); // url解码
            Message message = new();
            try
            {
                //University university = myContext.Universities.Single(b => b.UniversityChName.Contains(chname));
                var university = myContext.Universities
                    .Where(item => item.UniversityChName.Contains(chname))
                    .Select(item => new
                    {
                        item.UniversityId
                    })
                    .ToList().First();

                return getUniversityInfoById(university.UniversityId);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        //[HttpPost("")]
        //public string addUniversity(dynamic university_info)
        //{
        //    string university_email = university_info.GetProperty("university_email").ToString();
        //    string university_chname = university_info.GetProperty("university_chname").ToString();
        //    string university_enname = university_info.GetProperty("university_enname").ToString();
        //}

        [HttpPost]
        public string addUniversity(dynamic university_info)
        {
            string university_email = university_info.GetProperty("university_email").ToString();
            string university_chname = university_info.GetProperty("university_chname").ToString();
            string university_enname = university_info.GetProperty("university_enname").ToString();
            string university_badge = university_info.GetProperty("university_badge").ToString();
            string university_region = university_info.GetProperty("university_region").ToString();
            string university_country = university_info.GetProperty("university_country").ToString();
            string university_location = university_info.GetProperty("university_location").ToString();
            string university_introduction = university_info.GetProperty("university_introduction").ToString();
            int university_student_num = int.Parse(university_info.GetProperty("university_student_num").ToString());
            string university_website = university_info.GetProperty("university_website").ToString();
            string university_college = university_info.GetProperty("university_college").ToString();
            string university_abbreviation = university_info.GetProperty("university_abbreviation").ToString();
            short university_teacher_num = short.Parse(university_info.GetProperty("university_teacher_num").ToString());
            string university_tuition = university_info.GetProperty("university_tuition").ToString();
            short qs = short.Parse(university_info.GetProperty("qs_rank").ToString());
            short the = short.Parse(university_info.GetProperty("the_rank").ToString());
            short usnews = short.Parse(university_info.GetProperty("usnews_rank").ToString());
            //decimal university_address_x = decimal.Parse(university_info.GetProperty("university_address_x").ToString());
            //decimal university_address_y = decimal.Parse(university_info.GetProperty("university_address_y").ToString());
            //string university_photo = university_info.GetProperty("university_photo").ToString();
            //byte unviersity_tofel_requirement = byte.Parse(university_info.GetProperty("unviersity_tofel_requirement").ToString());
            //decimal unviersity_iltes_requirement = decimal.Parse(university_info.GetProperty("unviersity_iltes_requirement").ToString());
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                University university = new University();
                //university.UniversityAddressX = university_address_x;
                //university.UniversityAddressY = university_address_y;
                //university.UniversityIeltsRequirement = unviersity_iltes_requirement;
                //university.UniversityTofelRequirement = unviersity_tofel_requirement;
                university.UniversityEmail = university_email;
                university.UniversityTuition = university_tuition;
                university.UniversityChName = university_chname;
                university.UniversityEnName = university_enname;
                university.UniversityRegion = university_region;
                university.UniversityCountry = university_country;
                university.UniversityLocation = university_location;
                university.UniversityIntroduction = university_introduction;
                university.UniversityStudentNum = university_student_num;
                university.UniversityWebsite = university_website;
                university.UniversityCollege = university_college;
                university.UniversityAbbreviation = university_abbreviation;
                var count = myContext.Universities.Count();
                int id;
                if (count == 0)
                    id = 1;
                else
                {
                    id = myContext.Universities.Select(b => b.UniversityId).Max() + 1;
                }
                university.UniversityId = id;
                Rank rank = new();
                rank.RankYear = 2022;
                rank.UniversityId = id;
                rank.UniversityQsRank = qs;
                rank.UniversityTheRank = the;
                rank.UniversityUsnewsRank = usnews;
                myContext.Ranks.Add(rank);
                //添加图片
                string type1 = "." + university_badge.Split(',')[0].Split(';')[0].Split('/')[1];
                university_badge = university_badge.Split("base64,")[1];
                byte[] img_bytes_badge = Convert.FromBase64String(university_badge);
                //string type2 = "." + university_photo.Split(',')[0].Split(';')[0].Split('/')[1];
                //university_photo = university_photo.Split("base64,")[1];
                //byte[] img_bytes_photo =  Convert.FromBase64String(university_photo);
                var client = OssHelp.createClient();
                MemoryStream stream1 = new MemoryStream(img_bytes_badge, 0, img_bytes_badge.Length);
                //MemoryStream stream2 = new MemoryStream(img_bytes_photo, 0, img_bytes_photo.Length);
                string path1 = "university/badge" + id.ToString() + type1;
                //string path2 = "university/photo" + id.ToString() + type2;
                string imageurl1 = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path1;
                //string imageurl2 = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path2;
                client.PutObject(OssHelp.bucketName, path1, stream1);
                //client.PutObject(OssHelp.bucketName, path2, stream2);

                university.UniversityBadge = imageurl1;
                //university.UniversityPhoto = imageurl2;
                myContext.Universities.Add(university);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
                message.data.Add("university_id", id);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            return message.ReturnJson();
        }

        [HttpPost("change")]
        public string changeUniversityInfo()
        {
            Message message = new();
            try
            {
                int university_id = int.Parse(Request.Form["id"]);
                University university = myContext.Universities.Single(item => item.UniversityId == university_id);

                // name
                string chname = Request.Form["chname"];
                string enname = Request.Form["enname"];
                if (chname != null && enname != null)
                {
                    university.UniversityChName = chname;
                    university.UniversityEnName = enname;
                }

                // badge
                /////////

                // abbreviation
                string abbreviation = Request.Form["abbreviation"];
                if (abbreviation != null)
                {
                    university.UniversityAbbreviation = abbreviation;
                }

                // country
                string country = Request.Form["country"];
                if (country != null)
                {
                    university.UniversityCountry = country;
                }

                // region

                string region = Request.Form["region"];
                if (region != null)
                {
                    university.UniversityRegion = region;
                }

                // location
                string location = Request.Form["location"];
                if(location != null)
                {
                    university.UniversityLocation = location;
                }

                // email
                string email = Request.Form["email"];
                if (email != null)
                {
                    university.UniversityEmail = email;
                }

                // website
                string website = Request.Form["website"];
                if (website != null)
                {
                    university.UniversityWebsite = website;
                }

                // student_num
                string student_num = Request.Form["student_num"];
                if (student_num != null)
                {
                    university.UniversityStudentNum = decimal.Parse(student_num);
                }

                // teacher_num
                string teacher_num = Request.Form["teacher_num"];
                if (teacher_num != null)
                {
                    university.UniversityTeacherNum = short.Parse(teacher_num);
                }

                // 排名相关
                Rank rank = myContext.Ranks.Single(item => item.UniversityId == university_id && item.RankYear == 2022);
                // qs_rank
                string qs_rank = Request.Form["qs_rank"];
                if (qs_rank != null)
                {
                    rank.UniversityQsRank = short.Parse(qs_rank);
                }

                // the_rank
                string the_rank = Request.Form["the_rank"];
                if (the_rank != null)
                {
                    rank.UniversityTheRank = short.Parse(the_rank);
                }

                // usnews_rank
                string usnews_rank = Request.Form["usnews_rank"];
                if (usnews_rank != null)
                {
                    rank.UniversityUsnewsRank = short.Parse(usnews_rank);
                }

                //tofel_requirement
                string tofel_requirement = Request.Form["tofel_requirement"];
                if (tofel_requirement != null)
                {
                    university.UniversityTofelRequirement = byte.Parse(tofel_requirement);
                }

                //ielts_requirement
                string ielts_requirement = Request.Form["ielts_requirement"];
                if (ielts_requirement != null)
                {
                    university.UniversityIeltsRequirement = decimal.Parse(ielts_requirement);
                }

                //tuition
                string tuition = Request.Form["tuition"];
                if (tuition != null)
                {
                    university.UniversityTuition = tuition;
                }

                //introduction
                string introduction = Request.Form["introduction"];
                if (introduction != null)
                {
                    university.UniversityIntroduction = introduction;
                }

                //college
                string college = Request.Form["college"];
                if (college != null)
                {
                    university.UniversityCollege = college;
                }
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        /*tag:以哪个排行榜为主
         QS_rank：qs，The_rank：the，USNews_rank：usnews
         */
        [HttpGet("rank")]
        public string showQsRank(int page,int rank_year, string tag,int page_size = 5,string university_country = "")
        {
            Message message = new Message();
            university_country = System.Web.HttpUtility.UrlDecode(university_country); // url解码
            try
            {
                //var ranklist = myContext.Ranks
                //    .Where(a => a.RankYear == rank_year&&a.University.UniversityCountry.Contains(university_country))
                //    .OrderBy(b => b.UniversityQsRank)
                //    .Select(b => new { b.UniversityId, b.UniversityQsRank, b.UniversityTheRank, b.UniversityUsnewsRank, b.University.UniversityBadge, b.University.UniversityChName, b.University.UniversityEnName, b.University.UniversityStudentNum, b.University.UniversityIntroduction, b.University.UniversityLocation })
                //    .ToList();
                List<Rank> ranklist = new List<Rank>();
                List<UniversityList> lists = new List<UniversityList>();
                if (tag == "QS_rank")
                {
                    ranklist = myContext.Ranks
                        .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                        .OrderBy(b => b.UniversityQsRank)
                        .Skip(page_size*(page-1))
                        .Take(page_size)
                        .ToList();
                }
                else if (tag == "THE_rank")
                {
                    ranklist = myContext.Ranks
                      .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                      .OrderBy(b => b.UniversityTheRank)
                      .Skip(page_size * (page - 1))
                      .Take(page_size)
                      .ToList();
                }
                else
                {
                    ranklist = myContext.Ranks
                       .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                       .OrderBy(b => b.UniversityUsnewsRank)
                       .Skip(page_size * (page - 1))
                       .Take(page_size)
                       .ToList();
                }
                foreach (var rank in ranklist)
                {
                    UniversityList list = new UniversityList();
                    if (rank.University == null)
                        rank.University = myContext.Universities.Single(b => b.UniversityId == rank.UniversityId);
                    list.university_id = rank.UniversityId;
                    list.university_badge = rank.University.UniversityBadge;
                    list.university_chname = rank.University.UniversityChName;
                    list.university_enname = rank.University.UniversityEnName;
                    string temp="";
                    if (rank.University.UniversityIntroduction.Length > 90)
                    {
                        temp = rank.University.UniversityIntroduction.Substring(0, 90);
                        list.university_introduction = temp.Substring(0, temp.LastIndexOf('，')) + "......";
                    }
                    else
                    {
                        list.university_introduction = rank.University.UniversityIntroduction;
                    }
                    //list.university_introduction = temp;
                    
                    list.university_location = rank.University.UniversityLocation;
                    list.university_qs_rank = rank.UniversityQsRank;
                    list.university_student_num = rank.University.UniversityStudentNum;
                    list.university_the_rank = rank.UniversityTheRank;
                    list.university_usnews_rank = rank.UniversityUsnewsRank;
                    list.university_tuition = rank.University.UniversityTuition;
                    lists.Add(list);
                }
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
                message.data["university_list"] = lists;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("num")]
        public string getNumber(int rank_year, string tag, string university_country = "")
        {
            Message message = new Message();
            university_country = System.Web.HttpUtility.UrlDecode(university_country); // url解码
            try
            {
                List<Rank> ranklist = new List<Rank>();
                List<UniversityList> lists = new List<UniversityList>();
                if (tag == "QS_rank")
                {
                    ranklist = myContext.Ranks
                        .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                        .OrderBy(b => b.UniversityQsRank)
                        .ToList();
                }
                else if (tag == "THE_rank")
                {
                    ranklist = myContext.Ranks
                      .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                      .OrderBy(b => b.UniversityTheRank)
                      .ToList();
                }
                else
                {
                    ranklist = myContext.Ranks
                       .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                       .OrderBy(b => b.UniversityUsnewsRank)

                       .ToList();
                }
                foreach (var rank in ranklist)
                {
                    UniversityList list = new UniversityList();
                    if (rank.University == null)
                        rank.University = myContext.Universities.Single(b => b.UniversityId == rank.UniversityId);
                    list.university_id = rank.UniversityId;
                    list.university_chname = rank.University.UniversityChName;
                    lists.Add(list);
                }
                message.data["university_list"] = lists;
                message.status = true;
                message.errorCode = 200;
                message.data["num"] = ranklist.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("get_rank")]
        public string getUniversityRank(int university_id,int rank_year = 2022)
        {
            Message message = new();
            try
            {
                var rank = myContext.Ranks.Where(b => b.UniversityId == university_id && b.RankYear == rank_year).Select(b => new
                {
                    b.UniversityId,
                    b.UniversityQsRank,
                    b.UniversityTheRank,
                    b.UniversityUsnewsRank,
                }).ToList();
                message.data.Add("rank", rank.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }
        public class UserInfo
        {
            public int user_id { get; set; }
            public string user_profile { get; set; }
            public decimal user_follower { get; set; }
            public string user_signature { get; set; }
            public decimal user_level { get; set; }
            public string user_gender { get; set; }
            public string user_name { get; set; }
        }
        [HttpGet("get_oldboy")]
        public string getOldBoy(int university_id)
        {
            Message message = new();
            try
            {
                var user_list = myContext.Qualifications
                    .Where(b => b.Visible == true&&b.UniversityId==university_id)
                    .Select(b => b.UserId)
                    .Distinct()
                    .ToList();
                List<UserInfo> userInfos = new();
                foreach(int id in user_list)
                {
                    User user = myContext.Users.Single(b => b.UserId == id);
                    if (user.UserState == false)
                        continue;
                    UserInfo userInfo = new();
                    userInfo.user_id = id;
                    userInfo.user_profile = user.UserProfile;
                    userInfo.user_follower = user.UserFollower;
                    userInfo.user_signature = user.UserSignature;
                    userInfo.user_gender=user.UserGender;
                    userInfo.user_name=user.UserName;
                    userInfo.user_level=user.UserLevel;
                    userInfos.Add(userInfo);
                }
                message.data.Add("user_info", userInfos.ToArray());
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return message.ReturnJson();
        }

        [HttpDelete]
        public string deleteUniversity(int university_id)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                myContext.Universities.Remove(myContext.Universities.Single(b => b.UniversityId == university_id));
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
