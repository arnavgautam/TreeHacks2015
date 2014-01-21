using Microsoft.ApplicationServer.Caching;
using System.Diagnostics;
using System.Web.Mvc;
using TweetReader.Web.Repositories;

namespace TweetReader.Web.Controllers
{
    public class TwitterFeedController : Controller
    {
        public ActionResult Index(string name)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Tweets entries = getTweets(name);
            
            timer.Stop();
            ViewBag.LoadTime = timer.Elapsed.TotalMilliseconds;

            return View(entries);
        }
        
        Tweets getTweets(string name)
        {
            DataCache cache = new DataCache();

            Tweets entries = Tweets.FromObject(cache.Get(name));

            if (entries == null)
            {
                entries = TwitterFeed.GetTweets(name);
                cache.Add(name, entries.GetBytes());
            }

            return entries;
        }
    }
}
