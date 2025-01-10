using FluentAssertions;
using Maestro.Client.Integrator;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

[TestFixture]
public class MaestroApiClientTests
{
    private MaestroApiClient _maestroApiClient;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string uri = "http://localhost:5000/api/v1/";
        const string apiKey = "testIntegrator";

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

    [Test]
    public async Task Reminders_should_get_all_after_exclusive_start_date_time()
    {
        const long userId = 5;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2024, 1, 1),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2024, 4, 3),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2024, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            }
        };

        await CreateRemindersAsync(remindersCreateList);

        var exclusiveStartDateTime = new DateTime(2024, 1, 1);
        var reminders = await _maestroApiClient.GetAllRemindersAsync(exclusiveStartDateTime).ToListAsync();

        foreach (var reminder in reminders)
        {
            reminder.RemindDateTime.Should().BeAfter(exclusiveStartDateTime);
        }
    }

    [Test]
    public async Task Reminders_should_get_reminders_for_user()
    {
        const long userId = 1;
        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2024, 1, 1),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2024, 4, 3),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2024, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            }
        };

        var createdReminderIds = await CreateRemindersAsync(remindersCreateList);
        var remindersForUser = await _maestroApiClient.GetRemindersForUserAsync(userId).ToListAsync();

        var filteredRemindersForUser = remindersForUser
            .Where(reminder => createdReminderIds.Contains(reminder.ReminderId))
            .ToList();

        remindersCreateList.Should()
            .BeEquivalentTo(filteredRemindersForUser, options => { return options.Excluding(reminderWithId => reminderWithId.ReminderId); });
    }

    [Test]
    public async Task Reminders_should_get_reminder()
    {
        const long userId = 2;

        var reminder = new ReminderDto
        {
            UserId = userId,
            Description = "Test",
            RemindDateTime = new DateTime(2024, 1, 2),
            RemindInterval = TimeSpan.FromMinutes(1),
            RemindCount = 3
        };

        var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminder);
        var createdReminder = await _maestroApiClient.GetReminderAsync(createdReminderId);

        createdReminder.Should().BeEquivalentTo(reminder);
    }

    [Test]
    public async Task Reminders_should_get_reminders_after_exclusive_start_date_time()
    {
        const long userId = 3;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2025, 1, 1),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2025, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2025, 1, 3),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            }
        };

        var createdReminderIds = await CreateRemindersAsync(remindersCreateList);
        var exclusiveStartDateTime = new DateTime(2025, 1, 1);
        var remindersForUser = await _maestroApiClient.GetRemindersForUserAsync(userId, exclusiveStartDateTime).ToListAsync();

        var filteredRemindersForUser = remindersForUser
            .Where(reminder => createdReminderIds.Contains(reminder.ReminderId))
            .ToList();

        filteredRemindersForUser.Should().HaveCount(2);

        foreach (var reminder in filteredRemindersForUser)
        {
            reminder.RemindDateTime.Should().BeAfter(exclusiveStartDateTime);
        }
    }

    [Test]
    public async Task Reminders_should_set_completed()
    {
        const long userId = 4;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2025, 1, 1),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2025, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2025, 1, 3),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            }
        };

        var createdReminderIds = await CreateRemindersAsync(remindersCreateList);

        foreach (var createdReminderId in createdReminderIds)
        {
            await _maestroApiClient.SetReminderCompletedAsync(createdReminderId);
        }

        foreach (var createdReminderId in createdReminderIds)
        {
            Assert.Throws<AggregateException>(() => { _maestroApiClient.SetReminderCompletedAsync(createdReminderId).Wait(); });
        }
    }

    [Test]
    public async Task Reminders_should_decrement_remind_count()
    {
        const long userId = 6;
        const int remindCount = 5;

        var reminderToCreate = new ReminderDto
        {
            UserId = userId,
            Description = "Test1",
            RemindDateTime = new DateTime(2025, 1, 1),
            RemindInterval = TimeSpan.FromMinutes(1),
            RemindCount = remindCount
        };

        var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminderToCreate);

        for (int i = 1; i <= remindCount; i++)
        {
            var remainRemindCount = await _maestroApiClient.DecrementRemindCountAsync(createdReminderId);
            var reminder = await _maestroApiClient.GetReminderAsync(createdReminderId);
            var expectedRemainRemindCount = remindCount - i;
            reminder!.RemindCount.Should().Be(expectedRemainRemindCount);
            remainRemindCount.Should().Be(expectedRemainRemindCount);
        }
    }

    [Test]
    public async Task Reminders_should_set_new_remind_date_time()
    {
        const long userId = 7;

        var reminderToCreate = new ReminderDto
        {
            UserId = userId,
            Description = "Test1",
            RemindDateTime = new DateTime(2024, 2, 1, 16, 55, 0),
            RemindInterval = TimeSpan.FromMinutes(1),
            RemindCount = 3
        };

        var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminderToCreate);
        var newRemindDateTime = new DateTime(2025, 1, 2, 17, 0, 0);

        await _maestroApiClient.SetReminderDateTimeAsync(createdReminderId, newRemindDateTime);

        var reminder = await _maestroApiClient.GetReminderAsync(createdReminderId);
        reminder!.RemindDateTime.Should().Be(newRemindDateTime);
    }

    private async Task<List<long>> CreateRemindersAsync(List<ReminderDto> remindersCreateList)
    {
        var createdReminderIds = new List<long>();

        foreach (var reminder in remindersCreateList)
        {
            var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminder);
            createdReminderIds.Add(createdReminderId);
        }

        return createdReminderIds;
    }
}