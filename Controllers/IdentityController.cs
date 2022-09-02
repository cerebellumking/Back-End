using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using System.IO;
namespace Back_End.Controllers
{
    public class IdentityQualificationInfo
    {
        public int identity_id { get; set; }
        public string image { get; set; }
        public string university_name { get; set; }
        public string identity { get; set; }
        public string enrollment_time { get; set; }
        public string major { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ModelContext myContext;
        public IdentityController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpGet]
        public string getIdentityQualification(int user_id)
        {
            Message message = new();
            try
            {
                var list = myContext.Qualifications
                    .Where(b => b.UserId == user_id && b.Visible == true)
                    .Select(b => new IdentityQualificationInfo {identity_id=b.IdentityId ,enrollment_time = b.EnrollmentTime, identity = b.Identity, image = b.IdentityQualificationImage, major = b.Major, university_name = b.University.UniversityChName })
                    .ToList();
                message.errorCode = 200;
                message.status = true;
                message.data["identity_list"] = list.ToArray();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost]
        public string submitQualification(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                string img_base64 = front_end_data.GetProperty("img").ToString();
                Console.WriteLine(img_base64);
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                string identity = front_end_data.GetProperty("identity").ToString();
                string enrollment_time = front_end_data.GetProperty("enrollment_time").ToString();
                string major = front_end_data.GetProperty("major").ToString();
                string university_name = front_end_data.GetProperty("university_name").ToString();
                int id = myContext.Qualifications.Max(b=>b.IdentityId) + 1;
                //存储图片
                string type = "." + img_base64.Split(',')[0].Split(';')[0].Split('/')[1];
                img_base64 = img_base64.Split("base64,")[1];//非常重要
                byte[] img_bytes = Convert.FromBase64String(img_base64);
                var client = OssHelp.createClient();
                MemoryStream stream = new MemoryStream(img_bytes, 0, img_bytes.Length);
                string path = "identity/" + id.ToString() + type;
                client.PutObject(OssHelp.bucketName, path, stream);
                string imgurl = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path;
                //添加学历认证
                Qualification qualification = new();
                qualification.IdentityQualificationImage =imgurl;
                qualification.IdentityId = id;
                qualification.Major = major;
                University university = myContext.Universities.Single(b => b.UniversityChName == university_name);
                qualification.UniversityId = university.UniversityId;
                qualification.EnrollmentTime = enrollment_time;
                qualification.University = university;
                qualification.Visible = false;
                qualification.UserId = user_id;
                qualification.Identity = identity;
                qualification.User = myContext.Users.Single(b => b.UserId == user_id);
                //添加学历认证审核
                Qualificationchecking qualificationchecking = new();
                qualificationchecking.IdentityId = id;
                qualificationchecking.ReviewResult="待审核";
                qualificationchecking.SummitDate = DateTime.Now;
                qualificationchecking.AdministratorId = 0;
                qualification.Qualificationchecking = qualificationchecking;
                myContext.Qualificationcheckings.Add(qualificationchecking);
                myContext.Qualifications.Add(qualification);
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpDelete]
        public string deleteQualification(int identity_id)
        {
            Message message = new();
            try
            {
                myContext.DetachAll();
                Qualification qualification = myContext.Qualifications.Single(b => b.IdentityId == identity_id);
                qualification.Visible = false;
                myContext.SaveChanges();
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
