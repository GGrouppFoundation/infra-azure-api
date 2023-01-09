using System;

namespace GGroupp.Infra;

public sealed record class GeolocationApiCredential
{
    public GeolocationApiCredential(Guid tenantId, string clientId, string clientSecret)
    {
        TenantId = tenantId;
        ClientId = clientId.OrEmpty();
        ClientSecret = clientSecret.OrEmpty();
    }

    public Guid TenantId { get; }

    public string ClientId { get; }

    public string ClientSecret { get; }
}