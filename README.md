# infra-azure-services-api

## Geolocation API:
### appsettings.json example:
```
{
  "Azure.Geolocation": {
    "ClientId": "aa7ef185-eda5-4d69-8265-f6ffc1a75b89",
    "Credential": {
      "TenantId": "129990f8-e0e9-43b9-baca-024c2c0b83ac",
      "ClientId": "4a8a5fe3-a541-4334-b577-9f9f5f0b74e5",
      "ClientSecret": ""
    }
  }
}
```

### Dependency example:
```
private static Dependency<IGeolocationGetFunc> UseGeolocationGetFunc()
    =>
    PrimaryHandler.UseStandardSocketsHttpHandler()
    .UseLogging("GeolocationGetFunc")
    .UseGeolocationGetFunc();
```
