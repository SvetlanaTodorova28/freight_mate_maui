using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using SkiaSharp;


namespace Mde.Project.Mobile.Domain.Services.Web;

public class AzureOcrService : IOcrService
{
    private readonly HttpClient _httpClient;
   

    public AzureOcrService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        
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
           

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
               
                throw new HttpRequestException($"OCR request failed: {response.StatusCode}");
            }

            if (!response.Headers.TryGetValues("Operation-Location", out var operationLocations))
            {
               
                throw new InvalidOperationException("Operation-Location header not found in response.");
            }

            var operationLocation = operationLocations.FirstOrDefault();
           

            return await WaitForOcrResultsAsync(operationLocation);
        }
        catch (HttpRequestException ex)
        {
           
            throw;
        }
        catch (Exception ex)
        {
           
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
                request.Headers.Add("Ocp-Apim-Subscription-Key", SecureStorageHelper.GetApiKey("Key_OCR"));

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                   
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
                
                throw new InvalidOperationException("Failed to parse OCR response JSON.", ex);
            }

            await Task.Delay(1000); 
        }

        
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

       
        return null;
    }
    
    public async Task<string> ExtractTextFromImageAsync(Stream imageStream){
    var resizedStream =  ResizeImage(imageStream, 600, 600);

    if (resizedStream == null) throw new ArgumentNullException(nameof(resizedStream));

    if (resizedStream.CanSeek)
    {
        resizedStream.Position = 0;
    }

    try
    {
        
        using var content = new StreamContent(resizedStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        string requestUrl = "https://cargos.cognitiveservices.azure.com/vision/v3.2/ocr"; 
       

        var response = await _httpClient.PostAsync(requestUrl, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        
        if (response.IsSuccessStatusCode)
        {
            string extractedText = ParseOcrImageResponse(responseBody);
            return extractedText;
            
            
        }
        throw new HttpRequestException($"OCR image request failed: {response.StatusCode}");
        
    }
    catch (HttpRequestException ex)
    {
        throw;
    }

    }

    private Stream ResizeImage(Stream inputImageStream, int targetWidth, int targetHeight)
    {
        using var inputStream = new SKManagedStream(inputImageStream);
        using var original = SKBitmap.Decode(inputStream);
        var aspectRatio = original.Width / (float)original.Height;
        int width, height;

        if (original.Width > original.Height)
        {
            width = targetWidth;
            height = (int)(width / aspectRatio);
        }
        else
        {
            height = targetHeight;
            width = (int)(height * aspectRatio);
        }

        using var image = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
        var resizedStream = new MemoryStream();
        if (image != null)
        {
            using var skImage = SKImage.FromBitmap(image);
            skImage.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(resizedStream);
            resizedStream.Position = 0;  
        }
        return resizedStream;
    }


    private string ParseOcrImageResponse(string responseBody){
    var extractedText = new StringBuilder();
    var jsonDoc = JsonDocument.Parse(responseBody);

    var root = jsonDoc.RootElement;
    if (root.TryGetProperty("regions", out var regions))
    {
        foreach (var region in regions.EnumerateArray())
        {
            if (region.TryGetProperty("lines", out var lines))
            {
                foreach (var line in lines.EnumerateArray())
                {
                    if (line.TryGetProperty("words", out var words))
                    {
                        foreach (var word in words.EnumerateArray())
                        {
                            if (word.TryGetProperty("text", out var text))
                            {
                                extractedText.Append(text.GetString());
                                extractedText.Append(" ");
                            }
                        }
                    }
                    extractedText.AppendLine();
                }
            }
        }
    }

    return extractedText.ToString().Trim();
}

}
