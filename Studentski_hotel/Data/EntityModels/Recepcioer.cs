using System;
using System.Collections.Generic;
using System.Text;

namespace DBdata.EntityModels
{
   public class Recepcioer:Osoblje
    {
        public Korisnik Korisnik { get; set; }
        public string KorisnikID { get; set; }
    }
}
