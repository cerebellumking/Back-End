using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace Back_End
{
    public class Message
    {
        public int errorCode { get; set; }
        public bool status { get; set; }
        public Dictionary<string, dynamic> data { get; set; } = new Dictionary<string, dynamic>();

        public Message()
        {
            errorCode = 300;
            status = false;
        }
        public string ReturnJson()
        {
            var options = new JsonSerializerOptions
            {
                //Encoder = JavaScriptEncoder.Create(UnicodeRanges.CjkUnifiedIdeographs, UnicodeRanges.CjkUnifiedIdeographsExtensionA),
                //Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All,UnicodeRanges.All),
            };
            return JsonSerializer.Serialize(this,options);
        }
    }

    public class RegisterMessage : Message
    {
        public RegisterMessage()
        {
            data.Add("user_id", null);
            errorCode = 300;
            status = false;
        }
    }

    public class LoginMessage : Message
    {
        public LoginMessage()
        {
            errorCode = 300;
            status = false;
            data.Add("user_id", 0);
            data.Add("user_password", "none");
            data.Add("user_email", "none");
            data.Add("user_phone", "none");
            data.Add("user_name", null);
            data.Add("user_profile", null);
            data.Add("user_createtime", null);
            data.Add("user_birthday",null);
            data.Add("user_gender", null);
            data.Add("user_state", null);
            data.Add("user_signature", null);
            data.Add("user_follower", null);
            data.Add("user_follows", null);
            data.Add("user_level", null);
            data.Add("user_coin", null);
        }
    }

    public class FollowMessage : Message
    {
        public FollowMessage()
        {
            errorCode = 300;
            status = false;
        }
    }

    public class UniversityMessage : Message
    {
        public UniversityMessage()
        {
            errorCode = 500;
            status = false;
            data.Add("university_id", 0);
            data.Add("university_email", null);
            data.Add("university_name", null);
            data.Add("university_region", null);
            data.Add("university_country", null);
            data.Add("university_location", null);
            data.Add("university_introduction", null);
            data.Add("university_student_num", null);
            data.Add("university_website", null);
            data.Add("university_college", null);
            data.Add("university_abbreviation", null);
            data.Add("university_QS_rank", null);
            data.Add("university_THE_rank", null);
            data.Add("university_USNews_rank", null);
            data.Add("rank_year", null);
        }
    }



}
