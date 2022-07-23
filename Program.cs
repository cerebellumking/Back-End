using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;
namespace Back_End
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Factory.SetOptions(GetConfig());
            CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //            //webBuilder.UseUrls("http://*:59230");
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            var url = configuration["Urls"];
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(url);
                    webBuilder.UseStartup<Startup>();
                });
        }

        static private Config GetConfig()
        {
            return new Config()
            {
                Protocol = "https",
                GatewayHost = "openapi.alipaydev.com",
                //GatewayHost = "openapi.alipaydev.com/gateway.do",
                SignType = "RSA2",

                AppId = "2021000121619670",

                // 为避免私钥随源码泄露，推荐从文件中读取私钥字符串而不是写入源码中
                MerchantPrivateKey = "MIIEpAIBAAKCAQEAommn+3UAchwBJdWtibQj3ehcgtBi6Pmk2iXvXKnlzBlbkh1l4hohv5jX1zsZb3ovELGFt9VgrUbOQT4yMQWl9E7jT7RGDxkj6oBCbxVWRMrrjuDB/QZNfGLoTENO2rQd5ZqJJCJCfOueko8aptFPzzbgvRGdDylmy9jftX+NlBkP5ktKmA5EW7vsDGms/Im9l/M33KoGqFDHrrn2nz/MjJAWlYV3xwyMZEjnVHV2JlPU59nYS2gGc+GWg4ANM3MFRWxsrEOapv/j4GPdcy62jHsppMag2Pq4SGFRIQjTC+YdKTXsIdmZitsMAC8V+DWfqQduOzHCQLq05gG2+YUWFQIDAQABAoIBACaFWMpTXd+ytLL8k0Rt7XPcgNSmCE+ppmf3R2Z7BX37JbTqNBgEiqye29K4ubevCyqycDzB50Qx3wmqbs0hwdLIQzHMWDGMjPA5q4gdT3DRkD07IceZjxdJfj9YV4OX5N9oX86qMYrfd6TbPD8EtiE3quYtH0tRTohaz/SEZFgTlqMzNPzs1Qo5SY+AGu3ghIXRM9kPiXBCAc6XnVC+jxa7BhO7U/96axhETC2G3BYbpagUpK/eOe2OUBoAxxOikkVQIrl5ifGC1HhEVXUZ8sqs1Mb1VR60CjoL3e6gGqWkpyIpPw2hY8TmuyFVA6IMs62He4HlebsT+F0kdUnZ/QECgYEA/XvyassnFT2twH18tT3edkDlkqAeke5kpwF9tesbdQczKTiLfK07eKpJjCYPWfqQ/u/3NPCrxzxUVeTdOy0CdEIQ31DyFYphHdF3ELU3N5OK5YHIRZIUes2EdpkD+Ei+SUw4KnN3HU+jFWJqq6NJ5S7jLq2zluTUfiHs1A1H4wkCgYEApAZQkhARce7IaM+4wLRV3KVabmOeyGTiNkZPsCmah2SFbbaL+EmrfAhFHPZ8Fij/pQaanxBgpcq32Q4WFpjK2mcOeZEi0oiUqWqc/mPbK6CJgZqXpvg494yuWC8/mKiNdrtjMBxC101cEE62PF1Y/aKriyhJ70UDAvfIKjrooa0CgYEAyFQ2p7PaNW1Dbkc+UbaNpANx68ljn406Ospw4V7nmjuQcVlg063MvHZIHdzOcRwXj4NyaYJzH8hMFn4DYYsWz26PCw5b5C4yxnAOm0O8Wc3fDbbd4i2qxcq6j0gq3QQQbrkwAkyyrJWNm47mIrOv6NJ/dRqOPgim7vA8zPGaCJECgYAeTmBirH9cqr7yxrafPHLE4vRzgwqagjFdz933cahwrb0NiXYROSNrNmf2swA4Y/jgN/knvLHu0Cbp/vVL1Y1djz8YbR2fAsddCirZwf+D7xSZEsREgIaeDqtD9e7tOO9yaaso/3/GIjAJf1Gik6jWrFC/1IDcqcalrLyaOOo0vQKBgQDKfMq0dCNj1435x8T+oOwlIpYDhx82kHEC+6eDFpbEEZUfl4mFcNggDAXA66kBT1ypuBuRNWZhojQnzuTMKR37QNaZL8QMAbxjWTGBQRAhDZnyMNKqyXTsMxbDeFTgphbTdFsoxpaa/xAdo9tsna1JRk5voJ+LwZnkDHjRZ+BNMA==",

                // 如果采用非证书模式，则无需赋值上面的三个证书路径，改为赋值如下的支付宝公钥字符串即可
                AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxn3YczlkG4BDA1LLr2T2rAL0sjYOx36ENJ58h7yYg0wdagfzesRnbR999AIZIhR/A74LZhtMLr3cT6z0Dap0DBdgOBIh3s33mPAr0gPCbY1gB2VDmwqOGLaPjPixCmbesV+zcahWEmZLrRqj0T7WwO2pYgfOjLQqDiKUeJaJK8JTAKMvDYMTV5DJnOKhpMiODtr2ph/FtAc6TWPxddbNl+3m2ph26ZZHBDC+Jpyv+h5a9eO2Bldv/M8OyHrt4IFHE8TwBE4go4MMgzzCjaN2RMmz6tTVWgQ7Hnz8IZiWBlmbAzGD1ClhDEjq3mWP9n7XBIC0mHd1j6bZkfULElJN7wIDAQAB",

                //可设置异步通知接收服务地址（可选）
                NotifyUrl = "http://43.142.41.192:54686/#/home"

                //可设置AES密钥，调用AES加解密相关接口时需要（可选）
                // EncryptKey = "<-- 请填写您的AES密钥，例如：aa4BtZ4tspm2wnXLb1ThQA== -->"
            };
        }
    }
}
