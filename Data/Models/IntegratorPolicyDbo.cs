// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Maestro.Data.Models;

public class IntegratorPolicyDbo
{
    public long Id { get; set; }

    public long IntegratorId { get; set; }

    public string Policy { get; set; }
}