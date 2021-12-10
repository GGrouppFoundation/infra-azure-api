using System;

namespace GGroupp.Platform;

public sealed record class AzureUserApiConfiguration
{
    public AzureUserApiConfiguration(Uri graphApiBaseAddress)
        =>
        GraphApiBaseAddress = graphApiBaseAddress;

    public Uri GraphApiBaseAddress { get; }
}