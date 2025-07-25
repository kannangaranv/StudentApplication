﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentSubjectApplication.Infrastructure.DAL
{
    public class StudentContext : DbContext
    {
        public DbSet<Domain.Entities.Student> Students { get; set; }
        public DbSet<Domain.Entities.Subject> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
