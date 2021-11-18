using System.Text;
using hAngular_Project.Middleware;
using hAngular_Project.PolicyHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.DataAccess;
using ToMo.hAngularProject.DataAccess.Entities;
using ToMo.hAngularProject.DataAccess.Repositories;
using ToMo.hAngularProject.Domain.IRepositories;
using ToMo.hAngularProject.Domain.Services;
using ToMo.hAngularProject.Security;
using ToMo.hAngularProject.Security.Model;
using ToMo.hAngularProject.Security.Services;

namespace hAngular_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "hAngular.WebApi", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });
            services.AddDbContext<MainDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=MoTo.db"); 
            });
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=AuthDb.db"); 
            });
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });
            services.AddSingleton<IAuthorizationHandler, CanWriteProductsHandler>();
            services.AddSingleton<IAuthorizationHandler, CanReadProductsHandler>();
            services.AddSingleton<IAuthorizationHandler, CanEditProductsHandler>();
            services.AddSingleton<IAuthorizationHandler, CanRemoveProductsHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(CanWriteProductsHandler), 
                    policy => policy.Requirements.Add(new CanWriteProductsHandler()));
                options.AddPolicy(nameof(CanReadProductsHandler), 
                    policy => policy.Requirements.Add(new CanReadProductsHandler()));
                options.AddPolicy(nameof(CanEditProductsHandler), 
                    policy => policy.Requirements.Add(new CanEditProductsHandler()));
                options.AddPolicy(nameof(CanRemoveProductsHandler), 
                    policy => policy.Requirements.Add(new CanRemoveProductsHandler()));
            });
            services.AddCors(opt => opt
                .AddPolicy("dev-policy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext context, AuthDbContext authDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToMo.hAngularProject.WebApi v1"));
                app.UseCors("dev-policy");
                new DbSeeder(context).SeedDevelopment();
                
                #region Setup AuthContext
                
                authDbContext.Database.EnsureDeleted();
                authDbContext.Database.EnsureCreated();
                authDbContext.LoginUsers.Add(new LoginUser
                {
                    UserName = "admin",
                    HashedPassword = "admin",
                    DbUserId = 1,
                });
                authDbContext.LoginUsers.Add(new LoginUser
                {
                    UserName = "user",
                    HashedPassword = "user",
                    DbUserId = 2,
                });
                authDbContext.LoginUsers.Add(new LoginUser
                {
                    UserName = "moderator",
                    HashedPassword = "moderator",
                    DbUserId = 3,
                });
                authDbContext.Permissions.AddRange(new Permission()
                {
                    Name = "CanWriteProducts"
                },new Permission()
                {
                    Name = "CanRemoveProducts"
                },new Permission()
                {
                    Name = "CanEditProducts"
                }, new Permission()
                {
                    Name = "CanReadProducts"
                });
                authDbContext.SaveChanges();
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 1, UserId = 1 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 2, UserId = 1 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 3, UserId = 1 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 4, UserId = 1 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 4, UserId = 2 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 3, UserId = 3 });
                authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 4, UserId = 3 });
                authDbContext.SaveChanges();
                

                #endregion
            }
            else
            {
                app.UseCors("Prod-cors");
                new DbSeeder(context).SeedProduction();
            }
            
            app.UseMiddleware<JWTMiddleware>();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}