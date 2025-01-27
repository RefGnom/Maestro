﻿using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.View;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.TelegramIntegratorTests.ParseTests
{
    public class CreateReminderCommandParserTests : TestBase
    {
        private CreateReminderCommandParser _parser;

        [SetUp]
        public void SetUp()
        {
            var commandParsers = ServiceProvider.GetRequiredService<IEnumerable<ICommandParser>>();
            _parser = (CreateReminderCommandParser)commandParsers.First(x => x.CommandName == "/reminder");
        }

        [Test]
        public void TestParserShouldReturnSuccessResultIfCommandCorrect()
        {
            var command = "/reminder, 19.05.2025 10:00, test reminder, 3, 00:03:00";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var values = (CreateReminderCommandModel)parseResult.Value;

            values.ReminderTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            values.ReminderDescription.Should().Be("test reminder");
            values.RemindCount.Should().Be(3);
            values.RemindInterval.Should().Be(TimeSpan.FromHours(3));
        }

        [Test]
        public void TestParserShouldReturnSuccessResultWithDefaultValues()
        {
            const int defaultRemindCount = 1;
            var defaultRemindInterval = TimeSpan.FromMinutes(5);

            var command = "/reminder, 19.05.2025 10:00, test reminder";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeTrue();

            var values = (CreateReminderCommandModel)parseResult.Value;

            values.ReminderTime.Should().Be(new DateTime(2025, 5, 19, 10, 0, 0));
            values.ReminderDescription.Should().Be("test reminder");
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
        }

        [Test]
        public void TestParserShouldReturnParseTimeSpanFailureMessageIfRemindIntervalIncorrect()
        {
            var command = "/reminder, 19.05.2025 10:00, test reminder, 3, 1 минута";

            var parseResult = _parser.ParseCommand(command);

            parseResult.IsSuccessful.Should().BeFalse();
        }
    }
}
