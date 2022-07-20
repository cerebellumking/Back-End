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
    public class MoneyController : ControllerBase
    {
        private readonly ModelContext myContext;
        public MoneyController(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        [HttpPost]
        public string addMoney(dynamic front_end_data)
        {
            Message message = new Message();
            try
            {
                myContext.DetachAll();
                int user_id = int.Parse(front_end_data.GetProperty("user_id").ToString());
                int num = int.Parse(front_end_data.GetProperty("num").ToString());
                User user = myContext.Users.Single(b => b.UserId == user_id);
                //支付且钱包为空
                if (user.UserCoin == 0 && num < 0)
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 2;
                    message.data["user_coin"] = 0;
                    return message.ReturnJson();
                }
                //支付且余额不足
                else if ((user.UserCoin+num)<0)
                {
                    message.errorCode = 200;
                    message.status = false;
                    message.data["error"] = 1;
                    message.data["user_coin"] = user.UserCoin;
                    return message.ReturnJson();
                }
                user.UserCoin += num;
                myContext.SaveChanges();
                message.status = true;
                message.errorCode = 200;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
           
            return message.ReturnJson();
        }
        [HttpPost("record")]
        public void getRecord() { }
    }
}
