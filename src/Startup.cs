using AutoMapper;
using CoreCodeCamp.Controllers;
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
                // opt.ApiVersionReader = new QueryStringApiVersionReader("ver");

                // This will read from the Header request.
                // opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");

                // Combine both
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("ver", "version"),
                    new HeaderApiVersionReader("X-Version"));

                // Version by Controller
                // opt.Conventions.Controller<TalksController>()
                //        .HasApiVersion(new ApiVersion(1, 0))
                //        .HasApiVersion(new ApiVersion(1, 1))
                //        .Action(c => c.Delete(default(string), default(int)))
                //            .MapToApiVersion(1, 1);

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
