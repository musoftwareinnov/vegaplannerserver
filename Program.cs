using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace vega
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }   

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => { options.Limits.MaxRequestBodySize = 1000000000; } ) //For large images uploads 100MB
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                    {
                        var builtConfig = config.Build();
                            var azureServiceTokenProvider = new AzureServiceTokenProvider();
                            var keyVaultClient = new KeyVaultClient(
                                new KeyVaultClient.AuthenticationCallback(
                                    azureServiceTokenProvider.KeyVaultTokenCallback));
                            Console.WriteLine("config=" + builtConfig["KeyVaultName"]);
                            config.AddAzureKeyVault(
                                // $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                                "https://vegaplannerscds.vault.azure.net/",
                                keyVaultClient,
                                new DefaultKeyVaultSecretManager());
                    }
                })
                .UseStartup<Startup>();

        private static string GetKeyVaultEndpoint() => "https://vegaplannerscds.vault.azure.net";
    }
}
