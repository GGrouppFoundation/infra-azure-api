using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Moq;

namespace GarageGroup.Infra.Azure.Services.Geolocation.Api.Test;

public static partial class GeolocationApiTest
{
    static GeolocationApiTest()
    {
        SomeInput = new(
            ipAddress: IPAddress.Parse("193.108.118.36"));

        SomeApiOption = new(
            clientId: Guid.Parse("b2280c4f-d4c7-42c7-8a0b-022a784db721"),
            credential: new(
                tenantId: Guid.Parse("01aea89b-1a2b-4a05-9db6-1493fdc812de"),
                clientId: "Some client ID",
                clientSecret: "Some client secret"));

        SomeAuthenticationHeaderValue = new("Bearer", "Some token");
    }

    private static readonly GeolocationGetIn SomeInput;

    private static readonly GeolocationApiOption SomeApiOption;

    private static readonly AuthenticationHeaderValue SomeAuthenticationHeaderValue;

    private static Mock<IAsyncFunc<GeolocationApiCredential, AuthenticationHeaderValue>> CreateMockAuthTokenGetFunc(
        AuthenticationHeaderValue authenticationHeaderValue)
    {
        var mock = new Mock<IAsyncFunc<GeolocationApiCredential, AuthenticationHeaderValue>>();

        _ = mock
            .Setup(p => p.InvokeAsync(It.IsAny<GeolocationApiCredential>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authenticationHeaderValue);

        return mock;
    }

    private static Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>> CreateMockProxyHandler(
        HttpResponseMessage responseMessage, Action<HttpRequestMessage>? callback = default)
    {
        var mock = new Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>>();

        var m = mock
            .Setup(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMessage);

        if (callback is not null)
        {
            _ = m.Callback<HttpRequestMessage, CancellationToken>((r, _) => callback.Invoke(r));
        }

        return mock;
    }

    private static KeyValuePair<string, string>[] GetHeaderValues(this HttpRequestMessage requestMessage)
    {
        return requestMessage.Headers.Select(MapValue).ToArray();

        static KeyValuePair<string, string> MapValue(KeyValuePair<string, IEnumerable<string>> source)
            =>
            new(
                source.Key, string.Join(',', source.Value));
    }

    private static HttpResponseMessage CreateResponse(this HttpContent? httpContent, HttpStatusCode statusCode)
        =>
        new(statusCode)
        {
            Content = httpContent
        };
}