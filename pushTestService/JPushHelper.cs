using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace pushTestService
{
    public class JPushHelper
    {
        private const string API_APPKEY = "b11d787636e6b64c8982d782";
        private const string API_MASTERSECRET = "35a9d55dcb40819395dd2e8d";

        public static string BroadcastNotification(int sendno, string content, bool isJsonFormat = true)
        {
            int receiverType = 4;

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sendno", sendno.ToString());
            parameters.Add("app_key", API_APPKEY);
            parameters.Add("receiver_type", receiverType.ToString());
            string verificationCode = MD5(sendno.ToString() + receiverType.ToString() + API_MASTERSECRET).ToUpper();
            parameters.Add("verification_code", verificationCode);
            parameters.Add("msg_type", "1");
            if (isJsonFormat)
            {
                parameters.Add("msg_content", "{\"n_content\":" + content + "}");
            }
            else
            {
                parameters.Add("msg_content", "{\"n_content\":\"" + content + "\"}");
            }
            parameters.Add("platform", "android");
            return postApi(parameters);
        }

        public static string SendNotification(int sendno, string sendAlias, string content, bool isJsonFormat = true)
        {
            int receiverType = 3;
            string receiverValue = sendAlias;

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sendno", sendno.ToString());
            parameters.Add("app_key", API_APPKEY);
            parameters.Add("receiver_type", receiverType.ToString());
            parameters.Add("receiver_value", receiverValue.ToString());
            string verificationCode = MD5(sendno.ToString() + receiverType.ToString() + receiverValue + API_MASTERSECRET).ToUpper();
            parameters.Add("verification_code", verificationCode);
            parameters.Add("msg_type", "1");
            if (isJsonFormat)
            {
                parameters.Add("msg_content", "{\"n_content\":" + content + "}");
            }
            else
            {
                parameters.Add("msg_content", "{\"n_content\":\"" + content + "\"}");
            }
            parameters.Add("platform", "android");
            return postApi(parameters);
        }


        private const string API_URL = "http://api.jpush.cn:8800/sendmsg/v2/sendmsg";

        private static string postApi(IDictionary<string, string> parameters)
        {
            string content = string.Empty;
            HttpWebRequest request = WebRequest.Create(API_URL) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response != null)
            {
                Stream receiveStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    receiveStream = new GZipStream(receiveStream, CompressionMode.Decompress);

                }
                content = new StreamReader(receiveStream).ReadToEnd();
            }
            return content;
        }

        public static string MD5(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            byte[] hashValue = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                sb.Append(hashValue[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}