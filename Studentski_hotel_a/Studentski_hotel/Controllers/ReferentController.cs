﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Studentski_hotel.Data;
using Studentski_hotel.Helper;
using Studentski_hotel.Models.Referent;

namespace Studentski_hotel.Controllers
{
    [Autorizacija(false, false,true,false)]

    public class ReferentController : Controller
    {
        private  UserManager<Korisnik> _userManager;
        private  SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext dbContext;

        public ReferentController(ApplicationDbContext dbContext, UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PrikazPrijava(string pretraga,  int Status, int sort)
        {
           


                var prijave = dbContext.Konkurs.Where(a => (pretraga == null || (a.Ime + ' ' + a.Prezime).ToLower().StartsWith(pretraga.ToLower())
                || (a.Prezime + ' ' + a.Ime).ToLower().StartsWith(pretraga.ToLower())) && (a.RezultatKonkursa.VrstaStanjaKonkursaID == Status || Status == 0))
                    .Select(a => new PrikazPrijavaVM.Row()
                    {
                        ID = a.ID,
                        Ime = a.Ime,
                        Prezime = a.Prezime,
                        DatumRodjenja = a.DatumRodjenja,
                        OpstinaPrebivalista = a.MjestoStanovanja.Naziv,
                        StudentID = a.StudentID,
                        Bodovi = a.RezultatKonkursa == null ? 0 : a.RezultatKonkursa.BrojBodova,
                        Status = a.RezultatKonkursa == null ? "Nije pregledan" : a.RezultatKonkursa.VrstaStanjaKonkursa.Naziv,
                        Razlog = a.RezultatKonkursa == null ? "Nije pregledan" : a.RezultatKonkursa.VrstaRazlogaOdbijanja.Naziv,

                    }).ToList();
                if (sort == 1)
                {
                    prijave = prijave.OrderByDescending(a => a.Bodovi).ToList();
                }
                else if (sort == 2)
                {
                    prijave = prijave.OrderBy(a => a.Ime).ToList();
                }
                else if (sort == 3)
                {
                    prijave = prijave.OrderByDescending(a => a.DatumRodjenja).ToList();
                }
                PrikazPrijavaVM model = new PrikazPrijavaVM();
                model.Students = prijave;
                //model.Pretraga = pretraga;
                return View(model);
            
        }
        public IActionResult PrijavePocetna()
        {
           
             return View();
            
        }
        //public DetaljiPrikazPrijavaVM GenerisanjeStudenta(int PrvaParameta,int DrugiParametar)
        //{

        //}
        public IActionResult DetaljiPrikazPrijava(int KonkursID)
        {
            var student = dbContext.Konkurs.Where(a => KonkursID == a.ID).Select(admir => new DetaljiPrikazPrijavaVM
            {
                ID = admir.ID,
                Ime = admir.Ime,
                Prezime = admir.Prezime,
                ImeOca = admir.ImeOca,
                MjestoRodjenjaID = admir.MjestoRodjenjaID,
                MjestoRodjenja = admir.MjestoRodjenja.Naziv,
                ZanimanjeRoditelja = admir.ZanimanjeRoditelja,
                PolID = admir.PolID,
                Pol = admir.Pol.Naziv,
                JMBG = admir.JMBG,
                LicnaKarta = admir.LicnaKarta,
                DatumRodjenja = admir.DatumRodjenja.ToString(),
                Mobitel = admir.Mobitel,
                Email = admir.Email,

                Adresa = admir.Adresa,
                MjestoStanovanjaID = admir.MjestoStanovanjaID,
                MjestoStanovanja = admir.MjestoStanovanja.Naziv,
                KantonID = admir.KantonID,
                Kanton = admir.Kanton.Naziv,

                FakultetID = admir.FakultetID,
                Fakultet = admir.Fakultet.Naziv,
                TipKandidataID = admir.TipKandidataID,
                TipKandidata = admir.TipKandidata.Naziv,
                BrojIndeksa = admir.BrojIndeksa,
                CiklusStudijaID = admir.CiklusStudijaID,
                CiklusStudija = admir.CiklusStudija.Naziv,
                GodinaStudijaID = admir.GodinaStudijaID,
                GodinaStudija = admir.GodinaStudija.Naziv,
                Bodovi=admir.RezultatKonkursa==null?0:admir.RezultatKonkursa.BrojBodova,
                VrsteStanjaKonkursaID=admir.RezultatKonkursa==null?null:admir.RezultatKonkursa.VrstaStanjaKonkursaID,
                VrstaRazlogaOdbijanjaID=admir.RezultatKonkursa==null?null:admir.RezultatKonkursa.VrstaRazlogaOdbijanjaID
                
            }).Single();
          
            var stanje = dbContext.VrstaStanjaKonkursas.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Naziv
            }).ToList(); 
            var razlog  = dbContext.vrstaRazlogaOdbijanjas.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Naziv
            }).ToList();
            student.VrsteStanjaKonkursa = stanje;
            student.VrstaRazlogaOdbijanja = razlog;
            return View(student);
        }
        public IActionResult SnimiPrimljenogStudenta(DetaljiPrikazPrijavaVM admir)
        {
            var konkurs = dbContext.Konkurs.Find(admir.ID);

            var rezultat = konkurs.RezultatKonkursaID == null ? new RezultatKonkursa() : dbContext.RezultatKonkursas.Find(konkurs.RezultatKonkursaID);

            rezultat.VrstaStanjaKonkursaID = admir.VrsteStanjaKonkursaID;
            rezultat.VrstaRazlogaOdbijanjaID= rezultat.VrstaStanjaKonkursaID != 4? null: admir.VrstaRazlogaOdbijanjaID;
            if(rezultat.VrstaStanjaKonkursaID==4 && rezultat.VrstaRazlogaOdbijanjaID == 1)
            {
                rezultat.BrojBodova = 0;

            }
            else
            {
                rezultat.BrojBodova = admir.Bodovi;

            }
            var st = dbContext.Students.Find(konkurs.StudentID);
            if (konkurs.StudentID>0 && rezultat.VrstaStanjaKonkursaID !=2 && st.Uselio==false)
            {
            var lok = dbContext.Lokacijas.Find(st.LokacijaID);
                dbContext.Remove(konkurs.Student);
                dbContext.Remove(lok);
                dbContext.SaveChanges();
            }
            if (rezultat.ID == 0)
            {
                dbContext.Add(rezultat);
                dbContext.SaveChanges();

                konkurs.RezultatKonkursaID = rezultat.ID;
            }

            dbContext.SaveChanges();
            
            if (rezultat.VrstaStanjaKonkursaID == 2 && konkurs.StudentID==null)
            {
                Student student = new Student();
                student.Ime = admir.Ime;
                student.Prezime = admir.Prezime;
                student.ImeOca = admir.ImeOca;
                student.MjestoRodjenjaID = admir.MjestoRodjenjaID;
                student.ZanimanjeRoditelja = admir.ZanimanjeRoditelja;
                student.JMBG = admir.JMBG;
                student.PolID = admir.PolID;
                student.LicnaKarta = admir.LicnaKarta;
                student.DatumRodjenja = admir.DatumRodjenja.ToString();
                student.Mobitel = admir.Mobitel;
                student.Email = admir.Email;
                student.FakultetID = admir.FakultetID;
                student.TipKandidataID = admir.TipKandidataID;
                student.BrojIndeksa = admir.BrojIndeksa;
                student.CiklusStudijaID = admir.CiklusStudijaID;
                student.GodinaStudijaID = admir.GodinaStudijaID;
                student.Uselio = false;
                Lokacija lokacija = new Lokacija();
                lokacija.Adresa = admir.Adresa;
                lokacija.MjestoStanovanjaID = admir.MjestoRodjenjaID;
                lokacija.KantonID = admir.KantonID;
                dbContext.Add(lokacija);
                dbContext.SaveChanges();

                student.LokacijaID = lokacija.ID;
                dbContext.Add(student);
                dbContext.SaveChanges();

                konkurs.StudentID = student.ID;
                dbContext.SaveChanges();

            }
            return Redirect(url: "/Referent/PrijavePocetna"); 
        }
        public IActionResult SnimiPrimljeneStudente(int minBodova,int BrojStudenata)
        {
          
            var studenti = dbContext.Konkurs.Where(a => a.RezultatKonkursa.VrstaStanjaKonkursaID==1 && a.RezultatKonkursa.BrojBodova>minBodova).Select(admir => new DetaljiPrikazPrijavaVM
            {
                ID = admir.ID,
                Ime = admir.Ime,
                Prezime = admir.Prezime,
                ImeOca = admir.ImeOca,
                MjestoRodjenjaID = admir.MjestoRodjenjaID,
                MjestoRodjenja = admir.MjestoRodjenja.Naziv,
                ZanimanjeRoditelja = admir.ZanimanjeRoditelja,
                PolID = admir.PolID,
                Pol = admir.Pol.Naziv,
                JMBG = admir.JMBG,
                LicnaKarta = admir.LicnaKarta,
                DatumRodjenja = admir.DatumRodjenja.ToString(),
                Mobitel = admir.Mobitel,
                Email = admir.Email,

                Adresa = admir.Adresa,
                MjestoStanovanjaID = admir.MjestoStanovanjaID,
                MjestoStanovanja = admir.MjestoStanovanja.Naziv,
                KantonID = admir.KantonID,
                Kanton = admir.Kanton.Naziv,

                FakultetID = admir.FakultetID,
                Fakultet = admir.Fakultet.Naziv,
                TipKandidataID = admir.TipKandidataID,
                TipKandidata = admir.TipKandidata.Naziv,
                BrojIndeksa = admir.BrojIndeksa,
                CiklusStudijaID = admir.CiklusStudijaID,
                CiklusStudija = admir.CiklusStudija.Naziv,
                GodinaStudijaID = admir.GodinaStudijaID,
                GodinaStudija = admir.GodinaStudija.Naziv,
                Bodovi = admir.RezultatKonkursa == null ? 0 : admir.RezultatKonkursa.BrojBodova,
                VrsteStanjaKonkursaID = admir.RezultatKonkursa == null ? null : admir.RezultatKonkursa.VrstaStanjaKonkursaID,
                VrstaRazlogaOdbijanjaID = admir.RezultatKonkursa == null ? null : admir.RezultatKonkursa.VrstaRazlogaOdbijanjaID

            }).ToList();
            studenti = studenti.OrderByDescending(a => a.Bodovi).ToList();
            var brojac = 0;
            foreach (var admir in studenti)
            {
                Student student = new Student();
                student.Ime = admir.Ime;
                student.Prezime = admir.Prezime;
                student.ImeOca = admir.ImeOca;
                student.MjestoRodjenjaID = admir.MjestoRodjenjaID;
                student.ZanimanjeRoditelja = admir.ZanimanjeRoditelja;
                student.JMBG = admir.JMBG;
                student.PolID = admir.PolID;
                student.LicnaKarta = admir.LicnaKarta;
                student.DatumRodjenja = admir.DatumRodjenja.ToString();
                student.Mobitel = admir.Mobitel;
                student.Email = admir.Email;
                student.FakultetID = admir.FakultetID;
                student.TipKandidataID = admir.TipKandidataID;
                student.BrojIndeksa = admir.BrojIndeksa;
                student.CiklusStudijaID = admir.CiklusStudijaID;
                student.GodinaStudijaID = admir.GodinaStudijaID;
                student.Uselio = false;
                Lokacija lokacija = new Lokacija();
                lokacija.Adresa = admir.Adresa;
                lokacija.MjestoStanovanjaID = admir.MjestoRodjenjaID;
                lokacija.KantonID = admir.KantonID;
                dbContext.Add(lokacija);
                dbContext.SaveChanges();

                student.LokacijaID = lokacija.ID;
                dbContext.Add(student);
                dbContext.SaveChanges();
                var konkurs = dbContext.Konkurs.Find(admir.ID);
                var stanje = dbContext.RezultatKonkursas.Find(konkurs.RezultatKonkursaID);
                stanje.VrstaStanjaKonkursaID = 2;
                konkurs.StudentID = student.ID;
                
                dbContext.SaveChanges();
                brojac++;
                if (brojac == BrojStudenata)
                {
                    break;
                }
            }
            return Redirect(url: "/Referent/PrijavePocetna");
        }
    }



}
//TREBA U DB SET DODAT 