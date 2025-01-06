﻿using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Maestro.Server.Core.Models;

public class RemindersIdDto
{
    public const int LimitMaxValue = 50;

    [MaxLength(LimitMaxValue)] public List<long> RemindersId { get; set; }
}