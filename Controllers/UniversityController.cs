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
                message.data["university_name"] = university.UniversityName;
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
            catch
            {

            }
            return message.ReturnJson();
        }


        [HttpPost]
        public string addUniversity(string university_email, string university_name, string university_region, string university_country, string university_location,
        string university_introduction, int university_student_num, string university_website, string university_college, string university_abbreviation,
        short university_QS_rank, short university_THE_rank, short university_USNews_rank,short rank_year)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                University university = new University();
                university.UniversityEmail = university_email;
                university.UniversityName = university_name;
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
            catch
            {
                message.status = false;
                message.errorCode = 500;
            }
            return message.ReturnJson();
        }
    }
}
