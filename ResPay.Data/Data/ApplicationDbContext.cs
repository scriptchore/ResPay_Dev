using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResPay.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser> 
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
