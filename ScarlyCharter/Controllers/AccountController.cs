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
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController (ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            ModelState.AddModelError (string.Empty, "Invalid login attempt.");
            return View (model);
        }
    }
}
