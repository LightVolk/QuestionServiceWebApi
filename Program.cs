using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestionServiceWebApi.Controllers.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace QuestionServiceWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = new HostBuilder()
      .ConfigureAppConfiguration((hostContext, builder) =>
      {
          // Add other providers for JSON, etc.
          builder.AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.Development.json");

          if (hostContext.HostingEnvironment.IsDevelopment())
          {
              builder.AddUserSecrets<Program>();
          }

          

      }).ConfigureWebHostDefaults(webBuilder =>
      {
          webBuilder.UseStartup<Startup>();
          
      });
            return host;
            //Host.CreateDefaultBuilder(args)
            //    .ConfigureWebHostDefaults(webBuilder =>
            //    {
            //        webBuilder.UseStartup<Startup>();
            //    });
        }
    }
}
