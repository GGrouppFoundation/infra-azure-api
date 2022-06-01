using System;
using System.Net.Http;

namespace GGroupp.Platform;

using IAzureUserMeGetFunc = IAsyncValueFunc<AzureUserMeGetIn, Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>>;

internal sealed partial class AzureUserMeGetFunc : IAzureUserMeGetFunc
{
    public static AzureUserMeGetFunc Create(HttpMessageHandler httpMessageHandler, AzureUserApiConfiguration? configuration = null)
        =>
        new(
            httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)), configuration);

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly Uri graphApiBaseAddress;

    private AzureUserMeGetFunc(HttpMessageHandler httpMessageHandler, AzureUserApiConfiguration? configuration = null)
    {
        this.httpMessageHandler = httpMessageHandler;
        graphApiBaseAddress = configuration?.GraphApiBaseAddress ?? new("https://graph.microsoft.com/");
    }
}