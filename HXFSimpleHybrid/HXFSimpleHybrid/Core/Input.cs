using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace HXFSimpleHybrid.Core
{
    class Input
    {
        public static Dictionary<string, string> Deserialize(string input, bool decodeHtml = true)
        {
            Dictionary<string, string> postParams = new Dictionary<string, string>();
            string[] rawParams = input.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = "";
                if (decodeHtml)
                {
                    value = HttpUtility.UrlDecode(kvPair[1]);
                }
                else
                {
                    value = kvPair[1];
                }

                postParams.Add(key, value);
            }

            return postParams;
        }

        public static string ReadStream(Stream x, Encoding y)
        {
            //Stream body = request.InputStream;
            //System.Text.Encoding encoding = request.ContentEncoding;
            StreamReader reader = new StreamReader(x, y);
            string q = reader.ReadToEnd();

            reader.Close();

            return q;
        }

        public static dynamic JsonDecode(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
