using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Deliveggie.backend
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //building configurations
            var builder = new ConfigurationBuilder()                
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(s => s.AddAutofac())
                .UseStartup<Startup>()
                .Build();
    }
}
