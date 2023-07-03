using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct StubFailureJson
{
    [JsonPropertyName("error")]
    public StubErrorDetailJson? Error { get; init; }
}