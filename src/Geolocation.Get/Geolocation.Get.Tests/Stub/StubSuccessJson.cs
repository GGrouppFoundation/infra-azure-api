using System.Text.Json.Serialization;

namespace GGroupp.Infra.Azure.Services.AzureUser.Get.Tests;

internal sealed record class StubSuccessJson
{
    [JsonPropertyName("countryRegion")]
    public StubRegionJson? CountryRegion { get; init; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }
}