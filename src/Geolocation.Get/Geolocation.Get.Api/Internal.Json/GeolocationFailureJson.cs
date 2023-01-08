namespace GGroupp.Infra;

internal readonly record struct GeolocationFailureJson
{
    public ErrorDetailJson Error { get; init; }
}