using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.IO;

namespace Back_End
{
    public class OssHelp
    {
        public const string bucketName = "houniaoliuxue";
        public static OssClient createClient()
        {
            const string accessKeyId = "LTAI5tNHm9vkUTD9WshKKhvQ";
            const string accessKeySecret = "wwaOqFeNa3iwkETmcIdYYCkweyAhAx";
            const string endpoint = "http://oss-cn-shanghai.aliyuncs.com";
            return new OssClient(endpoint, accessKeyId, accessKeySecret);
        }
        public static string uploadImage(Stream text,string path)
        {
            const string accessKeyId = "LTAI5tNHm9vkUTD9WshKKhvQ";
            const string accessKeySecret = "wwaOqFeNa3iwkETmcIdYYCkweyAhAx";
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
