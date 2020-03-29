using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace com.pinkuburu.robot.Code.application.JIn10
{
    class GetURLContent
    {
        /// <summary>
        /// C#使用GZIP解压缩完整读取网页内容 Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public string GetHtmlWithUtf(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            #region 示例代码
            //request.Host = "qun.qq.com";
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
            request.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            string cookie = "onNotification=1; onSound=1; x-token=; UM_distinctid=170e62a684e93e-0c49e70e3945b1-4313f6a-384000-170e62a684f838; trend=1; onNight=1; kind_g=%5B%223%22%2C%227%22%5D; onTW=2; CNZZDATA1254715939=1416986378-1584541774-null%7C1584585204; CNZZDATA1000171913=214675370-1584405689-%7C1584600129";
            request.Headers.Add(HttpRequestHeader.Cookie, cookie);
            request.ContentType = "application/x-javascript;charset=UTF-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
            string sHTML = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
            }
            return sHTML;
            #endregion             
        }
    }
}
