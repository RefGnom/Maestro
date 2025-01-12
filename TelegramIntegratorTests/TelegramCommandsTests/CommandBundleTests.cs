using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands;
using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.Implementation.Commands.TelegramCommandDescriptions;
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
        var telegramCommandDescriptions = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ITelegramCommandDescription)))
            .ToArray();
        var commandParsers = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ICommandParser)))
            .ToArray();
        var commandHandlers = exportedTypes
            .Where(x => x.GetInterfaces().Contains(typeof(ICommandHandler)))
            .ToArray();

        telegramCommandDescriptions.Should().HaveCount(commandParsers.Length);
        commandParsers.Should().HaveCount(commandHandlers.Length);
    }

    [Test]
    public void TestThatForEveryTelegramCommandExistBundle()
    {
        var commandNames = TelegramCommandNames.GetCommandNames();
        var telegramCommandDescriptions = ServiceProvider.GetServices<ITelegramCommandDescription>()
            .Select(x => x.TelegramCommandName)
            .ToArray();
        var commandParsers = ServiceProvider.GetServices<ICommandParser>()
            .Select(x => x.Name)
            .ToArray();
        var commandHandlers = ServiceProvider.GetServices<ICommandHandler>()
            .Select(x => x.Name)
            .ToArray();

        telegramCommandDescriptions.Should().BeEquivalentTo(commandNames);
        commandParsers.Should().BeEquivalentTo(commandNames);
        commandHandlers.Should().BeEquivalentTo(commandNames);
    }

    [TestCaseSource(nameof(GetCommandNames))]
    public void TestTelegramCommandMapperShouldReturnBundleIfCommandCorrect(string commandName)
    {
        var noise = Guid.NewGuid().ToString();
        var commandBundle = _telegramCommandMapper.MapCommandBundle(commandName + noise);
        commandBundle.Should().NotBeNull();
        commandBundle.TelegramCommandDescription.TelegramCommandName.Should().Be(commandName);
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