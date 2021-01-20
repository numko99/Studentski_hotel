﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Studentski_hotel.Data;
using Studentski_hotel.Helper;
using Studentski_hotel.Models;

namespace Studentski_hotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<Korisnik> _userManager;
        private SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext applicationdbcontext;
        public HomeController(ILogger<HomeController> logger, UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager, ApplicationDbContext _applicationdbcontext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            applicationdbcontext = _applicationdbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var user =await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var referent = applicationdbcontext.Referents.Where(a => a.KorisnikID == user.Id).FirstOrDefault();
                if (referent != null)
                {
                string ime = referent.Ime;
                    return Redirect(url: "/Referent/PrijavePocetna");
                }
            }  
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
