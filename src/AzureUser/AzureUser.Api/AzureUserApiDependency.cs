using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class AzureUserApiDependency
{
    public static Dependency<IAzureUserApi> UseAzureUserApi(this Dependency<HttpMessageHandler> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map<IAzureUserApi>(CreateApi);

        static AzureUserApi CreateApi(HttpMessageHandler httpMessageHandler)
        {
            ArgumentNullException.ThrowIfNull(httpMessageHandler);

            return new(httpMessageHandler);
        }
    }
}