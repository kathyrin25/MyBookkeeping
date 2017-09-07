using MyBookkeeping.CustomResults;
using MyBookkeeping.Repositories;
using MyBookkeeping.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;

namespace MyBookkeeping.Controllers
{
    [AllowAnonymous]
    public class FeedController : Controller
    {
        private readonly BookkeepingService _BookkeepingSvc;

        public FeedController()
        {
            var unitOfWork = new EFUnitOfWork();
            _BookkeepingSvc = new BookkeepingService(unitOfWork);
        }

        // GET: Feed
        public ActionResult Index()
        {
            var feed = this.GetFeedData();
            return new RssResult(feed);
        }

        private SyndicationFeed GetFeedData()
        {
            var feed = new SyndicationFeed(
                "My Bookkeeping",
                "Data RSS！",
                new Uri(Url.Action("Login", "Home", null, "http")));

            var items = new List<SyndicationItem>();

            /*顯示最近3個月的資料*/
            var records = _BookkeepingSvc.Lookup().Where(x => x.Date >= DateTime.Now.AddMonths(-3)).OrderByDescending(x => x.Date);
                       
            foreach (var record in records)
            {
                var item = new SyndicationItem(
                    record.Date.ToString("yyyy/MM/dd"),
                    record.Type.ToString(),
                    new Uri(Url.Action("Details", "Bookkeeping", new { id = record.Id }, "http")),
                    "ID",
                    DateTime.Now);

                items.Add(item);
            }           

            feed.Items = items;
            return feed;
        }
        
    }
}