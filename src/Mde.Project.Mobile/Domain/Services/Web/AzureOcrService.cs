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
    private bool _initialized = false;
    private string _ocrApiKey;
    private bool _headersSetUp = false;

    public AzureOcrService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    private async Task<ServiceResult<bool>> InitializeAsync()
    {
        if (!_initialized)
        {
            _ocrApiKey = await SecureStorageHelper.GetApiKeyAsync("Key_OCR");
            if (string.IsNullOrEmpty(_ocrApiKey))
            {
                return ServiceResult<bool>.Failure("OCR API key not found in secure storage.");
            }
            _initialized = true;
        }

        if (!_headersSetUp)
        {
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocrApiKey);
            _headersSetUp = true;
        }
        return ServiceResult<bool>.Success(true);
    }


    public async Task<ServiceResult<string>> ExtractTextFromPdfAsync(Stream pdfStream)
    {
        var initResult = await InitializeAsync();
        if (!initResult.IsSuccess)
        {
            return ServiceResult<string>.Failure(initResult.ErrorMessage);
        }
        if (pdfStream == null) 
        {
            return ServiceResult<string>.Failure("PDF stream cannot be null.");
        }

        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0;
        }

       
        using var content = new StreamContent(pdfStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        string requestUrl = "vision/v3.2/read/analyze";
        var response = await _httpClient.PostAsync(requestUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            return ServiceResult<string>.Failure($"OCR request failed: {response.StatusCode}, Details: {errorResponse}");
        }

        if (!response.Headers.TryGetValues("Operation-Location", out var operationLocations))
        {
            return ServiceResult<string>.Failure("Operation-Location header not found in response.");
        }
        var operationLocation = operationLocations.FirstOrDefault();
        if (string.IsNullOrEmpty(operationLocation))
        {
            return ServiceResult<string>.Failure("Operation location is not valid or missing.");
        }
        return await WaitForOcrResultsAsync(operationLocation);
      
    }
    
    private async Task<ServiceResult<string>> WaitForOcrResultsAsync(string operationLocation)
    {
        if (string.IsNullOrEmpty(operationLocation))
        {
            return ServiceResult<string>.Failure("Operation location cannot be null or empty.");
        }
        
        if (string.IsNullOrEmpty(_ocrApiKey))
        {
            var initResult = await InitializeAsync();
            if (!initResult.IsSuccess)
            {
                return ServiceResult<string>.Failure(initResult.ErrorMessage);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, operationLocation);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _ocrApiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await Task.Delay(1000); 
                continue;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var ocrResult = ParseOcrResponse(responseBody);
            if (ocrResult.IsSuccess)
            {
                return ServiceResult<string>.Success(ocrResult.Data);
            }
            else
            {
                return ServiceResult<string>.Failure(ocrResult.ErrorMessage);
            }

        }

        return ServiceResult<string>.Failure("OCR processing timed out.");
    }


    private ServiceResult<string> ParseOcrResponse(string responseBody)
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

            if (extractedText.Count > 0)
            {
                return ServiceResult<string>.Success(string.Join("\n", extractedText));
            }
            else
            {
                return ServiceResult<string>.Failure("OCR data is present but contains no readable text.");
            }
        }

        return ServiceResult<string>.Failure("OCR processing did not succeed or returned an unexpected format.");
    }

    
    public async Task<ServiceResult<string>> ExtractTextFromImageAsync(Stream imageStream)
    {
        var resizedStream = ResizeImage(imageStream, 600, 600);

        if (resizedStream == null)
        {
            return ServiceResult<string>.Failure("Resized image stream is null.");
        }

        if (resizedStream.CanSeek)
        {
            resizedStream.Position = 0;
        }

        using var content = new StreamContent(resizedStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        string requestUrl = "https://cargos.cognitiveservices.azure.com/vision/v3.2/ocr";

        try
        {
            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var parsedResult = ParseOcrImageResponse(responseBody);
                if (parsedResult.IsSuccess)
                {
                    return ServiceResult<string>.Success(parsedResult.Data);
                }
                return ServiceResult<string>.Failure("OCR parsing successful but no text was extracted.");
            }
            else
            {
                var errorMessage = $"OCR image request failed: {response.StatusCode}, {response.ReasonPhrase}";
                return ServiceResult<string>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"An error occurred while processing the OCR request: {ex.Message}");
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


    private ServiceResult<string> ParseOcrImageResponse(string responseBody)
    {
        try
        {
            var jsonDoc = JsonDocument.Parse(responseBody);
            var root = jsonDoc.RootElement;
            if (root.TryGetProperty("regions", out var regions))
            {
                StringBuilder extractedText = new StringBuilder();
                foreach (var region in regions.EnumerateArray())
                {
                    foreach (var line in region.GetProperty("lines").EnumerateArray())
                    {
                        foreach (var word in line.GetProperty("words").EnumerateArray())
                        {
                            extractedText.Append(word.GetProperty("text").GetString());
                            extractedText.Append(' ');
                        }
                        extractedText.AppendLine();
                    }
                }
                return ServiceResult<string>.Success(extractedText.ToString().Trim());
            }
            return ServiceResult<string>.Failure("No 'regions' property found in OCR response.");
        }
        catch (JsonException ex)
        {
            return ServiceResult<string>.Failure($"Failed to parse OCR response JSON: {ex.Message}");
        }
    }


}