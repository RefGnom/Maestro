using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands;

namespace Maestro.TelegramIntegratorTests.TelegramCommandsTests;

public class TelegramCommandNamesTests
{
    [Test]
    public void TestGetCommandNames()
    {
        var names = TelegramCommandNames.GetCommandNames();
        names.Should().Contain("/reminder");
        names.Should().Contain("/schedule");

        var memberInfos = typeof(TelegramCommandNames).GetMembers();
        const int countObjectMembers = 4;
        const int countClassMethods = 1;
        names.Should().HaveCount(memberInfos.Length - countObjectMembers - countClassMethods);
    }
}