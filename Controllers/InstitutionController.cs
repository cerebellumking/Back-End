using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;

namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    public class InstitutionController : Controller
    {
        private readonly ModelContext myContext;
        public InstitutionController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getInstitutionInfo(int institution_id)
        {
            InstitutionMessage message = new();
            try
            {
                Institution institution = myContext.Institutions.Single(b => b.InstitutionId == institution_id);
                message.data["institution_id"] = institution_id;
                message.data["institution_name"] = institution.InstitutionName;
                message.data["institution_phone"] = institution.InstitutionPhone;
                message.data["institution_qualify"] = institution.InstitutionQualify;
                message.data["institution_introduction"] = institution.InstitutionIntroduction;
                message.data["institution_location"] = institution.InstitutionLocation;
                message.data["institution_profile"] = institution.InstitutionProfile;
                message.data["institution_city"] = institution.InstitutionCity;
                message.data["institution_province"] = institution.InstitutionProvince;
                message.data["institution_email"] = institution.InstitutionEmail;
                message.data["institution_lessons_characteristic"] = institution.InstitutionLessonsCharacter;
                message.data["institution_lessons"] = institution.InstitutionLessons;
                message.data["institution_createtime"] = institution.InstitutionCreatetime;
                message.data["institution_target"] = institution.InstitutionTarget;
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpPost]
        public string addInstitution(
            string institution_name,
            string institution_phone,
            string institution_qualify,
            string institution_introduction,
            string institution_profile,
            string institution_city,
            string institution_location,
            string institution_email,
            string institution_lessons_characteristic,
            string institution_lessons
            //DateTime institution_createtime
            )
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                Institution institution = new();
                institution.InstitutionName = institution_name;
                institution.InstitutionPhone = institution_phone;
                institution.InstitutionQualify = institution_qualify;
                institution.InstitutionIntroduction = institution_introduction;
                institution.InstitutionProfile = institution_profile;
                institution.InstitutionCity = institution_city;
                institution.InstitutionLocation = institution_location;
                institution.InstitutionEmail = institution_email;
                institution.InstitutionLessonsCharacter = institution_lessons_characteristic;
                institution.InstitutionLessons = institution_lessons;
                institution.InstitutionCreatetime = DateTime.Now;
                int id = 1, count = myContext.Institutions.Count();
                if(count != 0)
                {
                    id = myContext.Institutions.Select(b => b.InstitutionId).Max() + 1;

                }
                institution.InstitutionId = id;
                myContext.Institutions.Add(institution);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
                message.data.Add("institution_id", id);
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
