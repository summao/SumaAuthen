using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SumaAuthen.Databases;
using SumaAuthen.Repositories;
using SumaAuthen.Services;

namespace SumaAuthen
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

            services.AddScoped<IAccountRepositories, AccountRepositories>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtManager, JwtManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SumaAuthen", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SumaAuthen v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
