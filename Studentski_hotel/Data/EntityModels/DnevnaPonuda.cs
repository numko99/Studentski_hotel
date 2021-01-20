using System;
using System.Collections.Generic;
using System.Text;

namespace DBdata.EntityModels
{
    public class DnevnaPonuda
    {
        public int ID { get; set; }
        public string Tekst { get; set; }
        public string Datum { get; set; }
        public Kuharica Kuharica { get; set; }
        public int KuharicaID { get; set; }
        public Kartica Kartica { get; set; }
        public int KarticaID { get; set; }
    }
}
