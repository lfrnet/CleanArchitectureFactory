using Clean.Architecture.Factory.Cli.Commands;
using CliFx;
using System.Collections.Generic;
using Xunit;

namespace Clean.Architecture.Factory.Cli.IntegrationTests.Commands
{
    public class ConfigListCommandTests
    {
        [Fact]
        public async void RunAsync_ShouldListTheSettingsInTheOutput()
        {
            // Arrange
            var serviceProvider = Startup.CreateServiceProvider<ConfigListCommand>();
            var (console, output, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<ConfigListCommand>()
                .UseTypeActivator(serviceProvider.GetService)
                .UseConsole(console)
                .Build();

            // config list
            var args = new[] { "config", "list" };
            var envVars = new Dictionary<string, string>();

            // Act
            await app.RunAsync(args, envVars);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(output.GetString()));
        }
    }
}
