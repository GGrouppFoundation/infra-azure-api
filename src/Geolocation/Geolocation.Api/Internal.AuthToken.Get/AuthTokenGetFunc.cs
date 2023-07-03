using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;

namespace GarageGroup.Infra;

internal sealed partial class AuthTokenGetFunc : IAsyncFunc<GeolocationApiCredential, AuthenticationHeaderValue>
{
    public static AuthTokenGetFunc Instance { get; }

    static AuthTokenGetFunc()
    {
        Scopes = new[] { "https://atlas.microsoft.com/.default" };
        ClientApplications = new();
        Instance = new();
    }

    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private static readonly string[] Scopes;

    private static readonly Dictionary<GeolocationApiCredential, IConfidentialClientApplication> ClientApplications;

    private AuthTokenGetFunc()
    {
    }

    private static IConfidentialClientApplication GetClientApplication(GeolocationApiCredential credential)
    {
        if (ClientApplications.TryGetValue(credential, out var existedApplication))
        {
            return existedApplication;
        }

        var application = InnerCreateClientApplication();
        ClientApplications[credential] = application;

        return application;

        IConfidentialClientApplication InnerCreateClientApplication()
            =>
            ConfidentialClientApplicationBuilder.Create(credential.ClientId)
            .WithClientSecret(credential.ClientSecret)
            .WithAuthority(LoginMsOnlineServiceBaseUrl + credential.TenantId)
            .Build();
    }
}