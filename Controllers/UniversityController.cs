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

        [HttpPost]
        public string addUniversity(string university_email,string university_name,string university_region,string university_country,string university_location,
        string university_introduction, int university_student_num,string university_website,string university_college,string university_abbreviation,
        short university_QS_rank,short university_THE_rank,short university_USNews_rank)
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
                university.UniversityQsRank = university_QS_rank;
                university.UniversityTheRank = university_THE_rank;
                university.UniversityUsnewsRank = university_USNews_rank;
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
                message.data.Add("university_id",id);
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
