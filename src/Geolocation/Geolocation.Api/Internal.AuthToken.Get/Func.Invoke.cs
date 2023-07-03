using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class AuthTokenGetFunc
{
    public async Task<AuthenticationHeaderValue> InvokeAsync(
        GeolocationApiCredential credential, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(credential);

        var token = await GetClientApplication(credential).AcquireTokenForClient(Scopes).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        return new(token.TokenType, token.AccessToken);
    }
}
