using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KickAss_Music_Player
{
    public class Program
    {
        protected static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
