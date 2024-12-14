using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Mde.Project.Mobile.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Mobile.WebAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly SecretClient _secretClient;

    public ConfigurationController()
    {
        var credential = new DefaultAzureCredential();
        _secretClient = new SecretClient(new Uri("https://layka.vault.azure.net/"), credential);
    }

    [HttpGet]
    [Route("azure-keys")]
    public async Task<IActionResult> GetConfiguration()
    {
        try
        {
            var ocrKey = await _secretClient.GetSecretAsync("key-ocr");
            var speechKey = await _secretClient.GetSecretAsync("key-speech");
            var translationKey = await _secretClient.GetSecretAsync("key-translation");

            
            return Ok(new ConfigurationResponseDto
            {
                Key_OCR = ocrKey.Value.Value,
                Key_Speech = speechKey.Value.Value,
                Key_Translation = translationKey.Value.Value
            });
        }
        catch (Azure.RequestFailedException ex)
        {
            return StatusCode(500, $"Failed to retrieve keys: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }


}
