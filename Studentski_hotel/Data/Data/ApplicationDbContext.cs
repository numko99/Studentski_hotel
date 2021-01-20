using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBdata.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Studentski_hotel.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CiklusStudija> CiklusStudijas { get; set; }
        public DbSet<DnevnaPonuda> DnevnaPonudas { get; set; }
        public DbSet<Drzava> Drzavas { get; set; }
        public DbSet<Fakultet> Fakultets { get; set; }
        public DbSet<GodinaStudija> GodinaStudijas { get; set; }
        public DbSet<Grad> Grads { get; set; }
        public DbSet<Kanton> Kantons { get; set; }
        public DbSet<Pol> Pols { get; set; }
        public DbSet<Kartica> Karticas { get; set; }
        public DbSet<Konkurs> Konkurs { get; set; }
        public DbSet<Kuharica> Kuharicas { get; set; }
        public DbSet<Lokacija> Lokacijas { get; set; }
        public DbSet<NacinUplate> NacinUplates { get; set; }
        public DbSet<NajavaOdlaska> NajavaOdlaskas { get; set; }
        public DbSet<Obavijest> Obavijests { get; set; }
        public DbSet<Obrok> Obroks { get; set; }
        public DbSet<Osoblje> Osobljes { get; set; }
        public DbSet<Soba> Sobas { get; set; }
        public DbSet<TipKandidata> tipKandidatas { get; set; }
        public DbSet<Ugovor> Ugovors { get; set; }
        public DbSet<Uplata> Uplatas { get; set; }
        public DbSet<Upozorenje> Upozorenjes { get; set; }
        public DbSet<VrstaStanjaZahtjeva> VrstaStanjaZahtjevas { get; set; }
        public DbSet<VrstaStanjaKonkursa> VrstaStanjaKonkursas { get; set; }
        public DbSet<RezultatKonkursa> RezultatKonkursas { get; set; }
        public DbSet<VrstaRazlogaOdbijanja> vrstaRazlogaOdbijanjas { get; set; }
        public DbSet<VrstaZahtjeva> VrstaZahtjevas { get; set; }
        public DbSet<Zahtjev> Zahtjevs { get; set; }
        public DbSet<Recepcioer> Recepcioners { get; set; }
        public DbSet<Korisnik> Korisniks { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Referent> Referents { get; set; }
        //public DbSet<Referent> Referents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
             .SelectMany(t => t.GetForeignKeys())
             .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;



            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Korisnik>()
            .HasOne<Recepcioer>(s => s.Recepcioer)
            .WithOne(ad => ad.Korisnik)
            .HasForeignKey<Recepcioer>(ad => ad.KorisnikID);

            modelBuilder.Entity<Korisnik>()
            .HasOne<Student>(s => s.Student)
            .WithOne(ad => ad.Korisnik)
            .HasForeignKey<Student>(ad => ad.KorisnikID);

            modelBuilder.Entity<Korisnik>()
        .HasOne<Referent>(s => s.Referent)
        .WithOne(ad => ad.Korisnik)
        .HasForeignKey<Referent>(ad => ad.KorisnikID);



        }

    }
}
