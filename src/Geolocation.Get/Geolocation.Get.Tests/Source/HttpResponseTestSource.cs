using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra.Azure.Services.AzureUser.Get.Tests;

internal static partial class HttpResponseTestSource
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static HttpResponseTestSource()
        =>
        SerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    private static StringContent CreateJsonContent<T>(this T data)
        =>
        new(JsonSerializer.Serialize(data, SerializerOptions), default, "application/json");

    private static StringContent CreateTextContent(this string text)
        =>
        new(text);
}