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
            InstitutionMessage message = new InstitutionMessage();
            try
            {
                Institution institution = myContext.Institutions.Single(b => b.InstitutionId == institution_id);
                message.data["institution_id"] = institution_id;
                message.data["institution_name"] = institution.InstitutionName;
                message.data["institution_phone"] = institution.InstitutionPhone;
                message.data["institution_qualify"] = institution.InstitutionQualify;
                message.data["institution_introducion"] = institution.InstitutionIntroduction;
                message.data["institution_profile"] = institution.InstitutionProfile;
                message.data["institution_city"] = institution.InstitutionCity;
                message.data["institution_email"] = institution.InstitutionEmail;
                message.data["institution_characteristic"] = institution.InstitutionLessonsCharacter;
                message.data["institution_lessons"] = institution.InstitutionLessons;
                message.data["institution_createtime"] = institution.InstitutionCreatetime;
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }
    }
}
