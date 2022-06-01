using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp.Platform;

using IAzureUserMeGetFunc = IAsyncValueFunc<AzureUserMeGetIn, Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>>;

public static class AzureUserGetFuncDependency
{
    public static Dependency<IAzureUserMeGetFunc> UseAzureUserMeGetApi<THttpMessageHandler>(
        this Dependency<THttpMessageHandler> dependency,
        Func<IServiceProvider, AzureUserApiConfiguration> configurationResolver)
        where THttpMessageHandler : HttpMessageHandler
        =>
        InnerUseAzureUserMeGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

    public static Dependency<IAzureUserMeGetFunc> UseAzureUserMeGetApi(
        this Dependency<HttpMessageHandler> dependency)
        =>
        InnerUseAzureUserMeGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)));

    private static Dependency<IAzureUserMeGetFunc> InnerUseAzureUserMeGetApi<THttpMessageHandler>(
        Dependency<THttpMessageHandler> dependency,
        Func<IServiceProvider, AzureUserApiConfiguration>? configurationResolver = null)
        where THttpMessageHandler : HttpMessageHandler
        =>
        dependency.Map<IAzureUserMeGetFunc>(
            (sp, handler) => AzureUserMeGetFunc.Create(handler, configurationResolver?.Invoke(sp)));
}