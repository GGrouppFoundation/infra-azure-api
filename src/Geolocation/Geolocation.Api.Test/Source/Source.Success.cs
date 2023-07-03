using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace GarageGroup.Infra.Azure.Services.Geolocation.Api.Test;

partial class HttpResponseTestSource
{
    public static StringContent CreateSomeSuccessContent()
        =>
        new StubSuccessJson
        {
            CountryRegion = new()
            {
                IsoCode = "GE"
            },
            IpAddress = "45.154.90.243"
        }
        .CreateJsonContent();

    public static IEnumerable<object?[]> GetSuccessResponseTestData()
        =>
        new[]
        {
            new object?[]
            {
                HttpStatusCode.OK,
                new StubSuccessJson
                {
                    CountryRegion = new()
                    {
                        IsoCode = "US"
                    },
                    IpAddress = "45.154.90.243"
                }
                .CreateJsonContent(),
                new GeolocationGetOut(
                    countryIsoCode: "US")
            },
            new object?[]
            {
                HttpStatusCode.Created,
                new StubSuccessJson
                {
                    IpAddress = "2001:db8:3333:4444:5555:6666:102:304"
                }
                .CreateJsonContent(),
                new GeolocationGetOut(
                    countryIsoCode: default)
            },
            new object?[]
            {
                HttpStatusCode.OK,
                null,
                default(GeolocationGetOut)
            },
            new object?[]
            {
                HttpStatusCode.OK,
                string.Empty.CreateTextContent(),
                default(GeolocationGetOut)
            }
        };
}