using Azure.Messaging.ServiceBus;
using BarcodeStandard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json;

namespace FunctionApp1;

public class GeradorCodigoBarras
{
    private readonly ILogger<GeradorCodigoBarras> _logger;
    private readonly string _serviceBusConnectiionString;
    private readonly string _queueName = "gerador-codigo-barras";

    public GeradorCodigoBarras(ILogger<GeradorCodigoBarras> logger)
    {
        _logger = logger;
        _serviceBusConnectiionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? string.Empty;
    }

    [Function("barcode-generate")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            // Tratamento do corpo da requisi��o
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(requestBody); // Explicitly specify the type as dynamic

            if (data == null)
            {
                _logger.LogError("Request body is null or invalid.");
                return new BadRequestObjectResult("Invalid request body.");
            }

            string? valor = data?.valor;
            string? dataVencimento = data?.dataVencimento;

            // Valida��o dos par�metros
            if (string.IsNullOrEmpty(valor) || string.IsNullOrEmpty(dataVencimento))
            {
                _logger.LogError("Valor or dataVencimento is null or empty.");
                return new BadRequestObjectResult("Valor and dataVencimento are required.");
            }
            
            if (!decimal.TryParse(valor, out decimal valorDecimal) || !DateTime.TryParseExact(dataVencimento, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dataVencDate))
            {
                _logger.LogError("Valor ou formto da dataVencimento est�(�o) inv�lido(s).");
                return new BadRequestObjectResult("Valor ou formto da dataVencimento est�(�o) inv�lido(s).");
            }

            // Tratamento do valor para centavos e formata��o de at� 8 casas decimais
            int valorCentavos = (int)(Decimal.Parse(valor) * 100);
            string valorStr = valorCentavos.ToString("D8");
            string dataStr = dataVencDate.ToString("ddMMyyyy");

            // Example bank code
            string bankCode = "008";
            // Gera��o do c�digo de barras com 44 caracteres, preenchendo com zeros � direita se necess�rio
            string baseCode = $"{bankCode}{dataStr}{valorStr}";
            string barcodeData = baseCode.Length < 44 ? baseCode.PadRight(44, '0').Substring(0, 44) : baseCode.Substring(0, 44);
            _logger.LogInformation($"Generated barcode: {barcodeData}");

            // Criar o barcode
            Barcode barcode = new ();
            var skImage = barcode.Encode(BarcodeStandard.Type.Code128, barcodeData);

            using var encodeData = skImage.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);            
            byte[] imageBytes = encodeData.ToArray();
            var resultObject = new
            {
                barcode = barcodeData,
                valorOriginal = 100.00,
                dataVencimento = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"),
                imageBase64 = imageBytes
            };
            await SenderFileFallback(resultObject, _serviceBusConnectiionString, _queueName);
            return new OkObjectResult(resultObject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
    }

    private async Task SenderFileFallback(object resultObject, string serviceBusConnectiionString, string queueName)
    {
        await using var client = new ServiceBusClient(serviceBusConnectiionString);
        var sender = client.CreateSender(queueName);
        try
        {
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(resultObject));
            await sender.SendMessageAsync(message);
            _logger.LogInformation("Message sent to Service Bus queue successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to Service Bus queue.");
            throw;
        }
    }
}