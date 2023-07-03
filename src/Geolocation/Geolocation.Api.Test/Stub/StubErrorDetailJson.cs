using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class StubErrorDetailJson
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("details")]
    public StubErrorDetailJson[]? Details { get; init; }
}