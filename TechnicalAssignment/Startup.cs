using AutoMapper;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.Base;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Services.Implementation;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalAssignment.Validator;

namespace TechnicalAssignment
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<ITransactionService, TransactionServices>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins( "http://localhost:3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            }); // Make sure you call this previous to AddMvc
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidateModelStateFilter));

            }).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
        
            app.UseCors(
                options => options.WithOrigins("http://example.com").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/api/Transaction/SampleReturn",null);
                endpoints.MapControllers();

            });

        }
    }
}
