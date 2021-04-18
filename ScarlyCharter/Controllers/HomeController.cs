using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScarlyCharter.Data;
using ScarlyCharter.Models;

namespace ScarlyCharter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController (ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index ()
        {
            return View ();
        }

        public IActionResult Privacy ()
        {
            return View ();
        }

        public IActionResult Schedule ()
        {
            return View ();
        }


        public IActionResult Search (string region, string season, string fish)
        {
            var db = new ApplicationDbContext ();
            var guides = from guide in db.Guides
                         select guide;

            var regions = db.Regions.ToArray ();
            var seasons = db.Seasons.ToArray ();

            foreach (var guide in guides)
            {
                guide.Region = regions.ElementAt (guide.RegionId);
                guide.Season = seasons.ElementAt (guide.SeasonId ?? 4);
            }

            if (!string.IsNullOrEmpty (region))
                guides = guides.Where (s => s.Region.RegionName.Contains (region));

            if (!string.IsNullOrEmpty (season) && !season.Equals ("All Seasons"))
                guides = guides.Where (s => s.Season.SeasonName.Contains (season));

            if (!string.IsNullOrEmpty (fish))
                guides = guides.Where (s => db.Fish.ElementAt (s.RegionId).FishName.Contains (fish));

            var Searches = new List<SearchViewModel> ();

            foreach (var guide in guides.ToList ())
            {
                if (guide == null)
                    continue;

                Searches.Add (new SearchViewModel
                {
                    GuideName = guide.GuideName,
                    Region = guide.Region.RegionName,
                    Season = guide.Season == null ? "All Seasons" : guide.Season.SeasonName,
                    FishingStyle = guide.FishingStyle,
                    Overnight = guide.Overnight ?? false,
                });
            }

            return View (Searches);
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
