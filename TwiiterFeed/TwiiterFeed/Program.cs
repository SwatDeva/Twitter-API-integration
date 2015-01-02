using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using LinqToTwitter;
using System.Configuration;


namespace TwiiterFeed
{
    internal class Program
    {
        public static string query = "Deva_Palanisamy";
        //    public string url = "https://api.twitter.com/1.1/users/search.json" ;
        public static string url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

        private string API_key = "fXAjp8H6mVUmuNsG582tsYnkj";
        private string API_secret = "1NskhR8QjICtAfkwCwwlnWHltDEWYyN5zkaGK6XHnl6NAvSMe3";
        private string Access_token_secret = "2857585397-srusMuxRLBuyxv1kXchZfJKzoiqwN34QSjWD08S"; 
        private string Access_token = "00SrNVevAUgaCH4uuAFbbDdGjrVcaUTaikxf5E8VwvWyn"; 
 
 
        private static void Main(string[] args)
        {

            // This is a super simple example that
            // retrieves the latest tweets of a given
            // twitter user.

            // SECTION A: Initialise local variables
            Console.WriteLine("SECTION A: Initialise local variables");

            // Access token goes here .. (Please generate your own)
            const string accessToken = "00SrNVevAUgaCH4uuAFbbDdGjrVcaUTaikxf5E8VwvWyn";
            // Access token secret goes here .. (Please generate your own)
            const string accessTokenSecret = "2857585397-srusMuxRLBuyxv1kXchZfJKzoiqwN34QSjWD08S"; 

            // Api key goes here .. (Please generate your own)
            const string consumerKey = "fXAjp8H6mVUmuNsG582tsYnkj";
            // Api secret goes here .. (Please generate your own)
            const string consumerSecret = "1NskhR8QjICtAfkwCwwlnWHltDEWYyN5zkaGK6XHnl6NAvSMe3";

            // The twitter account name goes here
            const string twitterAccountToDisplay = "Deva_Palanisamy";


            // SECTION B: Setup Single User Authorisation
            Console.WriteLine("SECTION B: Setup Single User Authorisation");
            var authorizer = new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret,
                    OAuthToken = accessToken,
                    OAuthTokenSecret = accessTokenSecret
                }
            };

            // SECTION C: Generate the Twitter Context
            Console.WriteLine("SECTION C: Generate the Twitter Context");
            var twitterContext = new TwitterContext(authorizer);

            // SECTION D: Get Tweets for user
            Console.WriteLine("SECTION D: Get Tweets for user");
            var statusTweets = from tweet in twitterContext.Status
                               where tweet.Type == StatusType.User &&
                                       tweet.ScreenName == twitterAccountToDisplay &&
                                       tweet.IncludeContributorDetails == true &&
                                       tweet.Count == 10 &&
                                       tweet.IncludeEntities == true
                               select tweet;

            // SECTION E: Print Tweets
            Console.WriteLine("SECTION E: Print Tweets");
            PrintTweets(statusTweets);
            Console.ReadLine();
        }

      
           private static void PrintTweets(IQueryable<Status> statusTweets)
            {
                foreach (var statusTweet in statusTweets)
                {
                    Console.WriteLine(string.Format("\n\nTweet From [{0}] at [{1}]: \n-{2}",
                        statusTweet.ScreenName,
                        statusTweet.CreatedAt,
                        statusTweet.Text));

                    Thread.Sleep(1000);
                }
            }
    }
}
