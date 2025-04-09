using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.IntegrationTests.Abstractions.Extensions;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static Task<ErrorResponse> GetErrorResponseAsync(
        this HttpResponseMessage response)
    {
        return response.GetFailureResponseAsync<ErrorResponse>();
    }

    public static Task<ErrorResponse<List<ValidationError>>> GetValidationErrorResponseAsync(
        this HttpResponseMessage response)
    {
        return response.GetFailureResponseAsync<ErrorResponse<List<ValidationError>>>();
    }

    public static async Task<TResponse?> GetContentAsync<TResponse>(this HttpResponseMessage response)
    {
        TResponse? content = await response
            .Content
            .ReadFromJsonAsync<TResponse>(JsonSerializerOptions);

        return content;
    }

    private static async Task<TFailureResponse> GetFailureResponseAsync<TFailureResponse>(
        this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Successful response");
        }

        TFailureResponse? failureResponse = await response
            .Content
            .ReadFromJsonAsync<TFailureResponse>();

        ArgumentNullException.ThrowIfNull(failureResponse);

        return failureResponse;
    }
}
