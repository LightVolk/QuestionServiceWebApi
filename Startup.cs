using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestionServiceWebApi.Controllers.Infrastructure;
using QuestionServiceWebApi.CQRS.Updater;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Db.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;         
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var connectionString = Configuration["QuestionPostgresP"];
            
           
            
            services.AddAuthorization();
            services.AddControllers();
            services.AddHttpClient();
            services.AddSingleton<QuestionService>();
            services.AddSingleton<App>();
            services.AddScoped<EfCoreTagRepository>();
            services.AddAuthentication();

            var tags = Configuration.GetSection("Tags").AsEnumerable().Where(x=>x.Value!=null).Select(x=>x.Value);
            //services.AddSingleton<ITagUpdaterService, TagUpdaterService>(options=>new TagUpdaterService(60*1000*5, tags));

            var connectionString = Configuration["ConnectionStrings:Questions"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql(connectionString);
            services.AddHostedService<TagUpdaterService>(options=> new TagUpdaterService(60*1000*5,tags,optionsBuilder.Options));

            
            //services.AddDbContext<ApplicationContext>(options=>options.UseNpgsql(connectionString));

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            
            //services.AddSingleton<ApplicationContext>(x => new ApplicationContext(Configuration,_questionPostgresSecret));

            //var builder = new SqlConnectionStringBuilder(
            //Configuration.GetConnectionString("PostgresPassword"));
            //builder.Password = Configuration["DbPassword"];

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var qService = serviceProvider.GetRequiredService<QuestionService>();
            var ss = serviceProvider.GetService<App>().RunAsync(qService).Result;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
