using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                    obavijestID=o.ID,
                    Naslov = o.Naslov,
                    Text = o.Text.Trim(),
                    RecepcionerID = o.RecepcioerID,
                    datum_dodavanja=o.DatumVrijeme
                }).Single();
              
             
            return View(obj);
        }
        public IActionResult RecepcionerPocetna()
        {
            return View();
        }

        public  IActionResult PrikazObavijesti(string pretraga)
        {
            PrikazObavijesti all = new PrikazObavijesti();
             all.obavijesti = dbContext.Obavijests.Where(x=>pretraga==null || (x.Naslov.ToLower().StartsWith(pretraga.ToLower())))
             .Select(o => new PrikazObavijesti.Row
            {
                obavijestID=o.ID,
                Naslov = o.Naslov,
                Text = o.Text,
                DatumObj = o.DatumVrijeme,
                 ImeRecepcionera = o.Recepcioer.Ime + " " + o.Recepcioer.Prezime
                
            }).ToList();

            all.obavijesti = all.obavijesti.OrderByDescending(x => x.DatumObj).ToList();

            all.pretraga = pretraga;
            //TempData["all"] = all.obavijesti;
            return View(all);
        }


        public async Task<IActionResult> SnimiAsync(DodajObavijestVM obj) 
        {
            
                var user = await _userManager.GetUserAsync(User);
                var referent = dbContext.Recepcioners.Where(a => a.KorisnikID == user.Id).FirstOrDefault();
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

            ob.RecepcioerID = referent.Korisnik.Recepcioer.ID;

            dbContext.SaveChanges();

            return Redirect("/Recepcija/RecepcionerPocetna");
        }

        public IActionResult Obrisi(int obavijestID)
        {
             var obrisana=dbContext.Obavijests.Find(obavijestID);

            dbContext.Remove(obrisana);
            dbContext.SaveChanges();
            return Redirect("/Recepcija/RecepcionerPocetna");
        }
        
        public IActionResult PregledObavijesti(int obavijestID)
        {
           

            var ob = dbContext.Obavijests.Find(obavijestID);
           
            PregledObavijesti po = new PregledObavijesti();
            po.obavijestID = ob.ID;
            po.Naslov = ob.Naslov;
            po.Text = ob.Text;
            po.ImeRecepcionera = ob.Recepcioer.Ime + " " + ob.Recepcioer.Prezime;
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

    }
}
