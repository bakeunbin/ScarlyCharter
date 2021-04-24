using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ScarlyCharter.Data;
using ScarlyCharter.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace ScarlyCharter.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController (ILogger<AccountController> logger)
        {
            _logger = logger;
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
                             where c.Username.Equals (model.Email)
                             select c;

                if (!clientc.Any ())
                {
                    ModelState.AddModelError (string.Empty, "Invalid login attempt.");
                    return View (model);
                }

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

            ModelState.AddModelError (string.Empty, "Invalid login attempt.");
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
                              where c.Username.Equals (model.Email)
                              select c;

                if (clientc.Any ())
                {
                    ModelState.AddModelError (string.Empty, "User already exists.");
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
    }
}
