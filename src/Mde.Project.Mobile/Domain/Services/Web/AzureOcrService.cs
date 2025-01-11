using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using SkiaSharp;

namespace Mde.Project.Mobile.Domain.Services.Web
{
    public class AzureOcrService : IOcrService
    {
        private readonly HttpClient _httpClient;
        private Lazy<Task<string>> _lazyOcrApiKey;
        

        public AzureOcrService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _lazyOcrApiKey = new Lazy<Task<string>>(GetOcrApiKeyAsync);
        }
        
        private async Task<string> GetOcrApiKeyAsync()
        {
            var apiKey = await SecureStorageHelper.GetApiKeyAsync("Key_OCR");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OCR API key not found in secure storage.");
            }
            return apiKey;
        }


        public async Task<ServiceResult<string>> ExtractTextFromPdfAsync(Stream pdfStream)
        {
            if (pdfStream == null) return ServiceResult<string>.Failure("PDF stream cannot be null.");

            if (pdfStream.CanSeek) pdfStream.Position = 0;
            var ocrApiKey = await _lazyOcrApiKey.Value;
            try
            {
                using var content = new StreamContent(pdfStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                string requestUrl = "vision/v3.2/read/analyze";
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocrApiKey);

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<string>.Failure("Failed to process the PDF file. Please try again later.");
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
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure("An unexpected error occurred while extracting pdf file. Please contact support if the problem persists.");
            }
        }

        private async Task<ServiceResult<string>> WaitForOcrResultsAsync(string operationLocation)
        {
            if (string.IsNullOrEmpty(operationLocation))
            {
                return ServiceResult<string>.Failure("Operation location cannot be null or empty.");
            }

            var ocrApiKey = await _lazyOcrApiKey.Value;
            for (int i = 0; i < 10; i++) 
            {
                var request = new HttpRequestMessage(HttpMethod.Get, operationLocation);
                request.Headers.Add("Ocp-Apim-Subscription-Key", ocrApiKey);

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
                    return ocrResult;
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

                if (extractedText.Any())
                {
                    return ServiceResult<string>.Success(string.Join("\n", extractedText));
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
            var ocrApiKey = await _lazyOcrApiKey.Value;
            try
            {
                using var content = new StreamContent(resizedStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                string requestUrl = "https://cargos.cognitiveservices.azure.com/vision/v3.2/ocr";
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocrApiKey);

                var response = await _httpClient.PostAsync(requestUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var parsedResult = ParseOcrImageResponse(responseBody);
                    return parsedResult;
                }
                else
                {
                    return ServiceResult<string>.Failure("OCR image request failed");
                }
                
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure("An error occurred while processing the OCR request:. Please contact support if the problem persists.");
            }
        }

        private ServiceResult<string> ParseOcrImageResponse(string responseBody)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("regions", out var regions))
                {
                    var extractedText = new StringBuilder();
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
                return ServiceResult<string>.Failure("Failed to parse OCR response JSON. Please contact support if the problem persists.");
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

            using var resizedBitmap = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
            var resizedStream = new MemoryStream();
            if (resizedBitmap != null)
            {
                using var skImage = SKImage.FromBitmap(resizedBitmap);
                skImage.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(resizedStream);
                resizedStream.Position = 0;
            }

            return resizedStream;
        }
    }
}