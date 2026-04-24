using Microsoft.EntityFrameworkCore;
using GoodHamburger.Api.Middlewares;
using GoodHamburger.Infrastructure.Context;
using GoodHamburger.Infrastructure.Configuration;
using System;


namespace GoodHamburger.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            
            services.AddDbContext<ApiDbContext>(o => {
                o.UseNpgsql(ApiConfiguration.GetDatabaseConfig().ConnectionString);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.AddHealthChecks();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalErrorHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
