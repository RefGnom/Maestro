using FluentAssertions;
using Maestro.Server.Public.Models.Schedules;
using Maestro.Tests.Extensions;

namespace Maestro.Tests.Client;

public partial class MaestroApiClientTests
{
    [Test]
    public async Task Schedules_should_throw_on_overlapping_create()
    {
        const long userId = 16;

        var schedule = new ScheduleDto
        {
            UserId = userId,
            Description = "Test1",
            Duration = TimeSpan.FromHours(1),
            CanOverlap = true,
            IsCompleted = false,
            StartDateTime = new DateTime(2025, 6, 7, 14, 0, 0),
        };

        await _maestroApiClient.CreateScheduleAsync(schedule);

        var overlapSchedule = new ScheduleDto
        {
            UserId = userId,
            Description = "Test2",
            Duration = TimeSpan.FromHours(1),
            CanOverlap = false,
            IsCompleted = false,
            StartDateTime = new DateTime(2025, 6, 7, 14, 30, 0),
        };

        (await _maestroApiClient.CreateScheduleAsync(overlapSchedule)).Should().BeNull();
    }

    [Test]
    public async Task Schedules_should_get_all_after_exclusive_start_date_time()
    {
        const long userId = 12;

        var schedulesCreateList = new List<ScheduleDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 1),
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 2),
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 3),
            }
        };

        await CreateSchedulesAsync(schedulesCreateList);

        var exclusiveStartDateTime = new DateTime(2024, 1, 1);
        var schedules = await _maestroApiClient.GetAllSchedulesAsync(exclusiveStartDateTime).ToListAsync();

        foreach (var schedule in schedules)
        {
            schedule.StartDateTime.Should().BeAfter(exclusiveStartDateTime);
        }
    }

    [Test]
    public async Task Schedules_should_get_for_user()
    {
        const long userId = 13;

        var schedulesCreateList = new List<ScheduleDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 1),
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 2),
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2024, 1, 3),
            }
        };

        var createdScheduleIds = await CreateSchedulesAsync(schedulesCreateList);
        var schedulesForUser = await _maestroApiClient.GetSchedulesForUserAsync(userId).ToListAsync();

        var filteredSchedulesForUser = schedulesForUser
            .Where(schedule => createdScheduleIds.Contains(schedule.ScheduleId))
            .ToList();

        schedulesCreateList.Should()
            .BeEquivalentTo(filteredSchedulesForUser, options => { return options.Excluding(reminderWithId => reminderWithId.ScheduleId); });
    }

    [Test]
    public async Task Schedules_should_get()
    {
        const long userId = 15;

        var schedule = new ScheduleDto
        {
            UserId = userId,
            Description = "Test1",
            Duration = TimeSpan.FromDays(1),
            CanOverlap = true,
            IsCompleted = false,
            StartDateTime = new DateTime(2024, 1, 1),
        };

        var createdScheduleId = await _maestroApiClient.CreateScheduleAsync(schedule);
        var createdSchedule = await _maestroApiClient.GetScheduleByIdAsync(createdScheduleId!.Value);

        createdSchedule.Should().BeEquivalentTo(schedule);
    }

    [Test]
    public async Task Schedules_should_get_for_user_after_exclusive_start_date_time()
    {
        const long userId = 14;

        var schedulesCreateList = new List<ScheduleDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 1),
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 2),
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 3),
            }
        };

        var createdScheduleIds = await CreateSchedulesAsync(schedulesCreateList);
        var exclusiveStartDateTime = new DateTime(2025, 1, 1);
        var schedulesForUser = await _maestroApiClient.GetSchedulesForUserAsync(userId, exclusiveStartDateTime).ToListAsync();

        var filteredSchedulesForUser = schedulesForUser
            .Where(schedule => createdScheduleIds.Contains(schedule.ScheduleId))
            .ToList();

        filteredSchedulesForUser.Should().HaveCount(2);

        foreach (var schedules in filteredSchedulesForUser)
        {
            schedules.StartDateTime.Should().BeAfter(exclusiveStartDateTime);
        }
    }

    [Test]
    public async Task Schedules_should_set_completed()
    {
        const long userId = 11;

        var schedulesCreateList = new List<ScheduleDto>
        {
            new()
            {
                UserId = userId,
                Description = "Test1",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 1),
            },
            new()
            {
                UserId = userId,
                Description = "Test2",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 2),
            },
            new()
            {
                UserId = userId,
                Description = "Test3",
                Duration = TimeSpan.FromDays(1),
                CanOverlap = true,
                IsCompleted = false,
                StartDateTime = new DateTime(2025, 1, 3),
            }
        };

        var createdScheduleIds = await CreateSchedulesAsync(schedulesCreateList);

        foreach (var createdScheduleId in createdScheduleIds)
        {
            await _maestroApiClient.SetScheduleCompletedAsync(createdScheduleId);
        }

        foreach (var createdScheduleId in createdScheduleIds)
        {
            Assert.Throws<AggregateException>(() => { _maestroApiClient.SetScheduleCompletedAsync(createdScheduleId).Wait(); });
        }
    }

    private async Task<List<long>> CreateSchedulesAsync(List<ScheduleDto> schedulesCreateList)
    {
        var createdScheduleIds = new List<long>();

        foreach (var schedule in schedulesCreateList)
        {
            var createdScheduleId = await _maestroApiClient.CreateScheduleAsync(schedule);
            createdScheduleIds.Add(createdScheduleId!.Value);
        }

        return createdScheduleIds;
    }
}