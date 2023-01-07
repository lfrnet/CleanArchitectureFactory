using Clean.Architecture.Factory.Cli.Commands;
using CliFx;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Clean.Architecture.Factory.Cli.IntegrationTests.Commands
{
    public class InitializeCommandTests
    {
        [Fact]
        public async void RunAsync_ShouldCreateFolderStructure()
        {
            // Arrange
            var serviceProvider = Startup.CreateServiceProvider<AddSolutionCommand>();
            var (console, output, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<AddSolutionCommand>()
                .UseTypeActivator(serviceProvider.GetService)
                .UseConsole(console)
                .Build();

            // init "eShopOnWeb"
            var args = new[] { "init", "eShopOnWeb" };
            var envVars = new Dictionary<string, string>();

            // Act
            await app.RunAsync(args, envVars);

            // Assert
            Assert.True(Directory.Exists("eShopOnWeb"));
            Assert.True(Directory.Exists("eShopOnWeb/src"));
            Assert.True(Directory.Exists("eShopOnWeb/test"));

            // Cleanup
            if (Directory.Exists("eShopOnWeb")) Directory.Delete("eShopOnWeb", true);
        }
    }
}
