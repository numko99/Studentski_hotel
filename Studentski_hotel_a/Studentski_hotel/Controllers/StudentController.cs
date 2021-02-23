using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Studentski_hotel.Data;
using Studentski_hotel.Helper;
using Studentski_hotel.Models.Student;

namespace Studentski_hotel.Controllers
{
    [Autorizacija(true, false, false,false)]

    public class StudentController : Controller
    {
        private UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext dbContext;


        public StudentController(UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager, ApplicationDbContext _dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            dbContext = _dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentPocetna()
        {
            return View();

        }
        public IActionResult PrikazObavijestiStudent(string pretraga)
        {
            PrikazObavijestiStudent all = new PrikazObavijestiStudent();
            all.obavijesti = dbContext.Obavijests.Where(x => pretraga == null || (x.Naslov.ToLower().StartsWith(pretraga.ToLower())))
            .Select(o => new PrikazObavijestiStudent.Row
            {
                obavijestID = o.ID,
                Naslov = o.Naslov,
                Text = o.Text,
                DatumObj = o.DatumVrijeme,
                ImeRecepcionera=o.Recepcioer.Ime + " " + o.Recepcioer.Prezime
            }).ToList();

            all.obavijesti = all.obavijesti.OrderByDescending(x => x.DatumObj).ToList();

            all.pretraga = pretraga;
            //TempData["all"] = all.obavijesti;
            return View(all);
        }
        public IActionResult PregledObavijestiS(int obavijestID)
        {

            //var user = await _userManager.GetUserAsync(User);
            //var referent = dbContext.Recepcioners.Where(a => a.KorisnikID == user.Id).FirstOrDefault();
            var ob = dbContext.Obavijests.Find(obavijestID);
            //ob.RecepcioerID = referent.Korisnik.Recepcioer.ID;


            PregledObavijestiS po = dbContext.Obavijests.Where(x => x.ID == obavijestID)
                .Select(ob => new PregledObavijestiS { 
            obavijestID = ob.ID,
            Naslov = ob.Naslov,
            Text = ob.Text,
            DatumObj = ob.DatumVrijeme,
            ImeRecepcionera = ob.Recepcioer.Ime + " " + ob.Recepcioer.Prezime
        }).Single();



            return View(po);
        }

    }

}
