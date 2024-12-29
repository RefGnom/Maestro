using FluentAssertions;
using Maestro.Client;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.ApiModels;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

[TestFixture]
public class ApiClientTests
{
    private ApiClient _apiClient;

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

    #region Get

    [Test]
    public async Task Reminders_ForUser()
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
                IsRepeatable = true,
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                ReminderTime = new DateTime(2024, 4, 3),
                ReminderTimeDuration = TimeSpan.FromMinutes(2),
                IsCompleted = true,
                IsRepeatable = false,
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                ReminderTime = new DateTime(2024, 1, 2),
                ReminderTimeDuration = TimeSpan.FromMinutes(3),
                IsCompleted = false,
                IsRepeatable = false,
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

    #endregion

    #region Post

    [Test]
    [Order(1)]
    public async Task Reminders_Create()
    {
        var reminder = new ReminderDto
        {
            UserId = 2,
            Description = "Test",
            ReminderTime = new DateTime(2024, 1, 2),
            ReminderTimeDuration = TimeSpan.FromMinutes(1),
            IsCompleted = true,
            IsRepeatable = false,
        };

        var createdReminderId = await _apiClient.CreateReminderAsync(reminder);

        createdReminderId.Should().NotBe(0);
    }

    [Test]
    [Order(2)]
    public async Task Reminders_Get()
    {
        
    }

    #endregion
}