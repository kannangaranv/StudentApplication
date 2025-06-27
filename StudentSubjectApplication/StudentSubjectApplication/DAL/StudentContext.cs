using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentSubjectApplication.DAL
{
    public class StudentContext : DbContext
    {
        public DbSet<Domain.Entities.Student> Students { get; set; }
        public DbSet<Domain.Entities.Subject> Subjects { get; set; }

        //create the connection string in the appsettings.json file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=NUWANK-BP;Database=StudentDB;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

        }
        //    Server=localhost;Database=master;Trusted_Connection=True;
    }
}
