using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;

namespace Back_End.Controllers
{

    public class IdentityInfo
    {
        //public int user_id { get; set; }
        public string identity { get; set; }
        public string university_name { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly ModelContext myContext;
        public UserInfoController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getInfo(int user_id)
        {
            Message message = new Message();
            try
            {
                var qualificationList = myContext.Qualifications.Where(a=>a.UserId==user_id&&a.Visible==true).Select(b => new { b.UserId, b.Identity, b.UniversityId }).ToList();
                int count = 1;
                message.data.Add("user_id", user_id);
                foreach (var qualification in qualificationList)
                {
                    IdentityInfo identityInfo = new IdentityInfo();
                    //identityInfo.user_id = qualification.UserId;
                    identityInfo.identity = qualification.Identity;
                    University university = myContext.Universities.Single(b => b.UniversityId == qualification.UniversityId);
                    identityInfo.university_name = university.UniversityName;
                    message.data.Add("identity" + count.ToString(), identityInfo);
                    count++;
                }
                //Console.WriteLine(count);
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {
            }
            return message.ReturnJson();
        }

        [HttpPut("change")]
        public void changeInfo()
        {

        }

        [HttpDelete]
        public void deleteInfo()
        {

        }

        [HttpPut]
        public void changeVerification()
        {

        }

    }
}
