using System.ComponentModel.DataAnnotations;
using Maestro.Server.Public.Attributes;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Maestro.Server.Public.Models;

public class ReminderIdsDto
{
    public const int LimitMaxValue = 50;

    [MaxLength(LimitMaxValue)]
    [UniqueValues]
    public IList<long> ReminderIds { get; set; }
}