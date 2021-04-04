﻿using DBdata.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentski_hotel.Models.Recepcija
{
    public class DetaljiPrikazStudenataVM
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string ImeOca { get; set; }
        public string MjestoRodjenja { get; set; }
        public int MjestoRodjenjaID { get; set; }
        public string ZanimanjeRoditelja { get; set; }
        public string Pol { get; set; }
        public int PolID { get; set; }
        public string JMBG { get; set; }
        public string LicnaKarta { get; set; }
        public string Mjesto_izdavanja_LK { get; set; }
        public string DatumRodjenja { get; set; }
        public string Mobitel { get; set; }
        public string Email { get; set; }



        public string Adresa { get; set; }
        public string MjestoStanovanja { get; set; }
        public int MjestoStanovanjaID { get; set; }
        public string Kanton { get; set; }
        public int KantonID { get; set; }



        public string Fakultet { get; set; }
        public int FakultetID { get; set; }
        public string TipKandidata { get; set; }
        public int TipKandidataID { get; set; }
        public string BrojIndeksa { get; set; }
        public string CiklusStudija { get; set; }
        public int CiklusStudijaID { get; set; }
        public  string GodinaStudija { get; set; }
        public int GodinaStudijaID { get; set; }

        public bool Uselio { get; set; }
        public string Broj_Sobe { get; set; }
        public string Broj_Kartice { get; set; }
        public string PreostaliBrojObroka { get; set; }


    }
}
