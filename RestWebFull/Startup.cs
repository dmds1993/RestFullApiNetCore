using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWebFull.Domain;
using RestWebFull.Domain.Config;
using RestWebFull.Dtos;
using RestWebFull.Entities;
using RestWebFull.Middlewares;
using RestWebFull.Repositories;
using RestWebFull.Services;

namespace RestWebFull
{
    public class MyConfiguration
    {
        public string ConnectionStringDataBase { get; set; }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Configuration = configuration;

            _env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<MyConfiguration>(Configuration);
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerReader, CustomerReader>();
            services.AddSingleton(s => GetSettings<IDatabaseConfig, DataBaseConfig>(Configuration, "DataBase"));

            var sp = services.BuildServiceProvider();

            var databaseConfig = sp.GetService<IDatabaseConfig>();

            services.AddDbContext<PackDbContext>(options => options.UseSqlServer(databaseConfig.ConnectionString));
        }

        private static T GetSettings<T, I>(IConfiguration config, string settingsName)
            where I : T
        {
            var dbConnectionSettings = (I)Activator.CreateInstance(typeof(I));
            config.GetSection(settingsName).Bind(dbConnectionSettings);

            return dbConnectionSettings;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCustomMiddleware();
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
