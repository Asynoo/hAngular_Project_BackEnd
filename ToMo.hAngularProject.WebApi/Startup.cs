using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.DataAccess;
using ToMo.hAngularProject.DataAccess.Entities;
using ToMo.hAngularProject.DataAccess.Repositories;
using ToMo.hAngularProject.Domain.IRepositories;
using ToMo.hAngularProject.Domain.Services;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToMo.hAngularProject.WebApi", Version = "v1" });
            });
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            
            services.AddDbContext<MainDbContext>(options => { options.UseSqlite("Data Source=MoTo.db");});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToMo.hAngularProject.WebApi v1"));
                new DbSeeder(context).SeedDevelopment();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}