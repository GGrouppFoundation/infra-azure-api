using System;

namespace GGroupp.Infra;

public sealed record class GeolocationApiOption
{
    public GeolocationApiOption(Guid clientId, GeolocationApiCredential credential)
    {
        ClientId = clientId;
        Credential = credential;
    }

    public Guid ClientId { get; }

    public GeolocationApiCredential Credential { get; }
}