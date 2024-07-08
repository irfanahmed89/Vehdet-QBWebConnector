using System.Net;
using System;
using System.Reflection.Metadata;
using System.IO;
using System.Threading.Channels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace WebApplication.Sample
{
    public class HttpJson
    {
        public static string AuthorizationToken = "";
        public static string APIUrls = "";
       
        static HttpJson()
        {
            try
            {
                APIUrls = "";
                AuthorizationToken = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Authorization"];
                //AuthorizationToken = Utils.ReadConfig(Constant.QBCompanyName, isToken: true);
                Common.WriteToFile("Frappe -> API Authorizatoin: " + AuthorizationToken);
            }
            catch (Exception exp)
            {

            }
        }

        public string Request(string endpoint, string action = "GET", string body = null)
        {
            string response = "";
            try
            {
                // LoggingFactory.Debug(Utils.GetCurrentMethodName() + endpoint.ToString());

                string url = APIUrls + endpoint;

                HttpWebRequest call = (HttpWebRequest)WebRequest.Create(url);
                foreach (string key in call.Headers.Keys)
                    call.Headers.Remove(key);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                call.ServicePoint.Expect100Continue = false;

                call.ContentType = "application/json";
                call.Method = action;
                
                call.Headers.Add("Authorization", AuthorizationToken);
                
                if (action == "POST" || action == "PUT")
                    using (var StreamWriter = new StreamWriter(call.GetRequestStream()))
                    {
                        StreamWriter.Write(body);
                    }

                HttpWebResponse ObjHttpResponse = (HttpWebResponse)call.GetResponse();
                StreamReader reader = new StreamReader(ObjHttpResponse.GetResponseStream());               
                response = reader.ReadToEnd();
               
            }
            catch (WebException ex)
            {
                Common.WriteToFile("Frappe -> API Response Error: " + ex.Message + "\n endpoint: " + endpoint +"\n Method: "+ action + "\nBody: " + body);
            }
            return response;
        }

        public string Request3(string endpoint, string action = "GET", Dictionary<string, string> formData = null)
        {
            string response = "";
            try
            {
                string url = APIUrls + endpoint;

                HttpWebRequest call = (HttpWebRequest)WebRequest.Create(url);
                foreach (string key in call.Headers.Keys)
                    call.Headers.Remove(key);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                call.ServicePoint.Expect100Continue = false;

                // Use "application/x-www-form-urlencoded" for form data
                call.ContentType = "application/x-www-form-urlencoded";

                call.Method = action;

                call.Headers.Add("Authorization", AuthorizationToken);

                if ((action == "POST" || action == "PUT") && formData != null)
                {
                    // Convert the dictionary to a query string
                    string queryString = string.Join("&", formData.Select(pair => $"{WebUtility.UrlEncode(pair.Key)}={WebUtility.UrlEncode(pair.Value)}"));

                    using (var streamWriter = new StreamWriter(call.GetRequestStream()))
                    {
                        streamWriter.Write(queryString);
                    }
                }

                HttpWebResponse ObjHttpResponse = (HttpWebResponse)call.GetResponse();
                StreamReader reader = new StreamReader(ObjHttpResponse.GetResponseStream());
                response = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                // Handle exception
            }
            return response;
        }
    }
}
