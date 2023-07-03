using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Azure.Services.Geolocation.Api.Test")]

namespace GarageGroup.Infra;

public static class GeolocationApiDependency
{
    private const string DefaultSectionName = "Azure.Geolocation";

    public static Dependency<IGeolocationApi> UseGeolocationApi(
        this Dependency<HttpMessageHandler, GeolocationApiOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Fold<IGeolocationApi>(CreateApi);
    }

    public static Dependency<IGeolocationApi> UseGeolocationApi(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, GeolocationApiOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<IGeolocationApi>(CreateApi);
    }

    public static Dependency<IGeolocationApi> UseGeolocationApi(
        this Dependency<HttpMessageHandler> dependency, [AllowNull] string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.With(ResolveOption).Fold<IGeolocationApi>(CreateApi);

        GeolocationApiOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(sectionName.OrEmpty()).GetApiOption();
    }

    private static GeolocationApi CreateApi(HttpMessageHandler httpMessageHandler, GeolocationApiOption option)
    {
        ArgumentNullException.ThrowIfNull(httpMessageHandler);
        ArgumentNullException.ThrowIfNull(option);

        return new(httpMessageHandler, AuthTokenGetFunc.Instance, option);
    }

    private static GeolocationApiOption GetApiOption(this IConfigurationSection section)
        =>
        new(
            clientId: section.GetGuid("ClientId"),
            credential: section.GetRequiredSection("Credential").GetApiCredential());

    private static GeolocationApiCredential GetApiCredential(this IConfigurationSection section)
        =>
        new(
            tenantId: section.GetGuid("TenantId"),
            clientId: section["ClientId"].OrEmpty(),
            clientSecret: section["ClientSecret"].OrEmpty());

    private static Guid GetGuid(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return Guid.Parse(value);
    }
}
