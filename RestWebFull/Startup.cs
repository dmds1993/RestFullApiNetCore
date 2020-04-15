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
using RestWebFull.Repositories;
using RestWebFull.Services;
using System.IdentityModel.Tokens.Jwt;
using RestWebFull.Services.interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerReader, CustomerReader>();
            services.AddScoped<ICustomerUpdater, CustomerUpdater>();
            services.AddScoped<ICreatorCustomer, CreatorCustomer>();
            services.AddSingleton(s => GetSettings<IDatabaseConfig, DataBaseConfig>(Configuration, "DataBase"));

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "My first WebAPI", Version = "v1" });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("resourcesAdmin", policyAdmin => {
                    policyAdmin.RequireClaim("role", "resources.admin");
                });
                options.AddPolicy("resourcesUser", policyAdmin => {
                    policyAdmin.RequireClaim("role", "resources.user");
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("resourcesAdmin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "resources.admin");
                });
                options.AddPolicy("resourcesUser", policyUser =>
                {
                    policyUser.RequireClaim("role", "resources.user");
                });
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddDbContext<PackDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISeedDataService, SeedDataService>();

            services.AddControllersWithViews()
            .AddNewtonsoftJson();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5000";
                o.Audience = "apiApp";
                o.RequireHttpsMetadata = false;
            });

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
