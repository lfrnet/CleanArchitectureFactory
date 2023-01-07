using Clean.Architecture.Factory.Cli.Infrastructure.Options;
using Clean.Architecture.Factory.Cli.Models;
using CliFx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Clean.Architecture.Factory.Cli.IntegrationTests
{
    public class Startup
    {
        public static IServiceProvider CreateServiceProvider<TCommand>() where TCommand : class, ICommand
        {
            var executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var globalFilePath = Path.Combine(executingAssemblyPath, Constants.GlobalFileName);
            var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ConfigFileName);

            var appsettingsFilePath = File.Exists(localFilePath)
                ? localFilePath
                : globalFilePath;

            var config = new ConfigurationBuilder()
                .AddJsonFile(appsettingsFilePath, true, true)
                .Build();

            var services = new ServiceCollection()
                .AddOptions()
                .Configure<GeneralSection>(config)
                .AddSingleton<IConfiguration>(config)
                .AddTransient<TCommand>();

            return services.BuildServiceProvider();
        }
    }
}
