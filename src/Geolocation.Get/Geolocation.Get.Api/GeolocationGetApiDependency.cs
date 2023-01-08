using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Infra.Azure.Services.AzureUser.Get.Tests")]

namespace GGroupp.Infra;

public static class GeolocationGetApiDependency
{
    private const string DefaultSectionName = "Azure.Geolocation";

    public static Dependency<IGeolocationGetFunc> UseGeolocationGetFunc(
        this Dependency<HttpMessageHandler, GeolocationApiOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<IGeolocationGetFunc>(CreateFunc);
    }

    public static Dependency<IGeolocationGetFunc> UseGeolocationGetFunc(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, GeolocationApiOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<IGeolocationGetFunc>(CreateFunc);
    }

    public static Dependency<IGeolocationGetFunc> UseGeolocationGetFunc(
        this Dependency<HttpMessageHandler> dependency, [AllowNull] string sectionName = DefaultSectionName)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveOption).Fold<IGeolocationGetFunc>(CreateFunc);

        GeolocationApiOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().GetSection(sectionName.OrEmpty()).GetApiOption();
    }

    private static GeolocationGetFunc CreateFunc(HttpMessageHandler httpMessageHandler, GeolocationApiOption option)
    {
        ArgumentNullException.ThrowIfNull(httpMessageHandler);
        ArgumentNullException.ThrowIfNull(option);

        return GeolocationGetFunc.InternalCreate(httpMessageHandler, AuthTokenGetFunc.Instance, option);
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