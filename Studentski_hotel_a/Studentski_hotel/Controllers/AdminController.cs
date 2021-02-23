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
    [Autorizacija(false, false, false,true)]
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
        public IActionResult Prikaz()
        {

            return View();
        }
        public IActionResult PrikazOsoblja(int tip)
        {
            if (tip == 0)
            {
                var osoblje = dbContext.Osobljes.Select(a => new PrikazOsobljaVM.Row
                {
                    ID = a.ID,
                    Ime = a.Ime,
                    Prezime = a.Prezime,
                    DatumRodjenja = a.DatumRodjenja
                }).ToList();
                PrikazOsobljaVM osobljes = new PrikazOsobljaVM();
                osobljes.Osoblje = osoblje;
                return View(osobljes);
            }
            if (tip == 1)
            {
                var osoblje = dbContext.Recepcioners.Select(a => new PrikazOsobljaVM.Row
                {
                    ID = a.ID,
                    Ime = a.Ime,
                    Prezime = a.Prezime,
                    DatumRodjenja = a.DatumRodjenja
                }).ToList();
                PrikazOsobljaVM osobljes = new PrikazOsobljaVM();
                osobljes.Osoblje = osoblje;
                return View(osobljes);
            }
            if (tip == 2)
            {
                var osoblje = dbContext.Referents.Select(a => new PrikazOsobljaVM.Row
                {
                    ID = a.ID,
                    Ime = a.Ime,
                    Prezime = a.Prezime,
                    DatumRodjenja = a.DatumRodjenja
                }).ToList();
                PrikazOsobljaVM osobljes = new PrikazOsobljaVM();
                osobljes.Osoblje = osoblje;
                return View(osobljes);
            }
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
                    DatumRodjenja = Convert.ToDateTime(a.DatumRodjenja),
                    PolID = a.PolID,
                    mobitel = a.Korisnik.PhoneNumber,
                    Adresa = a.Lokacija.Adresa,
                    MjestoStanovanjaID = a.Lokacija.MjestoStanovanjaID,
                    KantonID = a.Lokacija.KantonID,
                    email = a.Korisnik.Email,
                    KorisnikID = a.KorisnikID,
                    LokacijaID=a.LokacijaID,
                    TipKorisnika=1
                    



                }).FirstOrDefault();
                if (nastavnik == null)
                {
                    nastavnik = dbContext.Referents.Where(a => a.ID == RecepcionerID).Select(a => new DodajNastavnikaVM
                    {
                        ID = a.ID,
                        Ime = a.Ime,
                        Prezime = a.Prezime,
                        DatumRodjenja = Convert.ToDateTime(a.DatumRodjenja),
                        PolID = a.PolID,
                        mobitel = a.Korisnik.PhoneNumber,
                        Adresa = a.Lokacija.Adresa,
                        MjestoStanovanjaID = a.Lokacija.MjestoStanovanjaID,
                        KantonID = a.Lokacija.KantonID,
                        email = a.Korisnik.Email,
                        LokacijaID = a.LokacijaID,
                        TipKorisnika = 2

                    }).FirstOrDefault();
                }
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

        public IActionResult Snimi(DodajNastavnikaVM admir)
        {
            Korisnik korisnik;
            Lokacija lokacija;

            if (admir.ID == 0)
            {
                korisnik = new Korisnik();
                korisnik.Email = admir.email;
                korisnik.UserName = admir.email;
                korisnik.EmailConfirmed = true;
                korisnik.PhoneNumber = admir.mobitel;

                IdentityResult result = _userManager.CreateAsync(korisnik, admir.password).Result;
                if (!result.Succeeded)
                {
                    return Content("errors: " + string.Join('|', result.Errors));
                }
                lokacija = new Lokacija();
                lokacija.Adresa = admir.Adresa;
                lokacija.MjestoStanovanjaID = admir.MjestoStanovanjaID;
                lokacija.KantonID = admir.KantonID;
                dbContext.Add(lokacija);
                dbContext.SaveChanges();
            }
            else
            {
                korisnik = dbContext.Korisniks.Where(a => a.Id == admir.KorisnikID).FirstOrDefault();
                lokacija = dbContext.Lokacijas.Where(a => a.ID == admir.LokacijaID).FirstOrDefault();
            }
          
            if (admir.TipKorisnika == 1)
            {
                Recepcioer recepcioer;
                if (admir.ID == 0)
                {
                    recepcioer = new Recepcioer();
                    recepcioer.LokacijaID = lokacija.ID;
                    dbContext.Add(recepcioer);

                }
                else
                {
                    recepcioer = dbContext.Recepcioners.Where(a => a.ID == admir.ID).FirstOrDefault();
                    recepcioer.LokacijaID = admir.LokacijaID;
                }
                recepcioer.Korisnik = korisnik;
                recepcioer.Ime = admir.Ime;
                recepcioer.Prezime = admir.Prezime;
                recepcioer.PolID = admir.PolID;
                recepcioer.DatumRodjenja = admir.DatumRodjenja.ToString("dd/MM/yyyy");


                //dbContext.Add(recepcioer);
                dbContext.SaveChanges();
            }
            else if (admir.TipKorisnika == 2)
            {
                Referent recepcioer;
                if (admir.ID == 0)
                {
                    recepcioer = new Referent();
                    recepcioer.LokacijaID = lokacija.ID;
                    dbContext.Add(recepcioer);

                }
                else
                {
                    recepcioer = dbContext.Referents.Where(a => a.ID == admir.ID).FirstOrDefault();
                    recepcioer.LokacijaID = admir.LokacijaID;
                }
                recepcioer.Korisnik = korisnik;
                recepcioer.Ime = admir.Ime;
                recepcioer.Prezime = admir.Prezime;
                recepcioer.PolID = admir.PolID;
                recepcioer.DatumRodjenja = admir.DatumRodjenja.ToString("dd/MM/yyyy");


                //dbContext.Add(recepcioer);
                dbContext.SaveChanges();
            }



            return Redirect(url:"/Admin/Prikaz");
        }
    }
}
