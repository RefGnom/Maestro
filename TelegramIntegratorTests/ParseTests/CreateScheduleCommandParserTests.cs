using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using NUnit.Framework.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.TelegramIntegratorTests.ParseTests
{
    public class CreateScheduleCommandParserTests : TestBase
    {
        private CreateScheduleCommandParser _parser;

        [SetUp]
        public void SetUp()
        {
            var commandParsers = ServiceProvider.GetRequiredService<IEnumerable<ICommandParser>>();
            _parser = (CreateScheduleCommandParser)commandParsers.First(x => x.CommandName == "/schedule");
        }

        [Test]
        public void TestParserShouldReturnSuccessResultIfCommandCorrect()
        {
            var command = "/schedule, 19.05.2025 10:00, 2:00, test schedule, overlap";

            var parseResult = _parser.ParseCommand(command, DateTime.Parse("19.05.2025 10:00"));

            parseResult.IsSuccessful.Should().BeTrue();

            var value = (CreateScheduleCommandModel)parseResult.Value;

            value.StartDateTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            value.Duration.Should().Be(TimeSpan.FromHours(2));
            value.ScheduleDescription.Should().Be("test schedule");
            value.CanOverlap.Should().BeTrue();
        }

        [Test]
        public void TestParserShouldReturnSuccessResultWithDefaultValues()
        {
            const bool canOverlap = false;

            var command = "/schedule, 19.05.2025 10:00, 2:00, test schedule";

            var parseResult = _parser.ParseCommand(command, DateTime.Parse("19.05.2025 10:00"));

            parseResult.IsSuccessful.Should().BeTrue();

            var value = (CreateScheduleCommandModel)parseResult.Value;

            value.CanOverlap.Should().Be(canOverlap);
        }
    }
}
