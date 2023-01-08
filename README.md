# infra-azure-services-api

# Geolocation API:
# appsettings.json example:
{
    "Azure.Geolocation": {
      "ClientId": "3689f36e-a64d-4d50-b13d-c736e8a99e30",
      "Credential": {
        "TenantId": "69879a40-68bb-4b30-941b-eb9692ddd9b4",
        "ClientId": "ce83e477-1a1a-46e9-a364-b71979c644de",
        "ClientSecret": ""
      }
    }
}

# Dependency example:
private static Dependency<IGeolocationGetFunc> UseGeolocationGetFunc()
    =>
    PrimaryHandler.UseStandardSocketsHttpHandler()
    .UseLogging("GeolocationGetFunc")
    .UseGeolocationGetFunc();