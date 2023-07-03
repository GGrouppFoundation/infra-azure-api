using System;
using System.Net.Http;

namespace GarageGroup.Infra;

internal sealed partial class AzureUserApi : IAzureUserApi
{
    private static readonly Uri GraphApiBaseAddress;

    static AzureUserApi()
        =>
        GraphApiBaseAddress = new("https://graph.microsoft.com/");

    private readonly HttpMessageHandler httpMessageHandler;

    internal AzureUserApi(HttpMessageHandler httpMessageHandler)
        =>
        this.httpMessageHandler = httpMessageHandler;
}