using FluentAssertions;
using Maestro.Client.Daemon;
using Maestro.Client.Integrator;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

[TestFixture]
public class DaemonMaestroApiClientTests
{
    private DaemonMaestroApiClient _daemonMaestroApiClient;
    private MaestroApiClient _maestroApiClient;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string daemonUri = "http://localhost:5000/daemon/";
        const string daemonApiKey = "daemon123";

        _daemonMaestroApiClient = new DaemonMaestroApiClient(daemonUri, daemonApiKey,
            // Substitute.For<ILogFactory>()
            new LogFactory(new DateTimeProvider(), new Writer())
        );

        const string integratorUri = "http://localhost:5000/api/v1/";
        const string integratorApiKey = "testIntegrator";

        _maestroApiClient = new MaestroApiClient(integratorUri, integratorApiKey,
            // Substitute.For<ILogFactory>()
            new LogFactory(new DateTimeProvider(), new Writer())
        );
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _daemonMaestroApiClient.Dispose();
        _maestroApiClient.Dispose();
    }

    [Test]
    public async Task Reminders_should_get_all_completed()
    {
        const long userId = 8;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2024, 1, 1),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3,
                IsCompleted = false,
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2024, 4, 3),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3,
                IsCompleted = true,
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2024, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3,
                IsCompleted = false,
            },
            new()
            {
                UserId = userId,
                Description = "Test4",
                RemindDateTime = new DateTime(2024, 1, 2),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3,
                IsCompleted = true,
            },
        };

        await CreateRemindersAsync(remindersCreateList);

        var reminders = await _daemonMaestroApiClient.GetCompletedRemindersAsync().ToListAsync();

        reminders.All(reminder => reminder.IsCompleted).Should().BeTrue();
    }

    [Test]
    public async Task Reminders_should_get_reminders_for_user()
    {
        const long userId = 9;

        var remindersCreateList = new List<ReminderDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                RemindDateTime = new DateTime(2025, 1, 5),
                RemindInterval = TimeSpan.FromMinutes(1),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                RemindDateTime = new DateTime(2025, 1, 6),
                RemindInterval = TimeSpan.FromMinutes(2),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2025, 1, 7),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                RemindDateTime = new DateTime(2025, 1, 8),
                RemindInterval = TimeSpan.FromMinutes(3),
                RemindCount = 3
            }
        };

        await CreateRemindersAsync(remindersCreateList);

        var inclusiveBeforeDateTime = new DateTime(2025, 1, 7);
        var reminders = await _daemonMaestroApiClient.GetOldRemindersAsync(inclusiveBeforeDateTime).ToListAsync();

        reminders.ForEach(reminder => { reminder.RemindDateTime.Should().BeOnOrBefore(inclusiveBeforeDateTime); });
    }

    [Test]
    public async Task Reminders_should_delete_reminder_by_id()
    {
        const long userId = 10;

        var reminderToCreate = new ReminderDto
        {
            UserId = userId,
            Description = "Test1",
            RemindDateTime = new DateTime(2025, 1, 5),
            RemindInterval = TimeSpan.FromMinutes(1),
            RemindCount = 3
        };

        var createdReminderId = await _maestroApiClient.CreateReminderAsync(reminderToCreate);
        await _daemonMaestroApiClient.DeleteReminderAsync(createdReminderId);

        (await _maestroApiClient.GetReminderAsync(createdReminderId)).Should().BeNull();

        Assert.Throws<AggregateException>(() => { _daemonMaestroApiClient.DeleteReminderAsync(createdReminderId).Wait(); });
    }

    private async Task CreateRemindersAsync(List<ReminderDto> remindersCreateList)
    {
        foreach (var reminder in remindersCreateList)
        {
            await _maestroApiClient.CreateReminderAsync(reminder);
        }
    }
}