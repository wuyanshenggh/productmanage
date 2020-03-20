using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ProductMange.BLL.OneKeyUpgrade;
using ProductMange.Model;
using ProductMange.Public.OutInterface;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductMange
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));


            services.AddDbContext<ProductManageDbContext>(options =>
            {
                options.UseMySQL(Configuration["AppSettings:ConnStrMysql"]);
            });

            services.AddScoped<IBLLUpgrade, BLLUpgrade>();

            services.AddMvc(op =>
            {
                op.Filters.Add(new CustomActionFilter());
                op.Filters.Add(new OutInterfaceActionFilter());
                op.Filters.Add(new CustomExceptionFilter());

            }).AddJsonOptions(op =>
            {
                op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(); });
            services.AddMemoryCache();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
     );

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ProductManageNew.xml");
                c.IncludeXmlComments(filePath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            
            app.UseSession();


            app.UseCors("cors");
            app.UseHttpsRedirection();
            app.UseMvc();

            // Swagger
            // 启用Swagger中间件
            app.UseSwagger();


            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreWebApi");
                c.RoutePrefix = string.Empty;
            });       

          


        }
    }
}
