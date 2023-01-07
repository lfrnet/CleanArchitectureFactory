using Clean.Architecture.Factory.Cli.Commands;
using Clean.Architecture.Factory.Cli.Infrastructure.Options;
using Clean.Architecture.Factory.Cli.Models;
using CliFx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Clean.Architecture.Factory.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            //var ProjectDirectory = "D:\\Develop\\projects\\Test\\gerarProjeto";
            //var webapiRelativePath = "src\\Fintech.Recharge.Api";

            ////        var result = await Cli.Wrap("path/to/exe")
            ////.WithArguments("--foo bar")
            ////.WithWorkingDirectory("work/dir/path")
            ////.ExecuteAsync();

            //var path = Path.Combine(ProjectDirectory, webapiRelativePath);

            //var result = await CliWrap.Cli.Wrap("echo")
            //    .WithWorkingDirectory(path)
            //    .WithArguments("some-text")
            //    .WithArguments(">")
            //    .WithArguments("appsettings.qa.json")
            //    .ExecuteAsync();



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
                .AddTransient<AddSolutionCommand>()
                .AddTransient<AddBackEndServiceCommand>()
                .AddTransient<AddWebAppCommand>()
                .AddTransient<ConfigSetCommand>()
                .AddTransient<ConfigListCommand>();

            using var serviceProvider = services.BuildServiceProvider();

            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(serviceProvider.GetService)
                .Build()
                .RunAsync();
        }
    }
}
