using System.ComponentModel.DataAnnotations;
using Maestro.Server.Core.Attributes;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Maestro.Server.Core.Models;

public class RemindersIdsDto
{
    public const int LimitMaxValue = 50;

    [MaxLength(LimitMaxValue)]
    [UniqueValues]
    public IList<long> RemindersIds { get; set; }
}