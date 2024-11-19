using System.Net.Http.Headers;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class AzureOcrService : IOcrService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AzureOcrService> _logger;

    public AzureOcrService(HttpClient httpClient, ILogger<AzureOcrService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
    {
        if (pdfStream == null) throw new ArgumentNullException(nameof(pdfStream));

        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0;
        }

        try
        {
            using var content = new StreamContent(pdfStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            string requestUrl = "vision/v3.2/read/analyze";
            _logger.LogInformation("Sending OCR request to: {RequestUrl}", _httpClient.BaseAddress + requestUrl);

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("OCR request failed with status code: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"OCR request failed: {response.StatusCode}");
            }

            if (!response.Headers.TryGetValues("Operation-Location", out var operationLocations))
            {
                _logger.LogError("Operation-Location header is missing in the OCR response.");
                throw new InvalidOperationException("Operation-Location header not found in response.");
            }

            var operationLocation = operationLocations.FirstOrDefault();
            _logger.LogInformation("Operation-Location received: {OperationLocation}", operationLocation);

            return await WaitForOcrResultsAsync(operationLocation);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error during OCR processing.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during OCR processing.");
            throw;
        }
    }

    private async Task<string> WaitForOcrResultsAsync(string operationLocation)
    {
        if (string.IsNullOrEmpty(operationLocation))
        {
            throw new ArgumentException("Operation location cannot be null or empty.", nameof(operationLocation));
        }

        for (int i = 0; i < 10; i++) // Max attempts
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, operationLocation);
                request.Headers.Add("Ocp-Apim-Subscription-Key", GlobalConstants.Key_OCR);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Attempt {Attempt}: Received non-success status code: {StatusCode}", i + 1, response.StatusCode);
                    continue;
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                var extractedText = ParseOcrResponse(responseBody);
                if (!string.IsNullOrEmpty(extractedText))
                {
                    return extractedText;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error during OCR result processing.");
                throw new InvalidOperationException("Failed to parse OCR response JSON.", ex);
            }

            await Task.Delay(1000); 
        }

        _logger.LogError("OCR processing timed out after 10 attempts.");
        throw new TimeoutException("OCR processing timed out.");
    }

    private string ParseOcrResponse(string responseBody)
    {
        using var jsonDoc = JsonDocument.Parse(responseBody);
        var root = jsonDoc.RootElement;

        if (root.TryGetProperty("status", out var statusProperty) &&
            statusProperty.GetString() == "succeeded" &&
            root.TryGetProperty("analyzeResult", out var analyzeResult) &&
            analyzeResult.TryGetProperty("readResults", out var readResults))
        {
            var extractedText = new List<string>();

            foreach (var page in readResults.EnumerateArray())
            {
                if (page.TryGetProperty("lines", out var lines))
                {
                    foreach (var line in lines.EnumerateArray())
                    {
                        if (line.TryGetProperty("text", out var text))
                        {
                            extractedText.Add(text.GetString());
                        }
                    }
                }
            }

            return string.Join("\n", extractedText);
        }

        _logger.LogWarning("OCR response does not contain valid 'succeeded' status or readable results.");
        return null;
    }
}
