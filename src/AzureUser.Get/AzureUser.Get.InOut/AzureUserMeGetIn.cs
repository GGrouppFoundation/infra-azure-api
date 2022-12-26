namespace GGroupp.Infra;

public sealed record class AzureUserMeGetIn
{
    public AzureUserMeGetIn(string accessToken)
        =>
        AccessToken = accessToken ?? string.Empty;

    public string AccessToken { get; }
}