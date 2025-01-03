using FluentAssertions;
using Maestro.Client;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.Models;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

[TestFixture]
public class MaestroApiClientTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string uri = "http://localhost:5000/api/v1/";
        const string apiKey = "00000000000000000000000000000000";

        _maestroApiClient = new MaestroApiClient(uri, apiKey,
            // Substitute.For<ILogFactory>()
            new LogFactory(new DateTimeProvider(), new Writer())
        );
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _maestroApiClient.Dispose();
    }
    private MaestroApiClient _maestroApiClient;

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
                ReminderTime = new DateTime(2024, 1, 1),
                ReminderTimeDuration = TimeSpan.FromMinutes(1),
                IsCompleted = true,
                IsRepeatable = true
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                ReminderTime = new DateTime(2024, 4, 3),
                ReminderTimeDuration = TimeSpan.FromMinutes(2),
                IsCompleted = true,
                IsRepeatable = false
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                ReminderTime = new DateTime(2024, 1, 2),
                ReminderTimeDuration = TimeSpan.FromMinutes(3),
                IsCompleted = false,
                IsRepeatable = false
            }
        };

        var createdRemindersId = new List<long>();

        foreach (var reminder in remindersCreateList)
        {
            var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminder);
            createdRemindersId.Add(createdReminderId);
        }

        var remindersForUser = await _maestroApiClient.GetRemindersForUserAsync(userId).ToListAsync();

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
            ReminderTime = new DateTime(2024, 1, 2),
            ReminderTimeDuration = TimeSpan.FromMinutes(1),
            IsCompleted = true,
            IsRepeatable = false
        };

        var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminder);

        var createdReminder = await _maestroApiClient.GetReminderAsync(createdReminderId);

        createdReminder.Should().BeEquivalentTo(reminder);
    }

    [Test]
    public async Task Reminders_should_marked_as_complited_and_get_reminders_for_user_in_time_range()
    {
        const int userId = 3;
        DateTime inclusiveStartDate = new DateTime(2025, 1, 1);
        DateTime exclusiveEndDate = new DateTime(2024, 1, 4);

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                ReminderTime = new DateTime(2025, 1, 1),
                ReminderTimeDuration = TimeSpan.FromMinutes(1),
                IsCompleted = false,
                IsRepeatable = true
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                ReminderTime = new DateTime(2025, 1, 2),
                ReminderTimeDuration = TimeSpan.FromMinutes(2),
                IsCompleted = false,
                IsRepeatable = false
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                ReminderTime = new DateTime(2025, 1, 3),
                ReminderTimeDuration = TimeSpan.FromMinutes(3),
                IsCompleted = false,
                IsRepeatable = false
            }
        };

        var createdRemindersId = new List<long>();

        foreach (var reminder in remindersCreateList)
        {
            var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminder);
            createdRemindersId.Add(createdReminderId);
            await _maestroApiClient.MarkRemindersAsCompletedAsync(createdReminderId);
        }

        var remindersForUser = await _maestroApiClient.GetRemindersForUserAsync(userId, inclusiveStartDate, exclusiveEndDate).ToListAsync();

        var filteredRemindersForUser = remindersForUser
            .Where(reminder => createdRemindersId.Contains(reminder.Id))
            .ToList();

        filteredRemindersForUser.Count.Should().Equals(3);

        foreach (var reminder in filteredRemindersForUser)
        {
            reminder.ReminderTime.Should().BeOnOrAfter(inclusiveStartDate).And.BeBefore(exclusiveEndDate);
            reminder.IsCompleted.Should().BeTrue();
        }
    }
}