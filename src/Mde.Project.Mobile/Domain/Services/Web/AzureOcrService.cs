using System.Net.Http.Headers;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;
public class AzureOcrService : IOcrService
{
    private readonly string _endpoint;
    private readonly string _subscriptionKey;

    public AzureOcrService(string endpoint, string subscriptionKey)
    {
        _endpoint = endpoint;
        _subscriptionKey = subscriptionKey;
    }

    public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

      
        string url = $"{_endpoint}/vision/v3.2/read/analyze";

        using (var content = new StreamContent(pdfStream))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return ParseOcrResult(responseBody);
        }
    }

    private string ParseOcrResult(string responseBody)
    {
      
        return ""; // Tijdelijke placeholder
    }
}
