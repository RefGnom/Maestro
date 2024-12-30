using FluentAssertions;
using Maestro.Client;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.Models;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

[TestFixture]
public class ApiClientTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string uri = "http://localhost:5000/api/v1/";
        const string apiKey = "00000000000000000000000000000000";

        _apiClient = new ApiClient(uri, apiKey,
            // Substitute.For<ILogFactory>()
            new LogFactory(new DateTimeProvider(), new Writer())
        );
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _apiClient.Dispose();
    }
    private ApiClient _apiClient;

    [Test]
    public async Task Reminders_should_create_and_get_equivalent_reminders_for_user()
    {
        const int userId = 1;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                ReminderTime = new DateTime(year: 2024, month: 1, day: 1),
                ReminderTimeDuration = TimeSpan.FromMinutes(1),
                IsCompleted = true,
                IsRepeatable = true
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                ReminderTime = new DateTime(year: 2024, month: 4, day: 3),
                ReminderTimeDuration = TimeSpan.FromMinutes(2),
                IsCompleted = true,
                IsRepeatable = false
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                ReminderTime = new DateTime(year: 2024, month: 1, day: 2),
                ReminderTimeDuration = TimeSpan.FromMinutes(3),
                IsCompleted = false,
                IsRepeatable = false
            }
        };

        var createdRemindersId = new List<long>();

        foreach (var reminder in remindersCreateList)
        {
            var createdReminderId = await _apiClient.CreateReminderAsync(reminder);
            createdRemindersId.Add(createdReminderId);
        }

        var remindersForUser = await _apiClient.GetRemindersForUserAsync(userId).ToListAsync();

        var filteredRemindersForUser = remindersForUser
            .Where(reminder => createdRemindersId.Contains(reminder.Id))
            .ToList();

        remindersCreateList.Should().BeEquivalentTo(filteredRemindersForUser, options => { return options.Excluding(x => x.Id); });
    }

    [Test]
    public async Task Reminders_should_create_and_get_equivalent_reminder()
    {
        var reminder = new ReminderDto
        {
            UserId = 2,
            Description = "Test",
            ReminderTime = new DateTime(year: 2024, month: 1, day: 2),
            ReminderTimeDuration = TimeSpan.FromMinutes(1),
            IsCompleted = true,
            IsRepeatable = false
        };

        var createdReminderId = await _apiClient.CreateReminderAsync(reminder);

        var createdReminder = await _apiClient.GetReminderAsync(createdReminderId);

        createdReminder.Should().BeEquivalentTo(reminder);
    }
}