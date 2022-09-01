using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;

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

        [HttpPost]
        public string addInstitution(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                string institution_name = front_end_data.GetProperty("institution_name").ToString();
                string institution_phone = front_end_data.GetProperty("institution_phone").ToString();
                string institution_qualify = front_end_data.GetProperty("institution_qualify").ToString();
                string institution_introduction = front_end_data.GetProperty("institution_introduction").ToString();
                string institution_profile = front_end_data.GetProperty("institution_profile").ToString();
                string institution_city = front_end_data.GetProperty("institution_city").ToString();
                string institution_location = front_end_data.GetProperty("institution_location").ToString();
                string institution_email = front_end_data.GetProperty("institution_email").ToString();
                string institution_lessons_characteristic = front_end_data.GetProperty("institution_lessons_characteristic").ToString();
                string institution_lessons = front_end_data.GetProperty("institution_lessons").ToString();

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
