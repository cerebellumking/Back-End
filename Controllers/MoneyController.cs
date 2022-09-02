using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;
using Alipay.EasySDK.Kernel.Util;
using Alipay.EasySDK.Payment.Common.Models;
using Alipay.EasySDK.Payment.FaceToFace.Models;
using Alipay.EasySDK.Payment.Page.Models;
using System.Text.Json;
namespace Back_End.Controllers
{
    public class RecordInfo
    {
        public int change_num { get; set; }
        public string change_reason { get; set; }
        public DateTime change_time { get; set;}
    }
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
                message.data["user_coin"] = user.UserCoin;
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
        [HttpGet("record")]
        public string getRecord(int user_id)
        {
            Message message = new();
            try
            {
                List<RecordInfo> recordInfos = new();
                //用户充值、登陆领取鸟币
                var list1 = myContext.Moneychangerecords
                    .Where(b => b.UserId == user_id)
                    .Select(b => new RecordInfo { change_num = b.ChangeNum, change_time = (DateTime)b.ChangeDate, change_reason = b.ChangeReason })
                    .ToList();
                recordInfos.AddRange(list1);
                //用户动态收获鸟币
                var list2 = myContext.Coinblogs
                    .Where(b => b.Blog.BlogUserId == user_id)
                    .ToList();
                foreach(var coin_blog in list2)
                {
                    RecordInfo record = new();
                    record.change_reason = "用户打赏动态"+coin_blog.BlogId.ToString();
                    record.change_num = coin_blog.CoinNum;
                    record.change_time = coin_blog.CoinTime;
                    recordInfos.Add(record);
                }
                //用户回答收获鸟币
                var list3 = myContext.Coinanswers
                    .Where(b => b.Answer.AnswerUserId == user_id)
                    .ToList();
                foreach (var coin_answer in list3)
                {
                    RecordInfo record = new();
                    record.change_reason = "用户打赏回答"+coin_answer.AnswerId.ToString();
                    record.change_num = coin_answer.CoinNum;
                    record.change_time = coin_answer.CoinTime;
                    recordInfos.Add(record);
                }
                //用户回答收获悬赏鸟币
                var list4 = myContext.Answers
                    .Where(b => b.Question.QuestionApply == b.AnswerId&&b.AnswerUserId==user_id)
                    .Select(b => new RecordInfo { change_num = (int)b.Question.QuestionReward, change_time = (DateTime)b.AnswerDate, change_reason = "回答"+b.AnswerId.ToString()+"获得最佳回答奖励" })
                    .ToList();
                recordInfos.AddRange(list4);
                //用户提问悬赏鸟币
                var list5 = myContext.Questions
                   .Where(b => b.QuestionUserId ==user_id)
                   .Select(b => new RecordInfo { change_num = -(int)b.QuestionReward, change_time = (DateTime)b.QuestionDate, change_reason = "问题"+b.QuestionId.ToString()+"提问悬赏" })
                   .ToList();
                recordInfos.AddRange(list5);
                //用户给动态投币
                var list6 = myContext.Coinblogs
                    .Where(b => b.UserId == user_id)
                    .Select(b=>new RecordInfo { change_num=-(int)b.CoinNum,change_time=b.CoinTime,change_reason="给动态"+b.BlogId.ToString()+"投币"})
                    .ToList();
                recordInfos.AddRange(list6);
                //用户给回答投币
                var list7 = myContext.Coinanswers
                   .Where(b => b.UserId == user_id)
                   .Select(b => new RecordInfo { change_num = -(int)b.CoinNum, change_time = b.CoinTime, change_reason = "给回答" + b.AnswerId.ToString() + "投币" })
                   .ToList();
                recordInfos.AddRange(list7);

                message.errorCode = 200;
                message.status = true;
                User user = myContext.Users.Single(b => b.UserId == user_id);
                message.data["coin"] = user.UserCoin;
                message.data["record_list"] = recordInfos.ToArray();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPost("check")]
        public string checkOrder()
        {
            Message message = new();
            try
            {
                string out_trade_num = Request.Form["out_trade_no"];
                int user_id = int.Parse(out_trade_num.Split('U')[1]);
                Console.WriteLine(user_id);
                AlipayTradeQueryResponse alipayTradeQueryResponse = Factory.Payment.Common().Query(out_trade_num);
                Console.WriteLine(alipayTradeQueryResponse.TradeStatus);
                Console.WriteLine(alipayTradeQueryResponse.TotalAmount);
                if (alipayTradeQueryResponse.TradeStatus == "TRADE_SUCCESS")
                {
                    myContext.DetachAll();
                    Moneychangerecord moneychangerecord = new();
                    moneychangerecord.RecordId = myContext.Moneychangerecords.Max(b=>b.RecordId) + 1;
                    moneychangerecord.UserId = user_id;
                    moneychangerecord.ChangeNum = int.Parse(alipayTradeQueryResponse.TotalAmount.Split('.')[0])*10;
                    moneychangerecord.ChangeDate = DateTime.Now;
                    moneychangerecord.ChangeReason = "充值" + (moneychangerecord.ChangeNum).ToString() + "枚鸟币";
                    User user = myContext.Users.Single(b => b.UserId == user_id);
                    user.UserCoin += moneychangerecord.ChangeNum;
                    myContext.Add(moneychangerecord);
                    myContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
        }
        [HttpPost("order")]
        public string createOrder(dynamic front_end_data)
        {
            Message message = new();
            try
            {
                string user_id = front_end_data.GetProperty("user_id").ToString();
                string num = front_end_data.GetProperty("num").ToString();
                string name = front_end_data.GetProperty("name").ToString();
                string order_num = DateTime.Now.ToShortDateString().Replace('/','T')+ DateTime.Now.ToShortTimeString().Replace(':', 'T')+DateTime.Now.Second.ToString()+"U"+user_id.ToString();
                order_num = order_num.Replace(' ', 'T');
                Console.WriteLine(order_num);
                AlipayTradePagePayResponse response = Factory.Payment.Page()
                    .AsyncNotify("http://43.142.41.192:6001/api/money/check")
                    .Pay(name, order_num, num, "http://www.houniaoliuxue.xyz/#/person_info");
                // 处理响应或异常
                if (ResponseChecker.Success(response))
                {
                    return response.Body;
                }
                else
                {
                    message.errorCode = 200;
                    message.status = false;
                }
                //Console.WriteLine(order_num);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return message.ReturnJson();
            //return message.ReturnJson();
        }
    }
}
