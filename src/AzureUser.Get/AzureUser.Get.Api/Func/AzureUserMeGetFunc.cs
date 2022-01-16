using System;
using System.Net.Http;

namespace GGroupp.Platform;

using IAzureUserMeGetFunc = IAsyncValueFunc<AzureUserMeGetIn, Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>>;

internal sealed partial class AzureUserMeGetFunc : IAzureUserMeGetFunc
{
    public static AzureUserMeGetFunc Create(HttpMessageHandler httpMessageHandler, AzureUserApiConfiguration configuration)
        =>
        new(
            httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)),
            configuration ?? throw new ArgumentNullException(nameof(configuration)));

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly AzureUserApiConfiguration configuration;

    private AzureUserMeGetFunc(HttpMessageHandler httpMessageHandler, AzureUserApiConfiguration configuration)
    {
        this.httpMessageHandler = httpMessageHandler;
        this.configuration = configuration;
    }
}