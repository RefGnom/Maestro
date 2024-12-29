using FluentAssertions;
using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Server.Core.ApiModels;
using Maestro.Tests.Extensions;
using NSubstitute;

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

        _apiClient = new ApiClient(uri, apiKey, Substitute.For<ILogFactory>());
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
                Description = "Test1",
                IsCompleted = true,
                IsRepeatable = true,
                ReminderTime = new DateTime(2024, 1, 1),
                ReminderTimeDuration = TimeSpan.FromMinutes(1),
                UserId = userId
            },
            new()
            {
                Description = "Test2",
                IsCompleted = true,
                IsRepeatable = false,
                ReminderTime = new DateTime(2024, 4, 3),
                ReminderTimeDuration = TimeSpan.FromMinutes(2),
                UserId = userId
            },
            new()
            {
                Description = "Test3",
                IsCompleted = false,
                IsRepeatable = false,
                ReminderTime = new DateTime(2024, 1, 2),
                ReminderTimeDuration = TimeSpan.FromMinutes(3),
                UserId = userId
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
    public async Task Reminders_Create()
    {
        var reminderDto = new ReminderDto
        {
            Description = "Test",
            IsCompleted = true,
            IsRepeatable = true,
            ReminderTime = DateTime.Now,
            ReminderTimeDuration = TimeSpan.FromMinutes(1),
            UserId = 1
        };

        var createdReminderId = await _apiClient.CreateReminderAsync(reminderDto);

        createdReminderId.Should().NotBe(0);
    }

    #endregion
}