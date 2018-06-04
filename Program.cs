using Dna;
using Dna.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Net;

namespace Y.Bizz.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {

            //https://cmatskas.com/enforcing-https-only-traffic-with-asp-net-core-and-kestrel/
            return WebHost.CreateDefaultBuilder()
                // Add Dna Framework
                .UseDnaFramework(construct =>
                {
                    construct.AddFileLogger();
                })
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Loopback, 4001);
                    options.Listen(IPAddress.Loopback, 4000, listenOptions =>
                    {
                        listenOptions.UseHttps("development.pfx", "YHotel2018");
                    });
                })
                .UseStartup<Startup>()
                .Build();
        }

    }
    }