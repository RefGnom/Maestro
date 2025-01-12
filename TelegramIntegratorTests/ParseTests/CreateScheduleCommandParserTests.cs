using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using NUnit.Framework.Internal;

namespace Maestro.TelegramIntegratorTests.ParseTests
{
    public class CreateScheduleCommandParserTests
    {
        private CreateScheduleCommandParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new CreateScheduleCommandParser();
        }

        [Test]
        public void TestParserShouldReturnSuccessResultIfCommandCorrect()
        {
            var command = "/schedule, 19.05.2025 10:00, 19.05.2025 12:30, test schedule, overlap";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var value = (CreateScheduleCommandModel)parseResult.Value;

            value.StartDateTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            value.EndDateTime.Should().Be(new DateTime(2025, 5, 19, 12, 30, 0));
            value.ScheduleDescription.Should().Be("test schedule");
            value.CanOverlap.Should().BeTrue();
        }

        [Test]
        public void TestParserShouldReturnSuccessResultWithDefaultValues()
        {
            const bool canOverlap = false;

            var defaultRemindInterval = TimeSpan.FromMinutes(5);

            var command = "/schedule, 19.05.2025 10:00, 19.05.2025 12:30, test schedule";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var value = (CreateScheduleCommandModel)parseResult.Value;

            value.StartDateTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            value.EndDateTime.Should().Be(new DateTime(2025, 5, 19, 12, 30, 0));
            value.ScheduleDescription.Should().Be("test schedule");
            value.CanOverlap.Should().Be(canOverlap);
        }
    }
}
