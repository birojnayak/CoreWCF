﻿using System.Diagnostics;
using System.Net;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreServer
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
    .UseKestrel(
    //options =>
    //{
    //    options.ListenAnyIP(8000);
    //    options.Listen(address: IPAddress.Loopback, 8000,
    //                listenOptions =>
    //    {
    //        listenOptions.UseHttps();
    //    });
    //}
    )
    .UseUrls("https://wcfserv.mscore.local:8000", "http://wcfserv.mscore.local:9080")
     .UseNetTcp(9808)
    .UseStartup<Startup>();*/
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options => {
                options.ListenLocalhost(8088);
                options.Listen(address: IPAddress.Loopback, 8443, listenOptions =>
                {
                    listenOptions.UseHttps(httpsOptions =>
                    {

#if NET472
                        httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
#endif // NET472
                    });
                    if (Debugger.IsAttached)
                    {
                        listenOptions.UseConnectionLogging();
                    }
                });
            })
            .UseNetTcp(8089)
            .UseStartup<Startup>();
    }
}
