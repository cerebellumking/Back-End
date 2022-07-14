using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MessageInfo
    {
        public MessageInfo()
        {
            userID = 0;
            content = "";
            image = "";
        }
        public int userID { get; set; }

        public string content { get; set; }

        public string image { get; set; }

    }

    public class MessageRecord
    {
        //public int userID { get; set; }

        //public string content { get; set; }

        //public string image { get; set; }

        //public DateTime time { get; set; }

        //public MessageInfo te { get; set; }
        public MessageRecord()
        {
            user_info.Add("user_id", 0);
            user_info.Add("user_name", "");
        }
        public Dictionary<string, dynamic> user_info { get; set; } = new Dictionary<string, dynamic>();
        public bool state { get; set; }
    }

    public class MessageController : ControllerBase
    {
        /// <summary>
        /// 该接口接收用户ID、目标用户ID、文本内容并返回私信是否成功发送
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="targetUserID"></param>
        /// <param name="image"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("send")]
        
        public string sendMessage(int userID,int targetUserID,string image="",string content="")
        {
            return JsonSerializer.Serialize("success");
        }
        /// <summary>
        /// 该接口接收用户ID、目标用户ID、私信时间并返回私信是否记录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="targetUserID"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost("tempSave")]
        public string tempSaveMessage(int userID, int targetUserID,DateTime time)
        {
            return JsonSerializer.Serialize("success");
        }
        /// <summary>
        /// 该接口接收用户ID、目标用户ID、私信时间并返回私信记录是否删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public string deleteMessageRecord(int userID, int targetUserID, DateTime time)
        {
            return JsonSerializer.Serialize("success");
        }
        /// <summary>
        /// 该接口接收用户ID并按时间排序返回私信、私信者信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("receive")]
        public string receiveMessage(int userID)
        {
            MessageInfo test=new MessageInfo();
            test.userID = 123223;
            test.content = "hello";
            test.image = "/img/hello.png";
            return JsonSerializer.Serialize(test);
        }
        /// <summary>
        /// 该接口接收用户ID、目标用户ID并按照时间顺序返回私信内容、私信时间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="targetUserID"></param>
        /// <returns></returns>
        [HttpPost("retrive")]
        public string retriveMessage(dynamic front_end_data)
        {
            MessageRecord test = new MessageRecord();
            int flag = int.Parse(front_end_data.GetProperty("flag").ToString());
            int userID = int.Parse(front_end_data.GetProperty("userID").ToString());
            int targetUserID = int.Parse(front_end_data.GetProperty("targetUserID").ToString());

            if (flag == 1)
            {
                test.user_info.Add("list", new List<MessageInfo>());
                List<MessageInfo> a=new List<MessageInfo>();
                MessageInfo aa=new MessageInfo();
                MessageInfo bb = new MessageInfo();
                aa.userID = 12;
                bb.userID = 11;
                a.Add(aa);
                a.Add(bb);
                test.user_info["list"] = a;
                test.user_info["user_id"] = 1000;
                test.user_info["user_name"] = "hello";
                //test.te.userID = 1333;
                //test.te.image = "/img/hello.png";
                //test.te.content = "hello";
                //test.userID = 123223;
                //test.content = "hello";
                //test.image = "/img/hello.png";
                //test.time = DateTime.Now;
                test.state = true;
            }
            else
            {
                test.state = false;
            }
            return JsonSerializer.Serialize(test);
        }
    }
}
