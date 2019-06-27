using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RESTWebRequest
{
    public class RESTWebRequest
    {
        public static string GET(string Uri, string Authorization = "")
        {
            HttpWebRequest accessTokenRequest = (HttpWebRequest)WebRequest.Create(Uri);
            accessTokenRequest.Method = "GET";

            if(!string.IsNullOrEmpty(Authorization))
            {
                accessTokenRequest.Headers.Add(HttpRequestHeader.Authorization, Authorization);
            }

            string responseString = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)accessTokenRequest.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        //Console.WriteLine(responseString);

                    }
                }
            }
            catch (Exception)
            {
                responseString = string.Empty;
            }

            return responseString;
            
        }
    }
}
