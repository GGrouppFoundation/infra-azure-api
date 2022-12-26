using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class AzureUserGetFuncDependency
{
    public static Dependency<IAzureUserMeGetFunc> UseAzureUserMeGetApi(this Dependency<HttpMessageHandler> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map<IAzureUserMeGetFunc>(AzureUserMeGetFunc.Create);
    }
}