using System.Text.Json.Serialization;

namespace GGroupp.Infra.Azure.Services.AzureUser.Get.Tests;

internal sealed record class StubRegionJson
{
    [JsonPropertyName("isoCode")]
    public string? IsoCode { get; init; }
}