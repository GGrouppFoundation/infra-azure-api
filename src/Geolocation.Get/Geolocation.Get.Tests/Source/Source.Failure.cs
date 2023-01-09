using System;
using System.Collections.Generic;
using System.Net;

namespace GGroupp.Infra.Azure.Services.AzureUser.Get.Tests;

partial class HttpResponseTestSource
{
    public static IEnumerable<object?[]> GetFailureResponseTestData()
        =>
        new[]
        {
            new object?[]
            {
                HttpStatusCode.BadRequest,
                null,
                Failure.Create(
                    GeolocationGetFailureCode.Unknown, GetDefaultFailureMessage("BadRequest"))
            },
            new object?[]
            {
                HttpStatusCode.TooManyRequests,
                string.Empty.CreateTextContent(),
                Failure.Create(
                    GeolocationGetFailureCode.TooManyRequests, GetDefaultFailureMessage("TooManyRequests"))
            },
            new object?[]
            {
                HttpStatusCode.InternalServerError,
                "Some text".CreateTextContent(),
                Failure.Create(
                    GeolocationGetFailureCode.Unknown, "Some text")
            },
            new object?[]
            {
                HttpStatusCode.BadGateway,
                new StubErrorDetailJson
                {
                    Details = new StubErrorDetailJson[]
                    {
                        new()
                        {
                            Message = string.Empty
                        }
                    }
                }
                .CreateJsonContent(),
                Failure.Create(
                    GeolocationGetFailureCode.Unknown, GetDefaultFailureMessage("BadGateway"))
            },
            new object?[]
            {
                HttpStatusCode.TooManyRequests,
                new StubFailureJson
                {
                    Error = new()
                    {
                        Message = "Some failure message. ",
                        Details = new StubErrorDetailJson[]
                        {
                            new()
                            {
                                Message = "  Some first message  "
                            },
                            new()
                            {
                                Message = "Some second message."
                            }
                        }
                    }
                }
                .CreateJsonContent(),
                Failure.Create(
                    GeolocationGetFailureCode.TooManyRequests, "Some failure message. Some first message. Some second message")
            },
            new object?[]
            {
                HttpStatusCode.Unauthorized,
                new StubFailureJson
                {
                    Error = new()
                    {
                        Message = "Some Failure Message."
                    }
                }
                .CreateJsonContent(),
                Failure.Create(
                    GeolocationGetFailureCode.Unknown, "Some Failure Message")
            }
        };

    private static string GetDefaultFailureMessage(string statusCode)
        =>
        "An unexpected geolocation http response: " + statusCode;
}