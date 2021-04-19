using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScarlyCharter.Data;
using ScarlyCharter.Models;
using System.Globalization;

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

        public IActionResult BookTrip (string guidestr, string locationstr, string fishstr, int partySize, string datestr)
        {
            DateTime parsed;

            if (partySize < 1)
                return Redirect ("/Home/Schedule?error=2");

            if (!DateTime.TryParseExact (datestr, "MM/dd/yyyy hh:mm", new CultureInfo ("en-US"), DateTimeStyles.None, out parsed))
                return Redirect ("/Home/Schedule?error=1");

            var db = new ApplicationDbContext ();
            var schedule = new Schedule
            {
                ScheduleId = db.Schedules.Count (),
                Date = parsed,
                StartTime = parsed.TimeOfDay,
                EndTime = parsed.TimeOfDay.Add (new TimeSpan (8, 0, 0))
            };

            db.Schedules.Add (schedule);

            var guide = (from g in db.Guides
                         where g.GuideName.Equals (guidestr)
                         select g).First ();

            var location = (from l in db.Locations
                            where l.Type == locationstr && l.RegionId == guide.RegionId
                            select l).First ();

            var fish = (from f in db.Fish
                        where f.FishName.Equals (fishstr)
                        select f).First ();

            var booked_trip = new BookedTrip
            {
                TripId = db.BookedTrips.Count (),
                ClientId = 0,
                GuideId = guide.GuideId,
                ScheduleId = schedule.ScheduleId,
                LocationId = location.LocationId,
                TargetFishId = fish.FishId,
                PartySize = partySize,
                FishingStyle = guide.FishingStyle
            };

            db.BookedTrips.Add (booked_trip);

            db.SaveChanges ();

            return Redirect ("/Home/Schedule?error=0");
        }

        public IActionResult Schedule (string guide, string location, int? partySize, string date, int? error)
        {
            var db = new ApplicationDbContext ();
            var model = new ScheduleViewModel
            {
                Error = error ?? 0,
                Guide = "",
                Location = !string.IsNullOrEmpty (location) ? location : "",
                PartySize = (int) (partySize != null ? partySize : 1),
                Date = !string.IsNullOrEmpty (date) ? date : ""
            };

            if (!string.IsNullOrEmpty (guide))
            {
                var gid = (from g in db.Guides
                          where g.GuideName.Equals (guide)
                          select g).First ();

                model.Guide = guide;
            }

            return View (model);
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
                    Overnight = (guide.Overnight ?? false) ? "Yes" : "No",
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
