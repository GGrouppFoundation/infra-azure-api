using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class AzureUserMeGetFunc : IAzureUserMeGetFunc
{
    public static AzureUserMeGetFunc Create(HttpMessageHandler httpMessageHandler)
        =>
        new(
            httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)));

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly Uri graphApiBaseAddress = new("https://graph.microsoft.com/");

    private AzureUserMeGetFunc(HttpMessageHandler httpMessageHandler)
        =>
        this.httpMessageHandler = httpMessageHandler;
}