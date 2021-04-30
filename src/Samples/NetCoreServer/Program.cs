using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseKestrel()
      .UseUrls("https://wcfserv.mscore.local:8000", "http://wcfserv.mscore.local:9080")
       .UseNetTcp(9808)
      .UseStartup<Startup>();
    }
}
