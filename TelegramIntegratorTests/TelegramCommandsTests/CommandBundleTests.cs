using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands;
using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.TelegramIntegratorTests.TelegramCommandsTests;

public class CommandBundleTests : TestBase
{
    private ITelegramCommandMapper _telegramCommandMapper;
    
    [SetUp]
    public void SetUp()
    {
        _telegramCommandMapper = ServiceProvider.GetRequiredService<ITelegramCommandMapper>();
    }

    [Test]
    public void TestThatAllBundlesSourcesEquivalents()
    {
        var exportedTypes = ServiceAssembly.GetExportedTypes();
        var commandModels = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ICommandModel)))
            .ToArray();
        var commandParsers = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ICommandParser)))
            .ToArray();
        var commandHandlers = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ICommandHandler)))
            .ToArray();

        commandModels.Should().HaveCount(commandParsers.Length);
        commandParsers.Should().HaveCount(commandHandlers.Length);
    }

    [Test]
    public void TestThatForEveryTelegramCommandExistBundle()
    {
        var commandNames = TelegramCommandNames.GetCommandNames();
        var commandModels = ServiceProvider.GetServices<ICommandModel>()
            .Select(x => x.TelegramCommand)
            .ToArray();
        var commandParsers = ServiceProvider.GetServices<ICommandParser>()
            .Select(x => x.CommandName)
            .ToArray();
        var commandHandlers = ServiceProvider.GetServices<ICommandHandler>()
            .Select(x => x.CommandName)
            .ToArray();

        commandModels.Should().BeEquivalentTo(commandNames);
        commandParsers.Should().BeEquivalentTo(commandNames);
        commandHandlers.Should().BeEquivalentTo(commandNames);
    }

    [TestCaseSource(nameof(GetCommandNames))]
    public void TestTelegramCommandMapperShouldReturnBundleIfCommandCorrect(string commandName)
    {
        var noise = Guid.NewGuid().ToString();
        var commandBundle = _telegramCommandMapper.MapCommandBundle(commandName + " " + noise);
        commandBundle.Should().NotBeNull();
    }

    [Test]
    public void TestTelegramCommandMapperShouldReturnNullIfCommandIncorrect()
    {
        var command = $"/{Guid.NewGuid()}";
        var noise = Guid.NewGuid().ToString();
        var commandBundle = _telegramCommandMapper.MapCommandBundle(command + noise);
        commandBundle.Should().BeNull();
    }

    private static string[] GetCommandNames() => TelegramCommandNames.GetCommandNames();
}