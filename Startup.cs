using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Suma.Authen.Databases;
using Suma.Authen.Helpers;
using Suma.Authen.Repositories;
using Suma.Authen.Services;

namespace Suma.Authen
{
    public class Startup
    {
        readonly string _allowOnlyWeb = "AllowOnlyWeb";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowOnlyWeb,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });
            services.AddDbContext<MysqlDataContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("Mysql"),
                    new MySqlServerVersion(new Version(8, 0, 21)),
                     mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });
            services.AddControllers();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSingleton<IMongoDatabase>(_ => {
                var mongoDbSettings = Configuration.GetSection("MongoDbSettings");
                var client = new MongoClient(mongoDbSettings.GetValue<string>("ConnectionString"));
                return client.GetDatabase(mongoDbSettings.GetValue<string>("DatabaseName"));
            });

            services.AddScoped<IAccountRepositories, AccountRepositories>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtManager, JwtManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Suma.Authen", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Suma.Authen v1"));
            }

            app.UseRouting();

            app.UseCors(_allowOnlyWeb);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
