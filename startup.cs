using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;

using bookParser.Models;
using bookParser.repository;
using bookParser.Logic;
using bookParser.Parser;

namespace bookParser.startup{
    public class Startup
    {
        public IConfiguration Configuration {get;}
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);

            var connectionString = "server=localhost;port=5436;userid=postgres;database=postgres;password=qwerty";

            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.ConnectionString));
            services.AddScoped<IRepository>(_ => new Repository(builder.ConnectionString));
            services.AddScoped<IBLogic, BLogic>();
            services.AddScoped<IParser, parserLabirint>();
            services.AddScoped<parserIgraslov>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                        webBuilder.UseStartup<Startup>();
                    }
                );
    }
}