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
using QuestionServiceWebApi.Models;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using QuestionServiceWebApi.Infrastructure.LoggerInfrastructure;

namespace QuestionServiceWebApi
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;         
        }

        public IConfiguration Configuration { get; }
    
        public static readonly ILoggerFactory EfCoreLoggerFactory = LoggerFactory.Create(builer =>
        {
            builer.AddProvider(new EfCoreLoggerProvider());
        });

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
            services.AddScoped<ApplicationContext>();
           
           
            //EfCoreTagRepository tagRepository, EfCoreQuestionsRepository questionsRepository
            // Register the Swagger services
            services.AddSwaggerDocument();



            var tags = Configuration.GetSection("Tags").AsEnumerable().Where(x=>x.Value!=null).Select(x=>x.Value);
            //services.AddSingleton<ITagUpdaterService, TagUpdaterService>(options=>new TagUpdaterService(60*1000*5, tags));

            var connectionString = Configuration["ConnectionStrings:Questions"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseLoggerFactory(EfCoreLoggerFactory);
         //   optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql(connectionString);


            // ON on production. Off when debugging sql queries
            //services.AddHostedService<TagUpdaterService>(options=> new TagUpdaterService(60*1000*5,tags,optionsBuilder.Options));
            //services.AddHostedService<QuestionUpdaterService>(options => new QuestionUpdaterService(60 * 1000 * 1, optionsBuilder.Options, options.GetService<IQuestionService>()));
           
            
            //services.AddDbContext<ApplicationContext>(options=>options.UseNpgsql(connectionString));

            services.AddScoped<DbContextOptions<ApplicationContext>>(x=> optionsBuilder.Options);
           
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(Question));
            services.AddMediatR(typeof(IAsyncEnumerable<Question>));
            services.AddMediatR(typeof(DbContext));
            services.AddMediatR(typeof(DbContextOptions<ApplicationContext>));
            services.AddMediatR(typeof(DbContextOptions));
            services.AddMediatR(typeof(ApplicationContext));
            services.AddMediatR(typeof(GetQuestionsQuery));
            services.AddMediatR(typeof(EfCoreQuestionsRepository));
            
            
            


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
