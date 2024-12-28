using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Models;
using NSubstitute;

namespace Maestro.Tests.Client;

[TestFixture]
public class EventsApiClientTests
{
    private EventsApiClient _eventsApiClient;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string uri = "http://localhost:5000/api/v1/";
        const string apiKey = "00000000000000000000000000000000";
        
        _eventsApiClient = new EventsApiClient(uri, apiKey, Substitute.For<ILogFactory>());
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _eventsApiClient.Dispose();
    }
    
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

        await _eventsApiClient.CreateAsync(reminderDto);
    }
}