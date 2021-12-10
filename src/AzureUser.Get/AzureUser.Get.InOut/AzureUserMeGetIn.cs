namespace GGroupp.Platform;

public readonly record struct AzureUserMeGetIn
{
    private readonly string? accessToken;

    public AzureUserMeGetIn(string accessToken)
        =>
        this.accessToken = string.IsNullOrEmpty(accessToken) ? default : accessToken;

    public string AccessToken => accessToken ?? string.Empty;
}