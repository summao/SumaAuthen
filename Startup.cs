using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Suma.Authen.Extensions;
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

            services.AddControllers();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            var mongoDbSettings = new MongoDbSettings();
            Configuration.Bind(nameof(MongoDbSettings), mongoDbSettings);
            services.AddMongoDb(mongoDbSettings);

            var appSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), appSettings);
            services.AddSingleton<RsaSecurityKey>(provider =>
            {
                // It's required to register the RSA key with depedency injection.
                // If you don't do this, the RSA instance will be prematurely disposed.
                RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(
                    source: Convert.FromBase64String(
                       appSettings.RsaPublicKey
                    ),
                    bytesRead: out int _
                );

                return new RsaSecurityKey(rsa);
            });

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = "Asymmetric";
                a.DefaultChallengeScheme = "Asymmetric";
            })
            .AddJwtBearer("Asymmetric", options =>
            {
                SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

                options.IncludeErrorDetails = true; // <- great for debugging

                // Configure the actual Bearer validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = rsa,
                    ValidAudience = "jwt-test",
                    ValidIssuer = "jwt-test",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
                    ValidateLifetime = true, // <- the "exp" will be validated
                    ValidateAudience = true,
                    ValidateIssuer = true,
                };
            });


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddScoped<IJwtManager, JwtManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Suma.Authen", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
