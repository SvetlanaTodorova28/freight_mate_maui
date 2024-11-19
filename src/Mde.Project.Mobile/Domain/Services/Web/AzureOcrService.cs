using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Utilities;


namespace Mde.Project.Mobile.Domain.Services.Web;

public class AzureOcrService : IOcrService{
   
    private readonly HttpClient _httpClient;

    public AzureOcrService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

 public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
{
    try
    {
        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0; // Reset de positie
        }

        using (var content = new StreamContent(pdfStream))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            // Stap 1: Verstuur de PDF naar de API
            string requestUrl = "vision/v3.2/read/analyze?";
            Console.WriteLine($"Sending OCR request to: {_httpClient.BaseAddress}{requestUrl}");

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"OCR request failed: {response.StatusCode}");
                return null;
            }

            // Haal de Operation-Location header op
            if (!response.Headers.Contains("Operation-Location"))
            {
                Console.WriteLine("Operation-Location header ontbreekt.");
                throw new InvalidOperationException("Geen Operation-Location header ontvangen.");
            }

            string operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            Console.WriteLine($"Operation-Location: {operationLocation}");

            // Stap 2: Wacht op resultaten
            return await WaitForOcrResultsAsync(operationLocation);
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"HTTP Request Error: {ex.Message}");
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in OCR extraction: {ex.Message}");
        return null;
    }
}


private async Task<string> WaitForOcrResultsAsync(string operationLocation)
{
    for (int i = 0; i < 10; i++) // Maximaal 10 pogingen
    {
        var request = new HttpRequestMessage(HttpMethod.Get, operationLocation);
        request.Headers.Add("Ocp-Apim-Subscription-Key", GlobalConstants.Key_OCR);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Body at attempt {i}: {responseBody}");

            try
            {
                using (var jsonDoc = JsonDocument.Parse(responseBody))
                {
                    var root = jsonDoc.RootElement;

                    // Controleer of "status" aanwezig is
                    if (!root.TryGetProperty("status", out var statusProperty))
                    {
                        Console.WriteLine("JSON does not contain 'status'.");
                        continue;
                    }

                    var status = statusProperty.GetString();
                    Console.WriteLine($"OCR status: {status}");

                    if (status == "succeeded")
                    {
                        Console.WriteLine($"OCR succeeded at attempt {i}");

                        // Controleer of "analyzeResult" bestaat
                        if (!root.TryGetProperty("analyzeResult", out var analyzeResult))
                        {
                            Console.WriteLine("JSON does not contain 'analyzeResult'.");
                            continue;
                        }

                        // Controleer of "readResults" bestaat
                        if (!analyzeResult.TryGetProperty("readResults", out var readResults))
                        {
                            Console.WriteLine("JSON does not contain 'readResults'.");
                            continue;
                        }

                        // Extracteer tekst uit "lines"
                        var extractedText = new List<string>();
                        foreach (var page in readResults.EnumerateArray())
                        {
                            if (!page.TryGetProperty("lines", out var lines)) continue;

                            foreach (var line in lines.EnumerateArray())
                            {
                                if (line.TryGetProperty("text", out var text))
                                {
                                    extractedText.Add(text.GetString());
                                }
                            }
                        }

                        // Combineer de lijnen tot een enkele string
                        return string.Join("\n", extractedText);
                    }
                    else if (status == "failed")
                    {
                        Console.WriteLine("OCR processing failed.");
                        return null; // Verlaat de functie
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                Console.WriteLine($"Response Body: {responseBody}");
                throw;
            }
        }

        // Wacht 1 seconde voor de volgende poging
        await Task.Delay(1000);
    }

    Console.WriteLine("OCR processing timed out.");
    return null;
}




}


public class OcrResponse
{
    public string Status { get; set; }
    public AnalyzeResult AnalyzeResult { get; set; }
    public string CreatedDateTime { get; set; } // Extra veld
    public string LastUpdatedDateTime { get; set; } // Extra veld
}

public class AnalyzeResult
{
    public string Version { get; set; } // Extra veld
    public string ModelVersion { get; set; } // Extra veld
    public List<ReadResult> ReadResults { get; set; }
}

public class ReadResult
{
    public int Page { get; set; }
    public double Angle { get; set; } // Extra veld
    public double Width { get; set; } // Extra veld
    public double Height { get; set; } // Extra veld
    public string Unit { get; set; } // Extra veld
    public List<Line> Lines { get; set; }
}

public class Line
{
    public string Text { get; set; }
    public List<double> BoundingBox { get; set; } // Extra veld
    public Appearance Appearance { get; set; } // Extra veld
    public List<Word> Words { get; set; }
}

public class Word
{
    public string Text { get; set; }
    public float? Confidence { get; set; }
    public List<double> BoundingBox { get; set; } // Extra veld
}

public class Appearance
{
    public Style Style { get; set; } // Extra veld
}

public class Style
{
    public string Name { get; set; }
    public float Confidence { get; set; }
}
