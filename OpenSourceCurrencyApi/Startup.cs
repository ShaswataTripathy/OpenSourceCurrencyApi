using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi
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
            services.AddControllers();

            var gitHubUrl = Configuration["GitHubProfile"];
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Demo Currency API",
                    Version = "v1",
                    Description = "Using https://github.com/fawazahmed0/currency-api in Backend",
                    Contact = new OpenApiContact
                    {
                        Name = "Shaswata Tripthy",
                        Email = string.Empty,
                        Url = new Uri(gitHubUrl),
                    },
                });
            });

            var allowedOrigin = Configuration["AllowedOrigin"];

            services.AddCors(options =>
            {
                options.AddPolicy("CorsApi",
                    builder => builder.WithOrigins(allowedOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddHttpClient<IGitHubClient, GitHubClient>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Currency API V1");
            });
            app.UseHttpsRedirection();



            app.UseRouting();
            app.UseCors("CorsApi");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


