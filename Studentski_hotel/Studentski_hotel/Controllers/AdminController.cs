using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Studentski_hotel.Data;
using Studentski_hotel.Helper;
using Studentski_hotel.Models.Admin;
namespace Studentski_hotel.Controllers
{
        //[Autorizacija(false, true, false)]
    public class AdminController : Controller
    {

        private UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext dbContext;


        public AdminController(UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager, ApplicationDbContext _dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DodajNastavnika(int RecepcionerID)
        {
            List<SelectListItem> kantoni = dbContext.Kantons.Select(a => new SelectListItem
            {
                Text = a.Naziv,
                Value = a.ID.ToString()
            }).ToList();
            List<SelectListItem> opstine = dbContext.Grads.Select(a => new SelectListItem
            {
                Text = a.Naziv,
                Value = a.ID.ToString()
            }).ToList();

            List<SelectListItem> pol = dbContext.Pols.Select(a => new SelectListItem
            {
                Text = a.Naziv,
                Value = a.ID.ToString()
            }).ToList();
            DodajNastavnikaVM nastavnik;
            if (RecepcionerID > 0)
            {
                nastavnik = dbContext.Recepcioners.Where(a => a.ID == RecepcionerID).Select(a => new DodajNastavnikaVM
                {
                    ID = a.ID,
                    Ime = a.Ime,
                    Prezime = a.Prezime,
                    //DatumRodjenja=DATE(a.DatumRodjenja)
                    PolID = a.PolID,
                    mobitel = a.Korisnik.PhoneNumber,
                    Adresa = a.Lokacija.Adresa,
                    MjestoStanovanjaID = a.Lokacija.MjestoStanovanjaID,
                    KantonID = a.Lokacija.KantonID,
                    email = a.Korisnik.Email,



                }).Single();
            }
            else
            {
                nastavnik = new DodajNastavnikaVM();
            }

            nastavnik.Kanton = kantoni;
            nastavnik.Pol = pol;
            nastavnik.MjestoStanovanja = opstine;
            return View(nastavnik);
        }
 
        public string Snimi(DodajNastavnikaVM admir)
        {

            var korisnik = new Korisnik
            {
                Email = admir.email,
                UserName = admir.email,
                EmailConfirmed = true,
                PhoneNumber = admir.mobitel
            };
            IdentityResult result = _userManager.CreateAsync(korisnik, admir.password).Result;
            if (!result.Succeeded)
            {
                return "errors: " + string.Join('|', result.Errors);
            }

            Referent recepcioner = new Referent()
            {
                Korisnik=korisnik,
                Ime = admir.Ime,
                Prezime = admir.Prezime,
                PolID = admir.PolID,
                DatumRodjenja = admir.DatumRodjenja.ToString(),

            };

            Lokacija lokacija = new Lokacija();
            lokacija.Adresa = admir.Adresa;
            lokacija.MjestoStanovanjaID = admir.MjestoStanovanjaID;
            lokacija.KantonID = admir.KantonID;
            dbContext.Add(lokacija);
            dbContext.SaveChanges();
            recepcioner.LokacijaID = lokacija.ID;
            dbContext.Add(recepcioner);
            dbContext.SaveChanges();

            return "Uspjesno dodan";
        }
    }
}
