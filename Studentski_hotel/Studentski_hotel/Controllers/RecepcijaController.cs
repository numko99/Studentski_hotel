using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentski_hotel.Data;
using Studentski_hotel.Helper;
using Studentski_hotel.Models.Recepcija;

namespace Studentski_hotel.Controllers
{
    [Autorizacija(false, true, false, false)]
    public class RecepcijaController : Controller
    {
        private UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext dbContext;


        public RecepcijaController(UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager, ApplicationDbContext _dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {


            return View();
        }
        public IActionResult EditObavijest(int obavijestID)
        {
            DodajObavijestVM obj = obavijestID == 0 ? new DodajObavijestVM() :
                dbContext.Obavijests.Where(x => x.ID == obavijestID).
                Select(o => new DodajObavijestVM
                {
                    obavijestID = o.ID,
                    Naslov = o.Naslov,
                    Text = o.Text.Trim(),
                    RecepcionerID = o.OsobljeID,
                    datum_dodavanja = o.DatumVrijeme
                }).Single();


            return View(obj);
        }
        public IActionResult RecepcionerPocetna()
        {
            return View();
        }

        public IActionResult PrikazObavijesti(string pretraga)
        {
            PrikazObavijesti all = new PrikazObavijesti();
            all.obavijesti = dbContext.Obavijests.Where(x => pretraga == null || (x.Naslov.ToLower().StartsWith(pretraga.ToLower())))
            .Select(o => new PrikazObavijesti.Row
            {
                obavijestID = o.ID,
                Naslov = o.Naslov,
                Text = o.Text,
                DatumObj = o.DatumVrijeme,
                ImeRecepcionera = o.Osoblje.Ime + " " + o.Osoblje.Prezime

            }).ToList();

            all.obavijesti = all.obavijesti.OrderByDescending(x => x.DatumObj).ToList();

            all.pretraga = pretraga;
            //TempData["all"] = all.obavijesti;
            return View(all);
        }


        public async Task<IActionResult> SnimiAsync(DodajObavijestVM obj)
        {

            var user = await _userManager.GetUserAsync(User);
            var referent = dbContext.Osobljes.Where(a => a.KorisnikID == user.Id).FirstOrDefault();
            Obavijest ob;
            if (obj.obavijestID == 0)
            {
                ob = new Obavijest();
                ob.DatumVrijeme = DateTime.Now.ToString("23.1.2021. 01:02:45");

                dbContext.Add(ob);
            }
            else
            {
                ob = dbContext.Obavijests.Find(obj.obavijestID);
            }


            ob.Naslov = obj.Naslov;
            ob.Text = obj.Text;

            ob.OsobljeID = referent.Korisnik.Osoblje.ID;

            dbContext.SaveChanges();

            return Redirect("/Recepcija/RecepcionerPocetna");
        }

        public IActionResult Obrisi(int obavijestID)
        {
            var obrisana = dbContext.Obavijests.Find(obavijestID);

            dbContext.Remove(obrisana);
            dbContext.SaveChanges();
            return Redirect("/Recepcija/RecepcionerPocetna");
        }

        public IActionResult PregledObavijesti(int obavijestID)
        {


            var ob = dbContext.Obavijests.Include(a => a.Osoblje).Where(a => a.ID == obavijestID).FirstOrDefault();

            PregledObavijesti po = new PregledObavijesti();
            po.obavijestID = ob.ID;
            po.Naslov = ob.Naslov;
            po.Text = ob.Text;
            po.ImeRecepcionera = ob.Osoblje.Ime + " " + ob.Osoblje.Prezime;
            po.DatumObj = ob.DatumVrijeme;




            return View(po);
        }
        //public PartialViewResult PrikazObavijestiPregled(string pretraga)
        //{
        //    PrikazObavijesti all = new PrikazObavijesti();
        //    all.obavijesti = dbContext.Obavijests.Where(x => pretraga == null || (x.Naslov.ToLower().StartsWith(pretraga.ToLower())))
        //    .Select(o => new PrikazObavijesti.Row
        //    {
        //        obavijestID = o.ID,
        //        Naslov = o.Naslov,
        //        Text = o.Text,
        //        DatumObj = Convert.ToDateTime(o.DatumVrijeme),
        //        ImeRecepcionera = o.Recepcioer.Ime + " " + o.Recepcioer.Prezime

        //    }).ToList();

        //    all.obavijesti = all.obavijesti.OrderByDescending(x => x.DatumObj).ToList();

        //    all.pretraga = pretraga;

        //    return PartialView(all);
        //}
        public IActionResult Pregled()
        {

            return View();
        }
        public IActionResult FilterSoba()
        {
            return View();
        }
        public IActionResult PrikazSoba(string Soba, string Krevet)
        {
            var db = dbContext.Sobas.Where(a => (Soba == null || a.BrojSobe.StartsWith(Soba)) && (Krevet == "0" || a.BrojKreveta.ToString() == Krevet)).
                Select(a => new PregledSobaVM.Row
                {
                    ID = a.ID,
                    BrojSobe = a.BrojSobe,
                    BrojKreveta = a.BrojKreveta.ToString(),
                }).ToList();
            foreach (var item in db)
            {
                var lista = dbContext.Ugovors.Where(x => x.SobaID == item.ID).Include(a => a.Student).ToList();
                if (lista.Count == 0)
                {
                    item.Studenti = "Slobodna soba";
                }
                else
                {

                    foreach (var item2 in lista)
                    {
                        item.Studenti += item2.Student.Ime + " " + item2.Student.Prezime + "; ";
                    }
                    if (lista.Count.ToString() == item.BrojKreveta)
                    {
                        item.Popunjena = true;
                    }
                }

            }

            PregledSobaVM model = new PregledSobaVM();
            model.Sobe = db;
            return View(model);
        }
        public IActionResult DetaljiPrikazSoba(int SobaID)
        {
            var model = dbContext.Sobas.Where(a => a.ID == SobaID).Select(a => new DetaljiPrikazSobavm
            {
                ID = a.ID,
                Broj_Sobe = a.BrojSobe,
                Sprat = a.Sprat,
                Napomena = a.Napomena,
                ImaBalkon = a.ImaBalkon? "DA" : "NE",
                bROJ_Kreveta = a.BrojKreveta.ToString()


            }).FirstOrDefault();
            var studenti = dbContext.Ugovors.Where(a => a.SobaID == SobaID).Select(a => new DetaljiPrikazSobavm.Studenti
            {
                ID = a.ID,
                Ime = a.Student.Ime + " " + a.Student.Prezime
            }).ToList();
            model.studenti = studenti;
            model.Popunjena = model.bROJ_Kreveta == studenti.Count.ToString();
            return View(model);
        }
        public IActionResult DodajUsobu(int SobaID)
        {
            var Primljenistudenti = dbContext.Ugovors.Select(a => a.StudentID).ToList();
            var studenti = dbContext.Students.Where(a => !Primljenistudenti.Contains(a.ID)).Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Ime + " " + a.Prezime
            }).ToList();
            var sobe = dbContext.Sobas.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.BrojSobe
            }).ToList();
            var ZauzeteKartice = dbContext.Ugovors.Select(a => a.KarticaID).ToList();
            var kartice = dbContext.Karticas.Where(a => !ZauzeteKartice.Contains(a.ID)).Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.BrojKartice
            }).ToList();

            var model = new DodajUSobuVM();
            model.Studenti = studenti;
            model.Soba = sobe;
            model.SobaID = SobaID;
            model.BrojKartice = kartice;
            return View(model);
        }
        public async Task<IActionResult> SnimiUgovor(DodajUSobuVM admir)
        {
            var user = await _userManager.GetUserAsync(User);
            var osoblje = dbContext.Osobljes.Where(a => a.KorisnikID == user.Id).FirstOrDefault();

            var ugovor = new Ugovor
            {
                SobaID = admir.SobaID,
                StudentID = admir.StudentID,
                KarticaID=admir.BrojKarticeID,
                DodanUgovorOsobljeID=osoblje.ID
            };
            dbContext.Add(ugovor);
            dbContext.SaveChanges();
            return Redirect("/Recepcija/DetaljiPrikazSoba?SobaID=" + admir.SobaID);
        }
        public IActionResult PrikazStudenata()
        {

            return View();
        }
        public IActionResult PrikazZahtjeva()
        {

            return View();
        }

    }
}
