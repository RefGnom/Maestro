using FluentAssertions;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;

namespace Maestro.TelegramIntegratorTests.ParseTests
{
    public class ParseDateTimeTest
    {
        [TestCase("17.01.2025 11:00", "17.01.2025 10:00", "17.01.2025 11:00")]
        [TestCase("17.01 11:00", "17.01.2025 10:00", "17.01.2025 11:00")]
        [TestCase("11:00", "17.01.2025 12:00", "18.01.2025 11:00")]
        [TestCase("11:00", "17.01.2025 10:00", "17.01.2025 11:00")]
        [TestCase("11.00", "17.01.2025 10:00", "17.01.2025 11:00")]
        [TestCase("11", "17.01.2025 10:00", "17.01.2025 11:00")]
        [TestCase("11", "17.01.2025 12:00", "18.01.2025 11:00")]
        public void ParseDateTime_ShouldReturnCorrectParsedDateTime(string stringToParse, string dateTimeNowString, string expectedParsedDateTimeString)
        {
            var dateTimeNow = DateTime.Parse(dateTimeNowString);
            var expectedParsedDateTime = DateTime.Parse(expectedParsedDateTimeString);
            var result = ParserHelper.ParseDateTime(stringToParse, dateTimeNow);

            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().Be(expectedParsedDateTime);
        }
    }
}
