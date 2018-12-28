using Dna;
using Dna.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Net;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateBuildWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateBuildWebHostBuilder(string[] args)
        {

            //https://cmatskas.com/enforcing-https-only-traffic-with-asp-net-core-and-kestrel/
            return WebHost.CreateDefaultBuilder()
                // Add Dna Framework
                .UseDnaFramework(construct =>
                {
                    construct.AddFileLogger();
                })
                .UseIISIntegration()
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Loopback, 7001);
                    options.Listen(IPAddress.Loopback, 7000, listenOptions =>
                    {
                        //listenOptions.UseHttps("development.pfx", "YHotel2018");
                    });
                })
                .UseStartup<Startup>();
                
        }

    }
    }