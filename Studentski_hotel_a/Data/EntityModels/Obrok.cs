using System;
using System.Collections.Generic;
using System.Text;

namespace DBdata.EntityModels
{
   public class Obrok
    {
        public int ID { get; set; }
        public string Datum { get; set; }
        public float Iznos { get; set; }
        public string Detalji { get; set; }
        public Kuharica Kuharica { get; set; }
        public int KuharicaID { get; set; }
    }
}
