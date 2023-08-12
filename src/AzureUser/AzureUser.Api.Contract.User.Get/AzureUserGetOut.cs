using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

public sealed record class AzureUserGetOut
{
    public AzureUserGetOut(Guid id, [AllowNull] string mail, [AllowNull] string displayName)
    {
        Id = id;
        Mail = mail ?? string.Empty;
        DisplayName = displayName ?? string.Empty;
    }

    public Guid Id { get; }

    public string Mail { get; }

    public string DisplayName { get; }
}