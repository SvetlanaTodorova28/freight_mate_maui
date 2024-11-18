using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
        var ocrResult = JsonSerializer.Deserialize<OcrResponse>(responseBody);
        if (ocrResult?.AnalyzeResult?.ReadResults == null) return "";

        StringBuilder extractedText = new StringBuilder();
        foreach (var page in ocrResult.AnalyzeResult.ReadResults)
        {
            foreach (var line in page.Lines)
            {
                extractedText.AppendLine(line.Text);
            }
        }
        return extractedText.ToString();
    }

    public class OcrResponse
    {
        public string Status { get; set; }
        public AnalyzeResult AnalyzeResult { get; set; }
    }

    public class AnalyzeResult
    {
        public List<ReadResult> ReadResults { get; set; }
    }

    public class ReadResult
    {
        public int Page { get; set; }
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public string Text { get; set; }
        public List<Word> Words { get; set; }
    }

    public class Word
    {
        public string Text { get; set; }
        public float Confidence { get; set; }
    }

}
