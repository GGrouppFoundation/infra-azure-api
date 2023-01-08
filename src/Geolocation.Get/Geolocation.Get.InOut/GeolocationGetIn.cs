using System.Net;

namespace GGroupp.Infra;

public sealed record class GeolocationGetIn
{
    public GeolocationGetIn(IPAddress ipAddress)
        =>
        IpAddress = ipAddress;

    public IPAddress IpAddress { get; }
}