using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;
using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;
using NUnit.Framework.Internal;

namespace Maestro.TelegramIntegratorTests.ParseTests
{
    public class CreateReminderCommandParserTests
    {
        private CreateReminderCommandParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new CreateReminderCommandParser();
        }

        [Test]
        public void TestParserShouldReturnSuccessResultIfCommandCorrect()
        {
            var command = "/reminder, 19.05.2025 10:00, test reminder, 3, 00:03:00";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var values = (CreateReminderCommand)parseResult.Value;

            values.ReminderTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            values.Description.Should().Be("test reminder");
            values.RemindCount.Should().Be(3);
            values.RemindInterval.Should().Be(TimeSpan.FromMinutes(3));
        }

        [Test]
        public void TestParserShouldReturnSuccessResultWithDefaultValues()
        {
            const int defaultRemindCount = 1;
            var defaultRemindInterval = TimeSpan.FromMinutes(5);

            var command = "/reminder, 19.05.2025 10:00, test reminder";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var values = (CreateReminderCommand)parseResult.Value;

            values.ReminderTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            values.Description.Should().Be("test reminder");
            values.RemindCount.Should().Be(defaultRemindCount);
            values.RemindInterval.Should().Be(defaultRemindInterval);
        }

        [Test]
        public void TestParserShouldReturnFailureResultIfCommandIncorrect()
        {
            var command = "/reminder, 19.05.2025 10:00";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeFalse();
            parseResult.ParseFailureMessage.Should().Be(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern));
        }

        [Test]
        public void TestParserShouldReturnParseDateTimeFailureMessageIfRemindDateTimeIncorrect()
        {
            var command = "/reminder, 19 мая 10:00, test reminder";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeFalse();
            parseResult.ParseFailureMessage.Should().Be(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
        }

        [Test]
        public void TestParserShouldReturnParseIntFailureMessageIfRemindCountIncorrect()
        {
            var command = "/reminder, 19.05.2025 10:00, test reminder, -1";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeFalse();
            parseResult.ParseFailureMessage.Should().Be(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseIntFailureMessage));
        }

        [Test]
        public void TestParserShouldReturnParseTimeSpanFailureMessageIfRemindIntervalIncorrect()
        {
            var command = "/reminder, 19.05.2025 10:00, test reminder, 3, 1 минута";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeFalse();
            parseResult.ParseFailureMessage.Should().Be(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseTimeSpanFailureMessage));
        }
    }
}
