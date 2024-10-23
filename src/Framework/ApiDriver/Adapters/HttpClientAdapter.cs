using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Framework.ApiDriver.Interfaces;
using Framework.Interfaces.Adapters;

namespace Framework.ApiDriver.Adapters;

public class HttpClientAdapter : IApiDriverAdapter
{
    private readonly HttpClient _httpClient;

    public HttpClientAdapter(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task<ApiResponse> SendRequestAsync(string method, string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var request = new HttpRequestMessage(new HttpMethod(method), endpoint);

        if (body != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await _httpClient.SendAsync(request);

        return new ApiResponse
        {
            StatusCode = (int)response.StatusCode,
            Content = await response.Content.ReadAsStringAsync(),
            Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                .Concat(response.Content.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)))
                .ToDictionary(h => h.Key, h => h.Value)
        };
    }
}
