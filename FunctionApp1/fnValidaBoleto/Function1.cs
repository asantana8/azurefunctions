using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;

namespace fnValidaBoleto;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("barcode-validate")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        // Tratamento do corpo da requisição
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<dynamic>(requestBody); // Explicitly specify the type as dynamic

        if (data == null)
        {
            _logger.LogError("Request body is null or invalid.");
            return new BadRequestObjectResult("Invalid request body.");
        }

        string? barcodeData = data?.barcode;        

        // Validação dos parâmetros
        if (string.IsNullOrEmpty(barcodeData))
        {
            _logger.LogError("barcode está vazio ou nulo.");
            return new BadRequestObjectResult("barcode está vazio ou nulo!");
        }

        if (barcodeData.Length != 44)
        {
            var result = new { valido = false, mensagem = "O campo barcode deve ter 44 caracteres." };
            return new BadRequestObjectResult(result);
        }

        string datePart = barcodeData.Substring(3, 8); // Data de vencimento no formato ddMMyyyy
        if (!DateTime.TryParseExact(datePart, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateObj))
        {
            var result = new { valido = false, mensagem = "Data de vencimento inválida." };
            return new BadRequestObjectResult(result);
        }

        var resultOK = new { valido = true, mensagem = "Boleto válido.", vecimento = dateObj.ToString("dd-MM-yyyy") };
        return new OkObjectResult(resultOK);
    }
}