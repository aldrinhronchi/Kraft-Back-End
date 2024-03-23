using KaibaSystem_Back_End.Connections.Configurations;
using KaibaSystem_Back_End.Extensions.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KaibaSystem_Back_End.Connections.Database;
using KaibaSystem_Back_End.Services.Core.Interfaces;
using KaibaSystem_Back_End.Services.Core;

namespace KaibaSystem_Back_End.Extensions
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            services.AddScoped<ICoreService, CoreService>();

            #endregion Services

            #region Others

            ServiceLocator.IncluirServico(services.BuildServiceProvider());

            #endregion Others
        }

        public static void RegisterBuild(WebApplicationBuilder builder)
        {
            #region Context

            builder.Services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")).EnableSensitiveDataLogging();
            });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            #endregion Context

            #region Swagger

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();

            #endregion Swagger

            #region JWT

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            #endregion JWT

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "Origins",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200",
                                                          "http://localhost",
                                                          "https://localhost:4200",
                                                          "https://localhost");
                                      policy.AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowAnyOrigin()
                                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                                  });
            });

            #endregion CORS
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.AddMiddlewares();
            app.UseCors("Origins");

            #region Files

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = "/Files"
            });

            #endregion Files

            app.UseSwaggerConfiguration();

            #region Auth

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion Auth
        }
    }

    public static class MiddlewareRegistrationExtension
    {
        public static void AddMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}