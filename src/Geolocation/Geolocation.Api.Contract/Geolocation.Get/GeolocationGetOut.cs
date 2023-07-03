using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

public readonly record struct GeolocationGetOut
{
    public GeolocationGetOut([AllowNull] string countryIsoCode)
        =>
        CountryIsoCode = string.IsNullOrEmpty(countryIsoCode) ? null : countryIsoCode;

    /// <value>
    /// The 2-character code (ISO 3166-1) of the country or region.
    /// IP address in ranges reserved for special purpose will return Null.
    /// </value>
    public string? CountryIsoCode { get; }
}