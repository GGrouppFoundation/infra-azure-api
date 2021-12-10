using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace GGroupp.Platform;

partial class AzureUserMeGetFunc
{
    public ValueTask<Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>> InvokeAsync(
        AzureUserMeGetIn input, CancellationToken cancellationToken = default)
        =>
        cancellationToken.IsCancellationRequested is false
        ? InnerInvokeAsync(input, cancellationToken)
        : ValueTask.FromCanceled<Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>>(cancellationToken);

    private async ValueTask<Result<AzureUserGetOut, Failure<AzureUserGetFailureCode>>> InnerInvokeAsync(
        AzureUserMeGetIn input, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient(httpMessageHandler, disposeHandler: false)
        {
            BaseAddress = configurationProvider.Invoke().GraphApiBaseAddress
        };
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", new(input.AccessToken));

        var response = await httpClient.GetAsync(ApiNames.MeRelativeUrl, cancellationToken).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode is false)
        {
            var failureMessage = Invariant($"User data request finished with an unexpected error: {response.StatusCode} {json}");
            return Failure.Create(AzureUserGetFailureCode.Unknown, failureMessage);
        }

        var userJson = JsonSerializer.Deserialize<UserJsonGetOut>(json);
        return new AzureUserGetOut(
            id: userJson.Id,
            mail: userJson.Mail,
            displayName: userJson.DisplayName);
    }
}