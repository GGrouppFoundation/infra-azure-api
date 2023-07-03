using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Azure.Services.Geolocation.Api.Test;

internal sealed record class StubSuccessJson
{
    [JsonPropertyName("countryRegion")]
    public StubRegionJson? CountryRegion { get; init; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }
}