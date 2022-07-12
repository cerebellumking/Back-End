using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
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

        [HttpGet]
        public string getUniversityInfo(int university_id)
        {
            UniversityMessage message = new UniversityMessage();
            try
            {
                University university = myContext.Universities.Single(b => b.UniversityId == university_id);
                var rank = myContext.Ranks
                    .Where(a => a.UniversityId == university_id)
                    .Select(b=>new {b.RankYear,b.UniversityQsRank,b.UniversityTheRank,b.UniversityUsnewsRank })
                    .ToList();
                message.data["university_id"] = university_id;
                message.data["university_email"] = university.UniversityEmail;
                message.data["university_chname"] = university.UniversityChName;
                message.data["university_enname"] = university.UniversityEnName;
                message.data["university_region"] = university.UniversityRegion;
                message.data["university_country"] = university.UniversityCountry;
                message.data["university_location"] = university.UniversityLocation;
                message.data["university_introduction"] = university.UniversityIntroduction;
                message.data["university_student_num"] = university.UniversityStudentNum;
                message.data["university_website"] = university.UniversityWebsite;
                message.data["university_college"] = university.UniversityCollege;
                message.data["university_abbreviation"] = university.UniversityAbbreviation;
                message.data["rank"] = rank.ToArray();
                //message.data["university_QS_rank"] = university.UniversityQsRank;
                //message.data["university_THE_rank"] = university.UniversityTheRank;
                //message.data["university_USNews_rank"] = university.UniversityUsnewsRank;
                //message.data["rank_year"] = university.Year;
                message.status = true;
                message.errorCode = 200;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return message.ReturnJson();
        }


        [HttpPost]
        public string addUniversity(string university_email, string university_chname, string university_enname, string university_region, string university_country, string university_location,
        string university_introduction, int university_student_num, string university_website, string university_college, string university_abbreviation,
        short university_QS_rank, short university_THE_rank, short university_USNews_rank,short rank_year)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                University university = new University();
                university.UniversityEmail = university_email;
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
                //university.UniversityQsRank = university_QS_rank;
                //university.UniversityTheRank = university_THE_rank;
                //university.UniversityUsnewsRank = university_USNews_rank;
                //university.Year = rank_year;
                var count = myContext.Universities.Count();
                int id;
                if (count == 0)
                    id = 1;
                else
                {
                    id = myContext.Universities.Select(b => b.UniversityId).Max() + 1;
                }
                university.UniversityId = id;
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
        /*tag:以哪个排行榜为主
         0：qs，1：the，2：usnews
         */
        [HttpGet("rank")]
        public string showQsRank(int rank_year,int tag,string university_country="")
        {
            Message message = new Message();
            try
            {
                //var ranklist = myContext.Ranks
                //    .Where(a => a.RankYear == rank_year&&a.University.UniversityCountry.Contains(university_country))
                //    .OrderBy(b => b.UniversityQsRank)
                //    .Select(b => new { b.UniversityId, b.UniversityQsRank, b.UniversityTheRank, b.UniversityUsnewsRank, b.University.UniversityBadge, b.University.UniversityChName, b.University.UniversityEnName, b.University.UniversityStudentNum, b.University.UniversityIntroduction, b.University.UniversityLocation })
                //    .ToList();
                List<Rank> ranklist = new List<Rank>();
                List<UniversityList> lists = new List<UniversityList>();
                if (tag == 0)
                {
                    ranklist = myContext.Ranks
                        .Where(a => a.RankYear == rank_year && a.University.UniversityCountry.Contains(university_country))
                        .OrderBy(b => b.UniversityQsRank)
                        .ToList();
                }
                else if (tag == 1)
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
                foreach(var rank in ranklist)
                {
                    UniversityList list = new UniversityList();
                    if(rank.University==null)
                        rank.University = myContext.Universities.Single(b => b.UniversityId == rank.UniversityId);
                    list.university_id = rank.UniversityId;
                    list.university_badge = rank.University.UniversityBadge;
                    list.university_chname = rank.University.UniversityChName;
                    list.university_enname = rank.University.UniversityEnName;
                    list.university_introduction = rank.University.UniversityIntroduction.Substring(0,60);
                    list.university_location = rank.University.UniversityLocation;
                    list.university_qs_rank = rank.UniversityQsRank;
                    list.university_student_num = rank.University.UniversityStudentNum;
                    list.university_the_rank = rank.UniversityTheRank;
                    list.university_usnews_rank = rank.UniversityUsnewsRank;
                    lists.Add(list);
                }
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
                message.data["university_list"] = lists;
                
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
    }
}
