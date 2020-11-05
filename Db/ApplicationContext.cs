using Microsoft.EntityFrameworkCore;
using QuestionServiceWebApi.Models;
using Serilog;

namespace QuestionServiceWebApi.Db
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<Tag> Tags { get; set; }

       

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
          
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {         
            Log.Information("OnConfigure finish");
        }
    }
}
