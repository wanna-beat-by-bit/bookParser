using Microsoft.Extensions.Hosting;
using bookParser.startup;
using Microsoft.AspNetCore.Hosting;

namespace bookParser{
    class Program{
        static void Main(string[] args){
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
