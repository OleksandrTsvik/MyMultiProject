using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.FunctionalTests.Abstractions.Extensions;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static Task<ErrorResponse<List<ValidationError>>> GetErrorResponseAsync(
        this HttpResponseMessage response)
    {
        return GetErrorResponseAsync<List<ValidationError>>(response);
    }

    public static async Task<ErrorResponse<TDetails>> GetErrorResponseAsync<TDetails>(
        this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Successful response");
        }

        ErrorResponse<TDetails>? errorResponse = await response
            .Content
            .ReadFromJsonAsync<ErrorResponse<TDetails>>();

        ArgumentNullException.ThrowIfNull(errorResponse);

        return errorResponse;
    }

    public static async Task<TResponse?> GetContentAsync<TResponse>(this HttpResponseMessage response)
    {
        TResponse? content = await response
            .Content
            .ReadFromJsonAsync<TResponse>(JsonSerializerOptions);

        return content;
    }
}
