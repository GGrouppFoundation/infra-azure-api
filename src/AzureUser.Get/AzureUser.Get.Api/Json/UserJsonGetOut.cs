using System;
using System.Text.Json.Serialization;

namespace GGroupp.Platform;

internal readonly record struct UserJsonGetOut
{
    [JsonPropertyName(ApiNames.IdFieldName)]
    public Guid Id { get; init; }

    [JsonPropertyName(ApiNames.MailFieldName)]
    public string? Mail { get; init; }

    [JsonPropertyName(ApiNames.DisplayNameFieldName)]
    public string? DisplayName { get; init; }
}