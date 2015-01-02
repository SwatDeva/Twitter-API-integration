using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace TwiiterFeed
{
    internal class Program
    {
        public static string query = "Deva_Palanisamy";
        //    public string url = "https://api.twitter.com/1.1/users/search.json" ;
        public static string url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
 
 
        private static void Main(string[] args)
        {

            //var request = (HttpWebRequest) WebRequest.Create("http://www.yahoo.com");

            //var response = (HttpWebResponse) request.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //Console.WriteLine("This is the output: " + responseString);

            Program program = new Program();
            program.findusertwitter(url,query);
        }

        public void findusertwitter(string resource_url, string q)
        { // oauth application keys
            var oauth_token = " 2857585397-srusMuxRLBuyxv1kXchZfJKzoiqwN34QSjWD08S";
            var oauth_token_secret = "00SrNVevAUgaCH4uuAFbbDdGjrVcaUTaikxf5E8VwvWyn";
            var oauth_consumer_key = "fXAjp8H6mVUmuNsG582tsYnkj";
            var oauth_consumer_secret = "1NskhR8QjICtAfkwCwwlnWHltDEWYyN5zkaGK6XHnl6NAvSMe3";

            // oauth implementation details
            var  oauth_version  = "1.1"; var  oauth_signature_method  = "HMAC-SHA1";

            // unique request details
            var  oauth_nonce  = Convert  . ToBase64String  ( new ASCIIEncoding  (). GetBytes  ( DateTime  . Now  .
            Ticks  . ToString  ())); 
            var  timeSpan  = DateTime  .UtcNow - new DateTime  (1970, 1, 1, 0, 0, 0, 0, DateTimeKind  .Utc  );
            var  oauth_timestamp  = Convert  .ToInt64  ( timeSpan  . TotalSeconds  ). ToString  ();
            //var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            //var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);


            // create oauth signature
             var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                        "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";
 
            var baseString = string.Format(baseFormat,
                                    oauth_consumer_key,
                                    oauth_nonce,
                                    oauth_signature_method,
                                    oauth_timestamp,
                                    oauth_token,
                                    oauth_version,
                                    Uri.EscapeDataString(q)
                                    );

        baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));
 
        var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                "&", Uri.EscapeDataString(oauth_token_secret));
 
        string oauth_signature;
        using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
        {
            oauth_signature = Convert.ToBase64String(
                hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
        }

        // create the request header
        var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                             "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                             "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                             "oauth_version=\"{6}\"";

        var authHeader = string.Format(headerFormat,
                                Uri.EscapeDataString(oauth_nonce),
                                Uri.EscapeDataString(oauth_signature_method),
                                Uri.EscapeDataString(oauth_timestamp),
                                Uri.EscapeDataString(oauth_consumer_key),
                                Uri.EscapeDataString(oauth_token),
                                Uri.EscapeDataString(oauth_signature),
                                Uri.EscapeDataString(oauth_version)
                        );

        ServicePointManager.Expect100Continue = false;

        // make the request
        var postBody = "screen_name=" + Uri.EscapeDataString(q);//
        resource_url += "?" + postBody;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
        request.Headers.Add("Authorization", authHeader);
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";
        var response = (HttpWebResponse)request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //var reader = new StreamReader(response.GetResponseStream());
        //var objText = reader.ReadToEnd();
        Console.WriteLine("response String " + responseString);
        string html = "";
        try
        {
            JArray jsonDat = JArray.Parse(responseString);
            for (int x = 0; x < jsonDat.Count(); x++)
            {
                //html += jsonDat[x]["id"].ToString() + "<br/>";
                html += jsonDat[x]["text"].ToString() + "<br/>";
                // html += jsonDat[x]["name"].ToString() + "<br/>";
                html += jsonDat[x]["created_at"].ToString() + "<br/>";

            }
            Console.WriteLine("Inner Html " + html);
        }
        catch (Exception twit_error)
        {
            Console.WriteLine("twitter error " + html + twit_error.ToString());
        }
    }


}
}
