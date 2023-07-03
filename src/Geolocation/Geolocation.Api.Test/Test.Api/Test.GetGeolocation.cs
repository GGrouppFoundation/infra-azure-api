using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Azure.Services.Geolocation.Api.Test;

using static HttpResponseTestSource;

partial class GeolocationApiTest
{
    [Fact]
    public static void GetGeolocationAsync_InputIsNull_ExpectArgumentNullException()
    {
        using var response = CreateSomeSuccessContent().CreateResponse(HttpStatusCode.OK);
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, SomeApiOption);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("input", ex.ParamName);

        void Test()
            =>
            _ = api.GetGeolocationAsync(null!, cancellationToken).AsTask();
    }

    [Fact]
    public static void GetGeolocationAsync_CancellationTokenIsCacneled_ExpectValueTaskIsCanceled()
    {
        using var response = CreateSomeSuccessContent().CreateResponse(HttpStatusCode.OK);
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, SomeApiOption);

        var cancellationToken = new CancellationToken(canceled: true);
        var actual = api.GetGeolocationAsync(SomeInput, cancellationToken);

        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task GetGeolocationAsync_CancellationTokenIsNotCacneled_ExpectAuthTokenGetFuncCalledOnce()
    {
        using var response = CreateSomeSuccessContent().CreateResponse(HttpStatusCode.OK);
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var apiCredential = new GeolocationApiCredential(
            tenantId: Guid.Parse("0741bca5-393d-46d7-adf7-d54178628c5f"),
            clientId: "Some client id",
            clientSecret: "Some secret");

        var apiOption = new GeolocationApiOption(Guid.Parse("2aaf10d0-c19f-4b1e-805b-aed6509470a1"), apiCredential);
        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, apiOption);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.GetGeolocationAsync(SomeInput, cancellationToken);
        
        mockAuthTokenGetFunc.Verify(f => f.InvokeAsync(apiCredential, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task GetGeolocationAsync_CancellationTokenIsNotCacneled_ExpectRequestIsAuthorized()
    {
        using var response = CreateSomeSuccessContent().CreateResponse(HttpStatusCode.OK);
        var mockProxyHandler = CreateMockProxyHandler(response, OnHttpSend);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);

        var authHeaderValue = new AuthenticationHeaderValue("Bearer", "AbCdEf123456");
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(authHeaderValue);

        var apiCredential = new GeolocationApiCredential(
            tenantId: Guid.Parse("6c1ad3db-3a65-4942-a888-a4b66f1c72a5"),
            clientId: "SomeId",
            clientSecret: "SomeSecret");

        var apiOption = new GeolocationApiOption(Guid.Parse("2a655714-b4a0-4d1d-8971-07c975562923"), apiCredential);
        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, apiOption);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.GetGeolocationAsync(SomeInput, cancellationToken);
        
        mockProxyHandler.Verify(f => f.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        static void OnHttpSend(HttpRequestMessage actualRequest)
        {
            var actualHeaderValues = actualRequest.GetHeaderValues().OrderBy(GetKey);

            var expectedHeaderValues = new KeyValuePair<string, string>[]
            {
                new("Authorization", "Bearer AbCdEf123456"),
                new("x-ms-client-id", "2a655714-b4a0-4d1d-8971-07c975562923")
            };

            Assert.Equal(expectedHeaderValues, actualHeaderValues);
        }

        static string GetKey(KeyValuePair<string, string> pair)
            =>
            pair.Key;
    }

    [Theory]
    [InlineData("149.202.82.172")]
    [InlineData("2001:db8::1234:5678:506:708")]
    public static async Task GetGeolocationAsync_CancellationTokenIsNotCacneled_ExpectCorrectRequestUrl(string sourceAddress)
    {
        using var response = CreateSomeSuccessContent().CreateResponse(HttpStatusCode.OK);
        var mockProxyHandler = CreateMockProxyHandler(response, OnHttpSend);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, SomeApiOption);

        var input = new GeolocationGetIn(IPAddress.Parse(sourceAddress));
        var cancellationToken = new CancellationToken(canceled: false);

        _ = await api.GetGeolocationAsync(input, cancellationToken);
        mockProxyHandler.Verify(f => f.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void OnHttpSend(HttpRequestMessage actualRequest)
        {
            Assert.Equal(HttpMethod.Get, actualRequest.Method);

            var expectedRequestUrl = "https://atlas.microsoft.com/geolocation/ip/json?api-version=1.0&ip=" + sourceAddress;
            Assert.Equal(expectedRequestUrl, actualRequest.RequestUri?.ToString());
        }
    }

    [Theory]
    [MemberData(nameof(GetSuccessResponseTestData), MemberType = typeof(HttpResponseTestSource))]
    public static async Task GetGeolocationAsync_HttpResponseIsSuccess_ExpectSuccess(
        HttpStatusCode statusCode, HttpContent content, GeolocationGetOut expected)
    {
        using var response = content.CreateResponse(statusCode);
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, SomeApiOption);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await api.GetGeolocationAsync(SomeInput, cancellationToken);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetFailureResponseTestData), MemberType = typeof(HttpResponseTestSource))]
    public static async Task GetGeolocationAsync_HttpResponseIsFailure_ExpectFailure(
        HttpStatusCode statusCode, HttpContent content, Failure<GeolocationGetFailureCode> expected)
    {
        using var response = content.CreateResponse(statusCode);
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var mockAuthTokenGetFunc = CreateMockAuthTokenGetFunc(SomeAuthenticationHeaderValue);

        var api = new GeolocationApi(messageHandler, mockAuthTokenGetFunc.Object, SomeApiOption);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await api.GetGeolocationAsync(SomeInput, cancellationToken);

        Assert.Equal(expected, actual);
    }
}