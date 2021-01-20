using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBdata.EntityModels
{
    public class Korisnik : IdentityUser
    {
        public Student Student { get; set; }
        public Recepcioer Recepcioer { get; set; }
        public Referent Referent { get; set; }
        public Kuharica Kuharica { get; set; }
    }
}
