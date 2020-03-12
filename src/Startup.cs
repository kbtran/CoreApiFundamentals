using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodeCamp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CampContext>();
            services.AddScoped<ICampRepository, CampRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddApiVersioning(opt => 
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(2, 0);
                opt.ReportApiVersions = true;
                // The "ver" can be used instead of the default api-version
                opt.ApiVersionReader = new QueryStringApiVersionReader("ver");
            });

            //services.AddMvc(option => option.EnableEndpointRouting = false);
            //services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddControllers();
        }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

            //app.UseMvc();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
      }
}
