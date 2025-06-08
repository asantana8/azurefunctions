FunctionApp1/README.md
# FunctionApp1 - Gerador de C�digo de Barras de Boletos

Este projeto � uma Azure Function desenvolvida em .NET 8 para gera��o de c�digos de barras de boletos banc�rios. Ele recebe dados via requisi��o HTTP, gera o c�digo de barras no padr�o banc�rio, cria uma imagem do c�digo e envia os dados para uma fila do Azure Service Bus.

## Funcionalidades

- Gera��o de c�digo de barras de boleto (44 d�gitos)
- Cria��o de imagem do c�digo de barras (formato PNG, base64)
- Envio dos dados para uma fila do Azure Service Bus

## Como usar

1. Fa�a uma requisi��o HTTP POST para o endpoint da fun��o `barcode-generate` com o seguinte corpo JSON:

2. O retorno ser� um JSON contendo:
- `barcode`: c�digo de barras gerado
- `valorOriginal`: valor informado
- `dataVencimento`: data de vencimento
- `imageBase64`: imagem do c�digo de barras em base64

## Configura��o

- Defina a vari�vel de ambiente `ServiceBusConnectionString` com a string de conex�o do Azure Service Bus.

## Requisitos

- .NET 8
- Azure Functions v4
- Azure Service Bus

---

fnValidaBoleto/README.md
# fnValidaBoleto - Validador de C�digo de Barras de Boletos

Este projeto � uma Azure Function desenvolvida em .NET 8 para valida��o de c�digos de barras de boletos banc�rios. Ele recebe um c�digo de barras via requisi��o HTTP e valida seu formato e data de vencimento.

## Funcionalidades

- Valida��o do tamanho do c�digo de barras (44 d�gitos)
- Valida��o da data de vencimento embutida no c�digo

## Como usar

1. Fa�a uma requisi��o HTTP POST para o endpoint da fun��o `barcode-validate` com o seguinte corpo JSON: { "barcode": "0081506202510000000...0000" }

2. O retorno ser� um JSON indicando se o boleto � v�lido e a data de vencimento extra�da.

## Requisitos

- .NET 8
- Azure Functions v4

---

