﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Helpers
{
    public static class ServiceHelper
    {
        public static IWebHostBuilder CreateWebHostBuilder<TStartup>(ITestOutputHelper outputHelper) where TStartup : class =>
            WebHost.CreateDefaultBuilder(Array.Empty<string>())
#if DEBUG
            .ConfigureLogging((ILoggingBuilder logging) =>
            {
                logging.AddProvider(new XunitLoggerProvider(outputHelper));
                logging.AddFilter("Default", LogLevel.Debug);
                logging.AddFilter("Microsoft", LogLevel.Debug);
                logging.SetMinimumLevel(LogLevel.Debug);
            })
#endif // DEBUG
            .UseNetTcp(0)
            .UseStartup<TStartup>();

        public static string GetNetTcpAddressInUse(this IWebHost host)
        {
            return $"net.tcp://localhost:{host.GetNetTcpPortInUse()}";
        }

        public static int GetNetTcpPortInUse(this IWebHost host)
        {
            System.Collections.Generic.ICollection<string> addresses = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            var addressInUse = new Uri(addresses.First(), UriKind.Absolute);
            return addressInUse.Port;
        }

        //only for test, don't use in production code
        public static X509Certificate2 GetTestCertificate()
        {
            String TEST_CERT_BASE_64 = "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";
            var cert = new X509Certificate2(Convert.FromBase64String(TEST_CERT_BASE_64), "corewcftest");
            return cert;
        }

        public static void CloseServiceModelObjects(params System.ServiceModel.ICommunicationObject[] objects)
        {
            foreach (System.ServiceModel.ICommunicationObject comObj in objects)
            {
                try
                {
                    if (comObj == null)
                    {
                        continue;
                    }
                    // Only want to call Close if it is in the Opened state
                    if (comObj.State == System.ServiceModel.CommunicationState.Opened)
                    {
                        comObj.Close();
                    }
                    // Anything not closed by this point should be aborted
                    if (comObj.State != System.ServiceModel.CommunicationState.Closed)
                    {
                        comObj.Abort();
                    }
                }
                catch (TimeoutException)
                {
                    comObj.Abort();
                }
                catch (System.ServiceModel.CommunicationException)
                {
                    comObj.Abort();
                }
            }
        }
    }
}
