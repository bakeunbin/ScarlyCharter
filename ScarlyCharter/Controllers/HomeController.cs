using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult Account ()
        {
            return View ();
        }

        [HttpGet]
        public IActionResult Login (string returnUrl = null)
        {
            return View (new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        public IActionResult Register (string returnUrl = null)
        {
            return View (new RegisterViewModel { ReturnUrl = returnUrl });
        }

        public IActionResult Logout ()
        {
            return View ();
        }

        public IActionResult ForgotPassword ()
        {
            return View ();
        }

        private readonly int SaltSize = 256;
        private readonly int HashSize = 256;
        private readonly int Iterations = 120000;

        private bool SlowEquals (byte [] a, byte [] b)
        {
            int res = a.Length ^ b.Length;

            for (int i = 0 ; i < Math.Min (a.Length, b.Length) ; i++)
                res |= a [i] ^ b [i];

            return res == 0;
        }

        [HttpPost]
        public IActionResult Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext ();
                var clients = db.Clients.ToList ();
                var clientc = from c in clients
                              where c.Username.Equals (model.Username)
                              select c;

                if (clientc.Any ())
                {
                    var client = clientc.First ();
                    var pbkdf2 = new Rfc2898DeriveBytes (model.Password, client.Salt, Iterations);
                    var hash = pbkdf2.GetBytes (HashSize);

                    if (SlowEquals (hash, client.Password))
                    {
                        if (!string.IsNullOrEmpty (model.ReturnUrl) && Url.IsLocalUrl (model.ReturnUrl))
                            return Redirect (model.ReturnUrl);
                        else
                            return RedirectToAction ("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError (string.Empty, "Either user doesn't exists or wrong password.");
            return View (model);
        }

        [HttpPost]
        public IActionResult Register (RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext ();
                var clients = db.Clients.ToList ();
                var clientc = from c in clients
                              where c.Username.Equals (model.Username)
                              select c;

                if (clientc.Any ())
                {
                    ModelState.AddModelError (string.Empty, "Invalid username.");
                    return View (model);
                }

                clientc = from c in clients
                          where c.Email.Equals (model.Email)
                          select c;

                if (clientc.Any ())
                {
                    ModelState.AddModelError (string.Empty, "Email already used.");
                    return View (model);
                }

                var salt = new byte [SaltSize];
                var provider = new RNGCryptoServiceProvider ();

                provider.GetBytes (salt);

                var pbkdf2a = new Rfc2898DeriveBytes (model.Password, salt, Iterations);
                var pbkdf2b = new Rfc2898DeriveBytes (model.Password, salt, Iterations);
                var passhash = pbkdf2a.GetBytes (HashSize);
                var confhash = pbkdf2b.GetBytes (HashSize);

                if (!SlowEquals (passhash, confhash))
                {
                    ModelState.AddModelError (string.Empty, "Passwords do not match!");
                    return View (model);
                }

                var client = new Client
                {
                    ClientId = clients.Count,
                    ClientName = model.Name,
                    PaymentInfo = model.PaymentInfo,
                    Email = model.Email,
                    Username = model.Username,
                    Password = passhash,
                    Salt = salt
                };

                db.Clients.Add (client);
                db.SaveChanges ();

                if (!string.IsNullOrEmpty (model.ReturnUrl) && Url.IsLocalUrl (model.ReturnUrl))
                    return Redirect (model.ReturnUrl);
                else
                    return RedirectToAction ("Index", "Home");
            }

            ModelState.AddModelError (string.Empty, "Registration failed.");
            return View (model);
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

            var guide_cand = (from g in db.Guides
                              where g.GuideName.Equals (guidestr)
                              select g);

            var guide = guide_cand.First ();

            var location_cand = (from l in db.Locations
                                 where l.Type == locationstr && l.RegionId == guide.RegionId
                                 select l);

            var location = location_cand.First ();

            var fish_cand = (from f in db.Fish
                             where f.FishName.Equals (fishstr)
                             select f);

            var fish = fish_cand.First ();

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
                Error = error,
                Guide = "",
                Location = !string.IsNullOrEmpty (location) ? location : "",
                PartySize = partySize != null ? (int) partySize : 1,
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
