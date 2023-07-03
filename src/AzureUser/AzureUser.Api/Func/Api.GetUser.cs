using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace GarageGroup.Infra;

partial class AzureUserApi
{
    public ValueTask<Result<AzureUserGetOut, Failure<Unit>>> GetUserAsync(AzureUserMeGetIn input, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<AzureUserGetOut, Failure<Unit>>>(cancellationToken);
        }

        return InnerInvokeAsync(input, cancellationToken);
    }

    private async ValueTask<Result<AzureUserGetOut, Failure<Unit>>> InnerInvokeAsync(
        AzureUserMeGetIn input, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient(httpMessageHandler, disposeHandler: false)
        {
            BaseAddress = GraphApiBaseAddress
        };

        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", new(input.AccessToken));

        var response = await httpClient.GetAsync(ApiNames.MeRelativeUrl, cancellationToken).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode is false)
        {
            var failureMessage = Invariant($"User data request finished with an unexpected error: {response.StatusCode} {json}");
            return Failure.Create(failureMessage);
        }

        var userJson = JsonSerializer.Deserialize<UserJsonGetOut>(json);

        return new AzureUserGetOut(
            id: userJson.Id,
            mail: userJson.Mail,
            displayName: userJson.DisplayName);
    }
}