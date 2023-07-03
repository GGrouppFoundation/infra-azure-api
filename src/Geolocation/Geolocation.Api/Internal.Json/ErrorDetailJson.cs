namespace GarageGroup.Infra;

internal readonly record struct ErrorDetailJson
{
    public string? Message { get; init; }

    public ErrorDetailJson[]? Details { get; init; }
}