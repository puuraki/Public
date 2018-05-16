using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace http_server
{
    class Program
    {
        private static string url = "index";

        static void Main(string[] args)
        {
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/" + url + "/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }

        public static string SendResponse(HttpListenerRequest request)
        {

            Console.WriteLine("Request method: " + request.HttpMethod);
            Console.WriteLine("Content type: " + request.ContentType);
            if (request.HttpMethod == "GET")
            {
                return getHandler(request);
            }
            else
            {
                return postHandler(request);
            }
        }

        public static string getHandler(HttpListenerRequest request)
        {
            string content = request.QueryString["page"];
            if (content == "" || content == null)
            {
                content = "index";
            }
            return File.ReadAllText(string.Format(@"{0}.html", content));
        }

        public static string postHandler(HttpListenerRequest request)
        {
            Hashtable formVars = new Hashtable();
            string result;

            if (request.HasEntityBody)
        	{
                using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string content = reader.ReadToEnd();
                    content = content.Replace("\r", "");
                    string[] pairs = content.Split('\n');
			        for (int x = 0; x < pairs.Length; x++)
			        {
                        Console.WriteLine(pairs[x]);
				        string[] item = pairs[x].Split('=');
				        formVars.Add(item[0],System.Web.HttpUtility.UrlDecode(item[1]));
			        }
                }
            }
            result = string.Format("Welcome {0}, your email is {1}", formVars["name"], formVars["email"]);
            Console.WriteLine(result);
            return result;
        }

    }
}
