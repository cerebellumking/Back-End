using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace Back_End
{
    public class OssHelp
    {
        public static IConfiguration _Configuration { get; set; }//读取配置文件

        static OssHelp()
        {
            _Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
        }

        public const string bucketName = "houniaoliuxue";
        public static OssClient createClient()
        {
            string accessKeyId = _Configuration["AccessKey"];
            string accessKeySecret = _Configuration["AccessPassword"];
            const string endpoint = "http://oss-cn-shanghai.aliyuncs.com";
            return new OssClient(endpoint, accessKeyId, accessKeySecret);
        }
        public static string uploadImage(Stream text,string path)
        {
            string accessKeyId = _Configuration["AccessKey"];
            string accessKeySecret = _Configuration["AccessPassword"];
            const string endpoint = "http://oss-cn-shanghai.aliyuncs.com";
            const string bucketName = "houniaoliuxue";
            var filebyte = StreamHelp.StreamToBytes(text);
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
            client.PutObject(bucketName, path, stream);
            string imgurl ="https://houniaoliuxue.oss-cn-shanghai.aliyuncs.com/"+path;
            return imgurl;
        }
    }
}
