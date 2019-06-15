using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using IdS4.CoreApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdS4.CoreApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddIdS4DbContext(Configuration);
            services.AddIdS4Authentication(Configuration);
            services.AddIdS4Cors(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(IdS4.Application.Mappers.UserProfile).Assembly);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule<MediatorModule>();
            container.RegisterModule<ApplicationModule>();
            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseIdS4Authentication();
            app.UseIdS4Cors();

            app.UseMvc();
        }
    }
}
