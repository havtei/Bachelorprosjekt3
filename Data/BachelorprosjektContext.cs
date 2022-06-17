#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bachelorprosjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bachelorprosjekt.Data
{
    public class BachelorprosjektContext : IdentityDbContext<IdentityUser>
    {
        

        public BachelorprosjektContext (DbContextOptions<BachelorprosjektContext> options)
            : base(options)
        {
        }

        public DbSet<Bachelorprosjekt.Models.Oppdragsgiver> Oppdragsgiver { get; set; }

        public DbSet<Bachelorprosjekt.Models.ProjectType> ProjectType { get; set; }

        public DbSet<Bachelorprosjekt.Models.ProsjektDescription> ProsjektDescription { get; set; }

        public DbSet<Bachelorprosjekt.Models.ProjectStatus> ProjectStatus { get; set; }

        public DbSet<Bachelorprosjekt.Models.Domain> Domain { get; set; }
        public DbSet<Bachelorprosjekt.Models.Lokasjon> Lokasjon { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
        }
    }
}
