using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Azure.Services.Geolocation.Api.Test;

internal sealed record class StubRegionJson
{
    [JsonPropertyName("isoCode")]
    public string? IsoCode { get; init; }
}