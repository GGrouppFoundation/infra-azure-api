using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Platform;

public readonly record struct AzureUserGetOut
{
    private readonly string? mail, displayName;

    public AzureUserGetOut(
        Guid id,
        [AllowNull] string mail,
        [AllowNull] string displayName)
    {
        Id = id;
        this.mail = string.IsNullOrEmpty(mail) ? default : mail;
        this.displayName = string.IsNullOrEmpty(displayName) ? default : displayName;
    }

    public Guid Id { get; }

    public string Mail => mail ?? string.Empty;

    public string DisplayName => displayName ?? string.Empty;
}