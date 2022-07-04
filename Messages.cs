using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End
{
    public class Message
    {
        public int errorCode { get; set; }
        public bool status { get; set; }
        public Dictionary<string, dynamic> data { get; set; } = new Dictionary<string, dynamic>();

        public string ReturnJson()
        {
            return JsonSerializer.Serialize(this);
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
            //data.Add("true_user_id", null);
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





}
