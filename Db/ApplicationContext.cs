using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuestionServiceWebApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Db
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<Tag> Tags { get; set; }
      

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();           
        }

        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {         
            Log.Information("OnConfigure finish");
        }
    }
}
