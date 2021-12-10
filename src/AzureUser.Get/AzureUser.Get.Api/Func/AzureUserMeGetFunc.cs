using System;
using System.Net.Http;

namespace GGroupp.Platform;

using IAzureUserApiConfigurationProvider = IFunc<AzureUserApiConfiguration>;
using IAzureUserMeGetFunc = IAsyncValueFunc<AzureUserMeGetIn, Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>>;

internal sealed partial class AzureUserMeGetFunc : IAzureUserMeGetFunc
{
    public static AzureUserMeGetFunc Create(HttpMessageHandler httpMessageHandler, IAzureUserApiConfigurationProvider configurationProvider)
        =>
        new(
            httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)),
            configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider)));

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly IAzureUserApiConfigurationProvider configurationProvider;

    private AzureUserMeGetFunc(HttpMessageHandler httpMessageHandler, IAzureUserApiConfigurationProvider configurationProvider)
    {
        this.httpMessageHandler = httpMessageHandler;
        this.configurationProvider = configurationProvider;
    }
}