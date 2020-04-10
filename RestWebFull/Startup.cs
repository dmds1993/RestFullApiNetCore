using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestWebFull.Domain;
using RestWebFull.Domain.Config;
using RestWebFull.Dtos;
using RestWebFull.Entities;
using RestWebFull.Middlewares;
using RestWebFull.Models;
using RestWebFull.Repositories;
using RestWebFull.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;

namespace RestWebFull
{
    public class MyConfiguration
    {
        public string ConnectionStringDataBase { get; set; }
    }

    public class Startup
    {
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
            services.AddMvc(option => {
                option.EnableEndpointRouting = false;
                option.ReturnHttpNotAcceptable = true;
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerReader, CustomerReader>();
            services.AddScoped<ICustomerUpdater, CustomerUpdater>();
            services.AddScoped<ICreatorCustomer, CreatorCustomer>();
            services.AddSingleton(s => GetSettings<IDatabaseConfig, DataBaseConfig>(Configuration, "DataBase"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My first WebAPI", Version = "v1" });
            });


            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddControllersWithViews()
            .AddNewtonsoftJson();

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

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            });

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
