using MediatR;
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
using QuestionServiceWebApi.CQRS.Updater.QUpdater;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Db.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using QuestionServiceWebApi.CQRS.Queries;

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
            services.AddSingleton<App>();
            services.AddScoped<EfCoreTagRepository>();
            services.AddScoped<EfCoreQuestionsRepository>();
            services.AddAuthentication();
            services.AddSingleton<IQuestionService, QuestionService>();
            services.AddMediatR(typeof(Startup),typeof(GetQuestionsQuery));
           
            // Register the Swagger services
            services.AddSwaggerDocument();



            var tags = Configuration.GetSection("Tags").AsEnumerable().Where(x=>x.Value!=null).Select(x=>x.Value);
            //services.AddSingleton<ITagUpdaterService, TagUpdaterService>(options=>new TagUpdaterService(60*1000*5, tags));

            var connectionString = Configuration["ConnectionStrings:Questions"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql(connectionString);
            services.AddHostedService<TagUpdaterService>(options=> new TagUpdaterService(60*1000*5,tags,optionsBuilder.Options));
            services.AddHostedService<QuestionUpdaterService>(options => new QuestionUpdaterService(60 * 1000 * 1, optionsBuilder.Options, options.GetService<IQuestionService>()));

            
            //services.AddDbContext<ApplicationContext>(options=>options.UseNpgsql(connectionString));

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            
          

            //IServiceProvider serviceProvider = services.BuildServiceProvider();
            //var qService = serviceProvider.GetRequiredService<QuestionService>();
            //var ss = serviceProvider.GetService<App>().RunAsync(qService).Result;
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

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
