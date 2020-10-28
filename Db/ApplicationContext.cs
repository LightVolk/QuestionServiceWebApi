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
        private string _questionServiceConnectionString { get; set; }

        public ApplicationContext(IConfiguration configuration,string secretKey)
        {
            Database.EnsureCreated();
            _questionServiceConnectionString = configuration["ConnectionStrings:Questions"];
            _questionServiceConnectionString = $"{_questionServiceConnectionString}Password:{secretKey}";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"{_questionServiceConnectionString}");
            Log.Information("OnConfigure finish");
        }
    }
}
