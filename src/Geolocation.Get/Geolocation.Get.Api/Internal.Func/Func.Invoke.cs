using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class GeolocationGetFunc
{
    public ValueTask<Result<GeolocationGetOut, Failure<GeolocationGetFailureCode>>> InvokeAsync(
        GeolocationGetIn input, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<GeolocationGetOut, Failure<GeolocationGetFailureCode>>>(cancellationToken);
        }

        return InnerInvokeAsync(input, cancellationToken);
    }

    private async ValueTask<Result<GeolocationGetOut, Failure<GeolocationGetFailureCode>>> InnerInvokeAsync(
        GeolocationGetIn input, CancellationToken cancellationToken)
    {
        var authHeaderValue = await authTokenGetFunc.InvokeAsync(option.Credential, cancellationToken).ConfigureAwait(false);
        using var httpClient = CreateHttpClient(authHeaderValue);

        using var httpRequest = CreateHttpRequestMessage(input.IpAddress);
        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

        var httpResult = await ReadResultAsync(httpResponse, cancellationToken).ConfigureAwait(false);
        return httpResult.Map(MapSuccess, MapFailure);

        static GeolocationGetOut MapSuccess(GeolocationSuccessJson success)
            =>
            new(
                countryIsoCode: success.CountryRegion.IsoCode);

        Failure<GeolocationGetFailureCode> MapFailure(GeolocationFailureJson failure)
            =>
            new(
                failureCode: GetFailureCode(httpResponse.StatusCode),
                failureMessage: GetFailureMessage(httpResponse.StatusCode, failure));
    }

    private static async Task<Result<GeolocationSuccessJson, GeolocationFailureJson>> ReadResultAsync(
        HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (httpResponse.IsSuccessStatusCode)
        {
            return Deserialize<GeolocationSuccessJson>(json);
        }

        if (httpResponse.Content.Headers.ContentType?.MediaType is not MediaTypeNames.Application.Json)
        {
            return new GeolocationFailureJson
            {
                Error = new()
                {
                    Message = json
                }
            };
        }

        return Deserialize<GeolocationFailureJson>(json);
    }

    private static string GetFailureMessage(HttpStatusCode statusCode, GeolocationFailureJson failure)
    {
        var message = InnerGetFailureMessage(failure);

        if (string.IsNullOrEmpty(message) is false)
        {
            return message;
        }

        return $"An unexpected geolocation http response: {statusCode}";

        static string? InnerGetFailureMessage(GeolocationFailureJson failure)
        {
            if (failure.Error.Details?.Length is not > 0)
            {
                return Trim(failure.Error.Message);
            }

            var messages = new List<string?>(1 + failure.Error.Details.Length)
            {
                failure.Error.Message
            };

            messages.AddRange(failure.Error.Details.Select(GetMessage));

            return string.Join(". ", messages.Select(Trim));
        }

        static string? GetMessage(ErrorDetailJson detail)
            =>
            detail.Message;

        static string? Trim(string? message)
            =>
            message?.Trim(' ').Trim('.');
    }

    private static GeolocationGetFailureCode GetFailureCode(HttpStatusCode statusCode)
        =>
        statusCode switch
        {
            HttpStatusCode.TooManyRequests => GeolocationGetFailureCode.TooManyRequests,
            _ => default
        };
}