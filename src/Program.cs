
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreCodeCamp
{
  public class Program
  {
    public static void Main(string[] args)
    {
        //CreateWebHostBuilder(args).Build().Run();
        CreateHostBuilder(args).Build().Run();
    }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //       WebHost.CreateDefaultBuilder(args)
        //           .UseStartup<Startup>();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

    }
}
