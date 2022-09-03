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
    public class InstitutionInfo
    {
        public int institution_id { get; set; }
        public string institution_profile { get; set; }
        public string institution_introduction { get; set; }

        public string institution_name { get; set; }
    }

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
                message.data["institution_photo"] = institution.InstitutionPhoto.Split(';');
                message.status = true;
                message.errorCode = 200;
            }
            catch
            {

            }
            return message.ReturnJson();
        }

        [HttpGet("list")]
        public string showInstitutionList(int page,int page_size=5,string institution_province="",string institution_city = "",string institution_target = "")
        {
            Message message = new Message();
            try
            {
                //url解码
                institution_province = System.Web.HttpUtility.UrlDecode(institution_province);
                institution_city = System.Web.HttpUtility.UrlDecode(institution_city);
                institution_target = System.Web.HttpUtility.UrlDecode(institution_target);
                List<InstitutionInfo> institutionInfos = new List<InstitutionInfo>();
                var institution_list = myContext.Institutions
                    .Where(b => b.InstitutionProvince.Contains(institution_province) && b.InstitutionCity.Contains(institution_city) && b.InstitutionTarget.Contains(institution_target))
                    .OrderBy(b=>b.InstitutionId)
                    .Skip(page_size * (page - 1))
                    .Take(page_size)
                    .ToList();
                foreach (Institution institution in institution_list)
                {
                    InstitutionInfo institutionInfo = new InstitutionInfo();
                    institutionInfo.institution_id = institution.InstitutionId;
                    string temp = institution.InstitutionIntroduction;
                    temp = temp.Substring(0, 140>temp.Length?temp.Length:140);
                    institutionInfo.institution_introduction = temp.Substring(0, temp.LastIndexOf('，')) + "......";
                    institutionInfo.institution_name = institution.InstitutionName;
                    institutionInfo.institution_profile = institution.InstitutionProfile;
                    institutionInfos.Add(institutionInfo);
                }
                message.data["institution_list"] = institutionInfos.ToArray();
                message.errorCode = 200;
                message.status = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpGet("num")]
        public string getNumber(string institution_province = "", string institution_city = "", string institution_target = "")
        {
            Message message = new Message();
            try
            {
                //url解码
                institution_province = System.Web.HttpUtility.UrlDecode(institution_province);
                institution_city = System.Web.HttpUtility.UrlDecode(institution_city);
                institution_target = System.Web.HttpUtility.UrlDecode(institution_target);
                var institution_list = myContext.Institutions
                    .Where(b => b.InstitutionProvince.Contains(institution_province) && b.InstitutionCity.Contains(institution_city) && b.InstitutionTarget.Contains(institution_target))
                    .OrderBy(b => b.InstitutionId)
                    .ToList();
                List<InstitutionInfo> institutionInfos = new List<InstitutionInfo>();
                foreach (Institution institution in institution_list)
                {
                    InstitutionInfo institutionInfo = new InstitutionInfo();
                    institutionInfo.institution_id = institution.InstitutionId;
                    institutionInfo.institution_name = institution.InstitutionName;
                    institutionInfos.Add(institutionInfo);
                }
                message.data["institution_list"] = institutionInfos.ToArray();
                message.data["num"] = institution_list.Count;
                message.errorCode = 200;
                message.status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }

        [HttpPost("change")]
        public string changeUniversityInfo()
        {
            Message message = new();
            try
            {
                int institution_id = int.Parse(Request.Form["id"]);
                Institution institution = myContext.Institutions.Single(b => b.InstitutionId == institution_id);

                // name
                string name = Request.Form["name"];
                if (name != null)
                {
                    institution.InstitutionName = name;
                }

                string phone = Request.Form["phone"];
                if (phone != null)
                {
                    institution.InstitutionPhone = phone;
                }


                string qualify = Request.Form["qualify"];
                if (qualify != null)
                {
                    institution.InstitutionQualify = qualify;
                }

                string introduction = Request.Form["introduction"];
                if (introduction != null)
                {
                    institution.InstitutionIntroduction = introduction;
                }

                string profile = Request.Form["profile"];
                if (profile != null)
                {
                    institution.InstitutionProfile = profile;
                }

                string city = Request.Form["city"];
                if (city != null)
                {
                    institution.InstitutionCity = city;
                }
                string target = Request.Form["target"];
                if (target != null)
                {
                    institution.InstitutionTarget = target;
                }
                string location = Request.Form["location"];
                if (location != null)
                {
                    institution.InstitutionLocation = location;
                }

                string email = Request.Form["email"];
                if (email != null)
                {
                    institution.InstitutionEmail = email;
                }


                string lessons_characteristic = Request.Form["lessons_characteristic"];
                if (lessons_characteristic != null)
                {
                    institution.InstitutionLessonsCharacter = lessons_characteristic;
                }
                string lessons = Request.Form["lessons"];
                if (lessons != null)
                {
                    institution.InstitutionLessons = lessons;
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

        [HttpPost]
        public string addInstitution(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                string institution_name = front_end_data.GetProperty("name").ToString();
                string institution_phone = front_end_data.GetProperty("phone").ToString();
                string institution_qualify = front_end_data.GetProperty("qualify").ToString();
                string institution_introduction = front_end_data.GetProperty("introduction").ToString();
                string institution_profile = front_end_data.GetProperty("profile").ToString();
                string institution_city = front_end_data.GetProperty("city").ToString();
                //string institution_target= front_end_data.GetProperty("target").ToString();
                string institution_location = front_end_data.GetProperty("location").ToString();
                string institution_email = front_end_data.GetProperty("email").ToString();
                string institution_lessons_characteristic = front_end_data.GetProperty("lessons_characteristic").ToString();
                string institution_lessons = front_end_data.GetProperty("lessons").ToString();
                DateTime institution_createtime=DateTime.Parse(front_end_data.GetProperty("createtime").ToString());
                myContext.DetachAll();
                Institution institution = new();
                institution.InstitutionName = institution_name;
                institution.InstitutionPhone = institution_phone;
                //institution.InstitutionQualify = institution_qualify;
                institution.InstitutionIntroduction = institution_introduction;
                //institution.InstitutionProfile = institution_profile;
                institution.InstitutionCity = institution_city;
                institution.InstitutionCreatetime = institution_createtime;
                //institution.InstitutionTarget = institution_target;
                institution.InstitutionLocation = institution_location;
                institution.InstitutionEmail = institution_email;
                institution.InstitutionLessonsCharacter = institution_lessons_characteristic;
                institution.InstitutionLessons = institution_lessons;
                //institution.InstitutionCreatetime = DateTime.Now;
                int id = 1, count = myContext.Institutions.Count();
                if(count != 0)
                {
                    id = myContext.Institutions.Select(b => b.InstitutionId).Max() + 1;

                }
                //添加图片
                string type1 = "." + institution_qualify.Split(',')[0].Split(';')[0].Split('/')[1];
                institution_qualify = institution_qualify.Split("base64,")[1];
                byte[] img_bytes_badge = Encoding.UTF8.GetBytes(institution_qualify);
                string type2 = "." + institution_profile.Split(',')[0].Split(';')[0].Split('/')[1];
                institution_profile = institution_profile.Split("base64,")[1];
                byte[] img_bytes_photo = Encoding.UTF8.GetBytes(institution_profile);
                var client = OssHelp.createClient();
                MemoryStream stream1 = new MemoryStream(img_bytes_badge, 0, img_bytes_badge.Length);
                MemoryStream stream2 = new MemoryStream(img_bytes_photo, 0, img_bytes_photo.Length);
                string path1 = "institution/qualify" + id.ToString() + type1;
                string path2 = "institution/profile" + id.ToString() + type2;
                string imageurl1 = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path1;
                string imageurl2 = "https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/" + path2;
                client.PutObject(OssHelp.bucketName, path1, stream1);
                client.PutObject(OssHelp.bucketName, path2, stream2);
                institution.InstitutionQualify = imageurl1;
                institution.InstitutionProfile = imageurl2;



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
