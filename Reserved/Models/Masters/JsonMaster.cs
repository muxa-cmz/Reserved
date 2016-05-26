using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Reserved.Models.Masters
{
    public class JsonMaster
    {
        public string GetJSON(string url, Dictionary<String, String> parameters)
        {
            StringBuilder paramSrting = new StringBuilder();

            if (parameters.Count != 0)
            {
                foreach (var parameter in parameters)
                {
                    paramSrting.Append(parameter.Key)
                        .Append("=")
                        .Append(parameter.Value)
                        .Append("&");
                }
                paramSrting.Remove(paramSrting.Length - 2, paramSrting.Length - 1);
            }

            // Start the request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StreamWriter streamWriter = new StreamWriter(req.GetRequestStream());
            streamWriter.Write(paramSrting.ToString());
            streamWriter.Flush();
            streamWriter.Close();
            // Get the response
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader streamReader = new StreamReader(res.GetResponseStream());
            string result = streamReader.ReadToEnd();
            return result;
        }
    }
}