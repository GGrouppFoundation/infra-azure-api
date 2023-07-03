using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GarageGroup.Infra;

using IAuthTokenGetFunc = IAsyncFunc<GeolocationApiCredential, AuthenticationHeaderValue>;

internal sealed partial class GeolocationApi : IGeolocationApi
{
    private const string ApiVersion = "1.0";

    private static readonly Uri GeolocationBaseAddress;

    private static readonly JsonSerializerOptions SerializerOptions;

    static GeolocationApi()
    {
        GeolocationBaseAddress = new("https://atlas.microsoft.com/");
        SerializerOptions = new(JsonSerializerDefaults.Web);
    }

    private readonly HttpMessageHandler httpMessageHandler;

    private readonly IAuthTokenGetFunc authTokenGetFunc;

    private readonly GeolocationApiOption option;

    internal GeolocationApi(HttpMessageHandler httpMessageHandler, IAuthTokenGetFunc authTokenGetFunc, GeolocationApiOption option)
    {
        this.httpMessageHandler = httpMessageHandler;
        this.authTokenGetFunc = authTokenGetFunc;
        this.option = option;
    }

    private HttpClient CreateHttpClient(AuthenticationHeaderValue authenticationHeaderValue)
    {
        var httpClient = new HttpClient(httpMessageHandler, disposeHandler: false)
        {
            BaseAddress = GeolocationBaseAddress
        };

        httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        httpClient.DefaultRequestHeaders.Add("x-ms-client-id", option.ClientId.ToString());

        return httpClient;
    }

    private static HttpRequestMessage CreateHttpRequestMessage(IPAddress ip)
        =>
        new(HttpMethod.Get, $"geolocation/ip/json?api-version={ApiVersion}&ip={ip}");

    private static T Deserialize<T>(string json)
        where T : struct
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, SerializerOptions);
    }
}